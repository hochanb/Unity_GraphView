using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventGraph
{


    [System.Serializable]
    public class GraphData
    {
        public List<NodeData> nodes = new List<NodeData>();
        public List<ConnectionData> connections = new List<ConnectionData>();
    }

    [System.Serializable]
    public class NodeData
    {
        public string nodeName;
        public Vector2 position;
    }

    [System.Serializable]
    public class ConnectionData
    {
        public string outputNodeName;
        public string inputNodeName;
    }

}