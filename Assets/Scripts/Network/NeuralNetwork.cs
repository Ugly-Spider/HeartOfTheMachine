using System.Collections.Generic;

namespace HeartOfTheMachine
{

    public class NeuralNetwork : INeuralNetwork
    {
        public string Version { get; internal set; }
        
        private List<LayerBase> _layers = new List<LayerBase>();
        public IReadOnlyList<LayerBase> Layers => _layers;
        public NeuralNetworkTrainArgs TrainArgs { get; private set; }

        public Shape InputShape => _layers[0].inputShape;

        public Shape OutputShape => _layers[_layers.Count - 1].outputShape;

        private bool _initialized;

        public void Initialize(NetworkInitializeArgs args)
        {
            if(_initialized) return;

            var inputShape = args.inputShape;
            for (int i = 0; i < _layers.Count; ++i)
            {
                var layer = _layers[i];
                layer.Initial(inputShape, this, i, args);
                inputShape = layer.outputShape;
            }
            
            _initialized = true;
        }

        public NeuralNetwork()
        {
        }

        public void AddLayer(LayerBase layer)
        {
            _layers.Add(layer);
        }

        public void LogDebugInfo()
        {
            foreach (var v in _layers)
            {
                v.LogDebugInfo();
            }
        }

        public void Train(NeuralNetworkTrainArgs trainArgs)
        {
            TrainArgs = trainArgs;
            
            for (int epoch = 0; epoch < trainArgs.trainEpoches; ++epoch)
            {
                trainArgs.onOnceEpoch?.Invoke(epoch);
                for (int i = 0; i < trainArgs.trainingData.Length; ++i)
                {
                    TrainOnce(trainArgs.trainingData[i], trainArgs.trainingLabels[i], trainArgs.learningRate);
                }
            }
            
            
            trainArgs.onOnceEpoch?.Invoke(trainArgs.trainEpoches);
            trainArgs.onFinish?.Invoke();
        }

        public float GetTotalError(Tensor[] inputs, Tensor[] labels)
        {
            float totalError = 0f;
            for (int i = 0; i < inputs.Length; ++i)
            {
                totalError += GetError(inputs[i], labels[i]);
            }
            
            return totalError;
        }

        public float GetError(Tensor input, Tensor label)
        {
            var predict = Forward(input);
            var delta = predict - label;
            var error = 0f;
            delta.ForEach(f => error += f * f);
            error *= 0.5f;
            return error;
        }

        public void TrainOnce(Tensor input, Tensor label, float learningRate)
        {
            var predict = Forward(input);
            var delta = predict - label;
            BackPropagation(delta, learningRate);
        }

        public Tensor Forward(Tensor input)
        {
            var temp = input;
            for (int i = 0; i < _layers.Count; ++i)
            {
                var layer = _layers[i];
                temp = layer.Forward(temp);
            }

            return temp;
        }

        public void BackPropagation(Tensor prevDelta, float learningRate)
        {
            var temp = prevDelta;
            for (int i = _layers.Count - 1; i >= 0; --i)
            {
                var layer = _layers[i];
                temp = layer.BackPropagation(temp, learningRate);
            }
        }

        public void CheckGradient(NeuralNetworkTrainArgs trainArgs)
        {
            TrainArgs = trainArgs;

            var input = trainArgs.trainingData[0];
            var label = trainArgs.trainingLabels[0];
            var predict = Forward(input);
            var delta = predict - label;
            
            var temp = delta;
            for (int i = _layers.Count - 1; i >= 0; --i)
            {
                var layer = _layers[i];
                temp = layer.CheckGradient(temp);
            }
        }

        public void Save(string filePath, string fileName)
        {
            NeuralNetworkHelper.SaveNetwork(this, filePath, fileName);
        }

        public NeuralNetwork Clone()
        {
            var clone = new NeuralNetwork();
            clone._initialized = true;

            foreach (var layer in _layers)
            {
                clone.AddLayer(layer.Clone(this));
            }

            return clone;
        }

        public NeuralNetwork Variant(float min, float max)
        {
            var variant = new NeuralNetwork();
            variant._initialized = true;

            foreach (var layer in _layers)
            {
                variant.AddLayer(layer.Variant(min, max, this));
            }

            return variant;
        }

        public static NeuralNetwork Load(string path)
        {
            return NeuralNetworkHelper.LoadNetwork(path);
        }

        public static void Save(NeuralNetwork neuralNetwork, string filePath, string fileName)
        {
            NeuralNetworkHelper.SaveNetwork(neuralNetwork, filePath, fileName);
        }
    }
    
}