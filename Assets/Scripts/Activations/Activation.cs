namespace HeartOfTheMachine
{

    /// <summary>
    /// Activation function forward and backward
    /// </summary>
    public static class Activation
    {

        public static Tensor Forward(Tensor input, ActivationType activationType)
        {
            return input.Clone(x => Forward(x, activationType));
        }

        public static Tensor Backward(Tensor input, ActivationType activationType)
        {
            return input.Clone(x => Backward(x, activationType));
        }

        public static float Forward(float input, ActivationType activationType)
        {
            float result;
            switch (activationType)
            {
                case ActivationType.None:
                    result = input;
                    break;
                case ActivationType.Sigmoid:
                    result = 1 / (1 + UnityEngine.Mathf.Exp(-input));
                    break;
                case ActivationType.ReLU:
                    result = input >= 0 ? input : 0;
                    break;
                default:
                    result = input;
                    break;
            }
            
            return result;
//            var result = activationType switch
//            {
//                ActivationType.None => input,
//                ActivationType.ReLU => (input >= 0 ? input : 0),
//                _ => input
//            };
//
//            return result;
        }

        public static float Backward(float input, ActivationType activationType)
        {
            float result;
            switch (activationType)
            {
                case ActivationType.None:
                    result = 1;
                    break;
                case ActivationType.Sigmoid:
                    var f = 1 / (1 + UnityEngine.Mathf.Exp(-input));
                    result = f * (1 - f);
                    break;
                case ActivationType.ReLU:
                    result = input >= 0 ? 1 : 0;
                    break;
                default:
                    result = 1;
                    break;
            }
            
            return result;
//            var result = activationType switch
//            {
//                ActivationType.None => 1,
//                ActivationType.ReLU => input >= 0 ? 1 : 0,
//                _ => 1
//            };
//            
//            return result;
        }
    }

}