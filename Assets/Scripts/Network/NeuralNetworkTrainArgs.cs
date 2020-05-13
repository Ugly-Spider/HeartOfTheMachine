using System;
using HeartOfTheMachine;

namespace HeartOfTheMachine
{

    public class NeuralNetworkTrainArgs
    {
        public Tensor[] trainingData;
        public Tensor[] trainingLabels;

        public Tensor[] testData;
        public Tensor[] testLabels;
        public float learningRate;
        public int trainEpoches;

        public Action onFinish;
        public Action<int> onOnceEpoch;
        public Func<Tensor, Tensor, float> getScore;
    }

}