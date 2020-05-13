//#if UNITY_EDITOR
//using UnityEditor;
//using UnityEngine;
//
//namespace HeartOfTheMachine
//{
//    //TODO
//    public class NetworkVisualizer : EditorWindow
//    {
//        private NeuralNetwork _neuralNetwork;
//        
//        public static void ShowWindow(NeuralNetwork neuralNetwork)
//        {
//            var window = GetWindow<NetworkVisualizer>();
//            window._neuralNetwork = neuralNetwork;
//            window.Show();
//        }
//        
//        private static Vector2 NODE_SIZE = new Vector2(20, 20);
//        private const float LAYER_INTERVAL = 200;
//        private const float NODE_INTERVAL = 40;
//
//        public void OnGUI()
//        {
//            if(_neuralNetwork == null) return;
//
//            var size = new Vector2(20, 20);
//            
//            for (int i = 0; i < _neuralNetwork.Layers.Count; ++i)
//            {
//                var layer = _neuralNetwork.Layers[i];
//                var weights = layer.GetWeights();
//                weights.ForEach((d0, d1, d2, f) =>
//                {
//                    var pos = new Vector2(LAYER_INTERVAL * (i + 1), NODE_INTERVAL * (d1 + 1));
//                    if (GUI.Button(new Rect(pos, size), ""))
//                    {
//                    
//                    }
//                });
//                
//            }
//        }
//    }
//}
//#endif