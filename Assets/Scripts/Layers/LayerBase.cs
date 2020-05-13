using HeartOfTheMachine;

namespace HeartOfTheMachine
{

    public abstract class LayerBase
    {

        public abstract LayerType LayerType { get; }
        public INeuralNetwork neuralNetwork;
        public Shape inputShape;
        public Shape outputShape;
        public ActivationType activationType;
        public int layerIndex;

        public abstract void Initial(Shape inputShape, INeuralNetwork neuralNetwork, int layerIndex, NetworkInitializeArgs args);
        public abstract Tensor Forward(Tensor input);
        public abstract Tensor BackPropagation(Tensor prevDelta, float learningRate);
        public abstract Tensor CheckGradient(Tensor prevDelta);
        public abstract LayerBase Clone(INeuralNetwork neuralNetwork);
        public abstract LayerBase Variant(float min, float max, INeuralNetwork neuralNetwork);

        public abstract void LogDebugInfo();
        internal abstract byte[] Serialize();
        internal abstract void Deserialize(byte[] bytes);
        internal abstract Tensor GetWeights();
        internal abstract Tensor GetBias();
    }
    
}