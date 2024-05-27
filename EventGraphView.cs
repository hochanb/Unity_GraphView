using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace EventGraph
{


    public class EventGraphView : GraphView
    {
        public GraphData graphData = new GraphData();



        public EventGraphView()
        {
            // Set up zoom and other basic settings
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            // Add grid background
            var grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            // Add style
            this.styleSheets.Add(Resources.Load<StyleSheet>("EventGraphStyle"));

            // Add contextual menu for adding nodes
            this.AddManipulator(new ContextualMenuManipulator(AddContextualMenu));
        }

        private void AddContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Add Event Node", action => AddEventNode(evt.mousePosition));
        }

        public void AddEventNode(Vector2 position)
        {
            var node = new EventNode("New Event");
            node.SetPosition(new Rect(position, new Vector2(150, 200)));
            AddElement(node);
        }


        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node && startPort.direction != port.direction)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }




        public void SaveGraphData()
        {
            graphData.nodes.Clear();
            graphData.connections.Clear();

            // Collect node data
            foreach (var node in nodes.ToList())
            {
                var nodeData = new NodeData
                {
                    nodeName = node.title,
                    position = node.GetPosition().position
                };
                graphData.nodes.Add(nodeData);
            }

            // Collect connection data
            foreach (var edge in edges.ToList())
            {
                var outputNode = (edge.output.node as EventNode);
                var inputNode = (edge.input.node as EventNode);

                var connectionData = new ConnectionData
                {
                    outputNodeName = outputNode.title,
                    inputNodeName = inputNode.title
                };
                graphData.connections.Add(connectionData);
            }

            // Save graph data to ScriptableObject
            GraphDataAsset graphDataAsset = ScriptableObject.CreateInstance<GraphDataAsset>();
            graphDataAsset.graphData = graphData;
            string path = "Assets/GraphData.asset";
            AssetDatabase.CreateAsset(graphDataAsset, path);
            AssetDatabase.SaveAssets();
        }


        public void LoadGraphData(GraphDataAsset graphDataAsset)
        {
            // Clear existing nodes and edges
            DeleteElements(nodes.ToList());
            DeleteElements(edges.ToList());

            // Load graph data from ScriptableObject
            graphData = graphDataAsset.graphData;

            // Create nodes
            foreach (var nodeData in graphData.nodes)
            {
                var node = new EventNode(nodeData.nodeName);
                node.SetPosition(new Rect(nodeData.position, new Vector2(150, 200)));
                AddElement(node);
            }

            // Create connections
            foreach (var connectionData in graphData.connections)
            {
                var outputNode = nodes.FirstOrDefault(n => (n as EventNode).title == connectionData.outputNodeName) as EventNode;
                var inputNode = nodes.FirstOrDefault(n => (n as EventNode).title == connectionData.inputNodeName) as EventNode;

                if (outputNode != null && inputNode != null)
                {
                    var outputPort = outputNode.output;
                    var inputPort = inputNode.input;

                    var edge = outputPort.ConnectTo(inputPort);
                    AddElement(edge);
                }
            }
        }
    }

}