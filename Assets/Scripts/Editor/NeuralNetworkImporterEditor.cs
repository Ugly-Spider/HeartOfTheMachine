#if UNITY_EDITOR
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using UnityEditor;

namespace HeartOfTheMachine
{

    [CustomEditor(typeof(NeuralNetworkImporter))]
    public class NeuralNetworkImporterEditor : ScriptedImporterEditor
    {

        private NeuralNetwork _neuralNetwork;
        private bool _foldout_overview;
        private bool _foldout_layers;
        
        public override void OnEnable()
        {
            base.OnEnable();
            EditorGUI.indentLevel = 1;
        }

        public override void OnInspectorGUI()
        {
            //0. overview 版本信息，层数信息，输入输出shape
            //1. layers   各层信息, inputShape, outputShape, activationType
            
            var importer = target as NeuralNetworkImporter;
            
            if (ReferenceEquals(_neuralNetwork, null))
            {
                var path = Application.dataPath.Replace("Assets", "") + importer.assetPath;
                _neuralNetwork = NeuralNetwork.Load(path);
            }

            _foldout_overview = EditorGUILayout.Foldout(_foldout_overview, "Overview");
            if (_foldout_overview)
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.LabelField($"Version:{ _neuralNetwork.Version}");
                EditorGUILayout.LabelField($"LayerNum:{ _neuralNetwork.Layers.Count}");
                EditorGUILayout.LabelField($"InputShape:{ _neuralNetwork.InputShape}");
                EditorGUILayout.LabelField($"OutputShape:{ _neuralNetwork.OutputShape}");
                --EditorGUI.indentLevel;
            }

            _foldout_layers = EditorGUILayout.Foldout(_foldout_layers, "Layers");
            if (_foldout_layers)
            {
                for (int i = 0; i < _neuralNetwork.Layers.Count; ++i)
                {
                    var layer = _neuralNetwork.Layers[i];
                    EditorGUILayout.LabelField($"Layer{i + 1}");
                    ++EditorGUI.indentLevel;
                    EditorGUILayout.LabelField($"Index:{i}");
                    EditorGUILayout.LabelField($"Type:{layer.LayerType}");
                    EditorGUILayout.LabelField($"Activation:{layer.activationType}");
                    EditorGUILayout.LabelField($"InputShape:{layer.inputShape}");
                    EditorGUILayout.LabelField($"OutputShape:{layer.outputShape}");
                    --EditorGUI.indentLevel;
                }
            }

            //TODO
//            if (GUILayout.Button("Visualize"))
//            {
//                NetworkVisualizer.ShowWindow(_neuralNetwork);
//            }
        }
    }

    
}

#endif