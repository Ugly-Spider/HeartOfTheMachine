namespace HeartOfTheMachine
{

    /// <summary>
    /// Dense layer
    /// </summary>
    public class DenseLayer : LayerBase
    {
        private Tensor _weights;
        private Tensor _bias;
        private Tensor _netResult;
        private Tensor _input;
        
        internal DenseLayer()
        {
            
        }
        
        /// <summary>
        /// create a dense layer
        /// </summary>
        /// <param name="outputShape"></param>
        /// <param name="activationType"></param>
        public DenseLayer(Shape outputShape, ActivationType activationType = ActivationType.None)
        {
            this.outputShape = outputShape;
            this.activationType = activationType;
        }

        public override LayerType LayerType => LayerType.DenseLayer;

        public override void Initial(Shape inputShape, INeuralNetwork neuralNetwork, int layerIndex, NetworkInitializeArgs args)
        {
            this.inputShape = inputShape;
            this.neuralNetwork = neuralNetwork;
            this.layerIndex = layerIndex;
            
            _weights = new Tensor(1, outputShape.len1, inputShape.len1);
            _weights.FillWithRandomValue(args.initWeightRange.Item1, args.initWeightRange.Item2);
            _bias = new Tensor(1, outputShape.len1, 1);
            _bias.FillWithRandomValue(args.initBiasRange.Item1, args.initBiasRange.Item2);
        }


        public override Tensor Forward(Tensor input)
        {
            _input = input.Clone();
            _netResult = _weights * input + _bias;
            return Activation.Forward(_netResult, activationType);
        }

        public override Tensor BackPropagation(Tensor prevDelta, float learningRate)
        {
            //update current layer's weights and propagation delta to previous layer
            CalculateWeightDelta(prevDelta);
            UpdateWeights(learningRate);
            
            return CalculatePrevDelta(prevDelta);
        }

        private void CalculateWeightDelta(Tensor prevDelta)
        {
            _biasDelta = prevDelta.Clone();
            _weightsDelta = prevDelta * _input.Transpose();
//            UnityEngine.Debug.Log($"[WeightsDelta: {layerIndex}] \\n" +  _weightsDelta.ToString());
        }

        private Tensor CalculatePrevDelta(Tensor prevDelta)
        {
            //the first layer don't need BackPropagation
            if (layerIndex == 0) return null;
            
            return _weights.Transpose() * prevDelta;
        }

        private void UpdateWeights(float learningRate)
        {
            _weights -= _weightsDelta * learningRate;
            _bias -= _biasDelta * learningRate;
        }

        private Tensor _weightsDelta;
        private Tensor _biasDelta;
        private Tensor _checkGradientWeightDelta;

        public override Tensor CheckGradient(Tensor prevDelta)
        {
            CalculateWeightDelta(prevDelta);
            var valsA = _weights.GetValues();
            var valsB = _weightsDelta.GetValues();
            var epsilon = 1e-3f;
            _weights.ForEach((d0, d1, d2, x) =>
            {
                var oldError = neuralNetwork.GetError(neuralNetwork.TrainArgs.trainingData[0], neuralNetwork.TrainArgs.trainingLabels[0]);
                valsA[d0, d1, d2] += epsilon;
                var newError = neuralNetwork.GetError(neuralNetwork.TrainArgs.trainingData[0], neuralNetwork.TrainArgs.trainingLabels[0]);
                var realPartialDerivative = (newError - oldError) / epsilon;
                var predictPartialDerivative = valsB[d0, d1, d2] * 2;
                var info = $"Index:{layerIndex} 真实偏导:{realPartialDerivative}, 计算偏导:{predictPartialDerivative}";
                
                if (UnityEngine.Mathf.Abs(realPartialDerivative - predictPartialDerivative) > 0.001f)
                {
                    UnityEngine.Debug.LogError(info);
                }
                else
                {
                    UnityEngine.Debug.Log(info);
                }
            });
            
            return CalculatePrevDelta(prevDelta);
        }

        public override LayerBase Clone(INeuralNetwork neuralNetwork)
        {
            var clone = new DenseLayer();
            clone.activationType = activationType;
            clone.inputShape = inputShape;
            clone.outputShape = outputShape;
            clone.layerIndex = layerIndex;
            clone.neuralNetwork = neuralNetwork;
            clone._weights = _weights.Clone();
            clone._bias = _bias.Clone();
            return clone;
        }
        
        public override LayerBase Variant(float min, float max, INeuralNetwork neuralNetwork)
        {
            var variant = new DenseLayer();
            variant.activationType = activationType;
            variant.inputShape = inputShape;
            variant.outputShape = outputShape;
            variant.layerIndex = layerIndex;
            variant.neuralNetwork = neuralNetwork;
            variant._weights = _weights.Variant(min, max);
            variant._bias = _bias.Variant(min, max);
            return variant;
        }

        public override void LogDebugInfo()
        {
            UnityEngine.Debug.Log(LayerType.ToString());
            UnityEngine.Debug.Log(outputShape.ToString());
            UnityEngine.Debug.Log(_weights.ToString());
            UnityEngine.Debug.Log(_bias.ToString());
        }

        internal override byte[] Serialize()
        {
            var buff = new NetworkBuffer();
            buff.WriteInt32((int)activationType);
            buff.WriteShape(inputShape);
            buff.WriteShape(outputShape);
            buff.WriteTensor(_weights);
            buff.WriteTensor(_bias);
            var bytes = buff.ToBytes();
            buff.Close();
            return bytes;
        }

        internal override void Deserialize(byte[] bytes)
        {
            var buff = new NetworkBuffer(bytes);
            activationType = (ActivationType)buff.ReadInt32();
            inputShape = buff.ReadShape();
            outputShape = buff.ReadShape();
            _weights = buff.ReadTensor();
            _bias = buff.ReadTensor();
            buff.Close();
        }

        internal override Tensor GetWeights()
        {
            return _weights;
        }

        internal override Tensor GetBias()
        {
            return _bias;
        }
    }
    
}