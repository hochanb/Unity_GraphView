using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace EventGraph
{

    public class EventGraphViewWindow : EditorWindow
    {
        private EventGraphView _graphView;
        private GraphDataAsset _loadedGraphDataAsset;

        [MenuItem("Window/Event Graph")]
        public static void OpenEventGraphWindow()
        {
            var window = GetWindow<EventGraphViewWindow>();
            window.titleContent = new GUIContent("Event Graph");
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }

        private void ConstructGraphView()
        {
            _graphView = new EventGraphView
            {
                name = "Event Graph"
            };

            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void GenerateToolbar()
        {
            var toolbar = new Toolbar();

            var addButton = new Button(() => { _graphView.AddEventNode(new Vector2(200, 200)); })
            {
                text = "Add Event Node"
            };
            var saveButton = new Button(() => { _graphView.SaveGraphData(); })
            {
                text = "Save Graph"
            };

            var loadButton = new Button(LoadGraphData)
            {
                text = "Load Graph Data"
            };
            toolbar.Add(loadButton);

            toolbar.Add(addButton);
            toolbar.Add(saveButton);

            rootVisualElement.Add(toolbar);
        }

        private void LoadGraphData()
        {
            string path = EditorUtility.OpenFilePanel("Load Graph Data", Application.dataPath, "asset");
            if (!string.IsNullOrEmpty(path))
            {
                path = "Assets" + path.Replace(Application.dataPath, "");
                _loadedGraphDataAsset = AssetDatabase.LoadAssetAtPath<GraphDataAsset>(path);
                if (_loadedGraphDataAsset != null)
                {
                    _graphView.LoadGraphData(_loadedGraphDataAsset);
                }
                else
                {
                    Debug.LogError("Failed to load graph data asset.");
                }
            }
        }
    }

}