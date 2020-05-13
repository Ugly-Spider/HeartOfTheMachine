#if UNITY_EDITOR
using UnityEditor.Experimental.AssetImporters;

namespace HeartOfTheMachine
{
    [ScriptedImporter(1, "." + Global.FILE_SUFFIX)]
    public class NeuralNetworkImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
        }
    }

}
#endif