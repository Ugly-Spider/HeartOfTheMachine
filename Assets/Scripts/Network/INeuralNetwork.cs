using System.Collections.Generic;

namespace HeartOfTheMachine
{

    public interface INeuralNetwork
    {
        IReadOnlyList<LayerBase> Layers { get; }

        NeuralNetworkTrainArgs TrainArgs { get; }
        
        void CheckGradient(NeuralNetworkTrainArgs args);

        float GetTotalError(Tensor[] inputs, Tensor[] labels);
        
        float GetError(Tensor inputs, Tensor labels);
    }
    
}