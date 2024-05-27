using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace EventGraph
{


    [CreateAssetMenu(fileName = "GraphData", menuName = "Graph Data", order = 1)]
    public class GraphDataAsset : ScriptableObject
    {
        public GraphData graphData;
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(GraphDataAsset))]
    public class GraphDataAssetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GraphDataAsset graphDataAsset = (GraphDataAsset)target;

            if (GUILayout.Button("Save Graph Data"))
            {
                string path = EditorUtility.SaveFilePanelInProject("Save Graph Data", "New Graph Data", "asset", "Please enter a file name to save the graph data to");
                if (!string.IsNullOrEmpty(path))
                {
                    AssetDatabase.CreateAsset(graphDataAsset, path);
                    AssetDatabase.SaveAssets();
                }
            }
        }
    }
#endif
}