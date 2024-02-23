using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// the data struct of graph.
    /// ||数据结构-图。
    /// </summary>
    public class Graph
    {
        public bool directed;
        public List<GraphNode> nodes = new List<GraphNode>();
        public List<GraphEdge> edges = new List<GraphEdge>();

        public Dictionary<string, GraphNode> nodeMap = new Dictionary<string, GraphNode>();
        public Dictionary<string, GraphEdge> edgeMap = new Dictionary<string, GraphEdge>();

        public Graph(bool directed)
        {
            this.directed = directed;
        }

        public void Clear()
        {
            nodes.Clear();
            edges.Clear();
            nodeMap.Clear();
            edgeMap.Clear();
        }

        public void Refresh()
        {
            foreach (var node in nodes)
            {
                node.depth = GetNodeDepth(node);
            }
        }

        public static double GetNodesTotalValue(List<GraphNode> nodes)
        {
            double totalValue = 0;
            foreach (var node in nodes)
            {
                totalValue += node.totalValues;
            }
            return totalValue;
        }

        public List<List<GraphNode>> GetDepthNodes()
        {
            List<List<GraphNode>> depthNodes = new List<List<GraphNode>>();
            var maxDepth = GetMaxDepth();
            for (int i = 0; i <= maxDepth; i++)
            {
                depthNodes.Add(new List<GraphNode>());
            }
            foreach (var node in nodes)
            {
                if (node.inDegree == 0)
                {
                    depthNodes[0].Add(node);
                }
                else
                {
                    int deep = GetNodeDepth(node);
                    depthNodes[maxDepth - deep].Add(node);
                }
            }
            return depthNodes;
        }

        public List<GraphNode> GetRootNodes()
        {
            List<GraphNode> rootNodes = new List<GraphNode>();
            foreach (var node in nodes)
            {
                if (node.inDegree == 0)
                {
                    rootNodes.Add(node);
                }
            }
            return rootNodes;
        }

        public int GetMaxDepth()
        {
            int maxDepth = 0;
            foreach (var node in nodes)
            {
                int deep = GetNodeDepth(node);
                if (deep > maxDepth)
                {
                    maxDepth = deep;
                }
            }
            return maxDepth;
        }

        // public int GetNodeDepth(GraphNode node)
        // {
        //     int depth = 0;
        //     GetNodeDepth(node, ref depth);
        //     return depth;
        // }

        // public void GetNodeDepth(GraphNode node, ref int depth, int recursiveCount = 0)
        // {
        //     if (recursiveCount > 50)
        //     {
        //         XLog.Error("Graph.GetNodeDeep(): recursiveCount > 50, maybe graph is ring");
        //         return;
        //     }
        //     if (node.inDegree == 0)
        //     {
        //         return;
        //     }
        //     else
        //     {
        //         depth += 1;
        //         foreach (var edge in node.inEdges)
        //         {
        //             GetNodeDepth(edge.node1, ref depth, recursiveCount + 1);
        //         }
        //     }
        // }

        public int GetNodeDepth(GraphNode node, int recursiveCount = 0)
        {
            if (recursiveCount > 50)
            {
                XLog.Error("Graph.GetNodeDeep(): recursiveCount > 50, maybe graph is ring");
                return 0;
            }
            int depth = 0;
            if (node.outDegree == 0)
            {
                return depth;
            }
            else
            {
                foreach (var edge in node.outEdges)
                {
                    int otherDeep = GetNodeDepth(edge.node2, recursiveCount + 1);
                    if (otherDeep > depth)
                    {
                        depth = otherDeep;
                    }
                }
                return depth + 1;
            }
        }



        public GraphNode GetNode(string nodeId)
        {
            if (nodeMap.ContainsKey(nodeId))
            {
                return nodeMap[nodeId];
            }
            else
            {
                return null;
            }
        }

        public GraphEdge GetEdge(string nodeId1, string nodeId2)
        {
            if (directed)
            {
                return edgeMap[nodeId1 + "_" + nodeId2];
            }
            else
            {
                var key = nodeId1 + "_" + nodeId2;
                if (edgeMap.ContainsKey(key))
                {
                    return edgeMap[key];
                }
                else
                {
                    key = nodeId2 + "_" + nodeId1;
                    if (edgeMap.ContainsKey(key))
                    {
                        return edgeMap[key];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public GraphNode AddNode(string nodeId, string nodeName, int dataIndex)
        {
            if (nodeMap.ContainsKey(nodeId))
            {
                return nodeMap[nodeId];
            }
            else
            {
                GraphNode node = new GraphNode(nodeId, nodeName, dataIndex);
                node.hostGraph = this;
                nodeMap.Add(nodeId, node);
                nodes.Add(node);
                return node;
            }
        }

        public GraphEdge AddEdge(string nodeId1, string nodeId2, double value)
        {
            GraphNode node1, node2;
            if (!nodeMap.TryGetValue(nodeId1, out node1))
            {
                XLog.Warning("Graph.AddEdge(): " + nodeId1 + " not exist");
                return null;
            }
            if (!nodeMap.TryGetValue(nodeId2, out node2))
            {
                XLog.Warning("Graph.AddEdge(): " + nodeId2 + " not exist");
                return null;
            }
            if (node1 == null)
            {
                XLog.Warning("Graph.AddEdge(): node1 is null");
                return null;
            }
            if (node2 == null)
            {
                XLog.Warning("Graph.AddEdge(): node2 is null");
                return null;
            }
            if (node1 == node2)
            {
                XLog.Warning("Graph.AddEdge(): node1 == node2");
                return null;
            }
            string edgeKey = nodeId1 + "_" + nodeId2;
            if (edgeMap.ContainsKey(edgeKey))
            {
                return edgeMap[edgeKey];
            }
            else
            {
                GraphEdge edge = new GraphEdge(node1, node2, value);
                edge.key = edgeKey;
                edge.hostGraph = this;

                if (directed)
                {
                    node1.outEdges.Add(edge);
                    node2.inEdges.Add(edge);
                }
                node1.edges.Add(edge);
                if (node1 != node2)
                {
                    node2.edges.Add(edge);
                }

                edgeMap.Add(edgeKey, edge);
                edges.Add(edge);
                return edge;
            }
        }

        public void EachNode(System.Action<GraphNode> onEach)
        {
            if (onEach == null) return;
            foreach (var node in nodes)
            {
                onEach(node);
            }
        }

        public void BreadthFirstTraverse(GraphNode startNode, System.Action<GraphNode> onTraverse)
        {
            if (startNode == null) return;
            foreach (var node in nodes)
            {
                node.visited = false;
            }

            onTraverse(startNode);
            startNode.visited = true;

            Queue<GraphNode> queue = new Queue<GraphNode>();
            queue.Enqueue(startNode);
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                foreach (var edge in currentNode.edges)
                {
                    var otherNode = edge.node1 == currentNode ? edge.node2 : edge.node1;
                    if (!otherNode.visited)
                    {
                        onTraverse(otherNode);
                        otherNode.visited = true;
                        queue.Enqueue(otherNode);
                    }
                }
            }
        }

        public void DeepFirstTraverse(GraphNode startNode, System.Action<GraphNode> onTraverse)
        {
            if (startNode == null) return;
            foreach (var node in nodes)
            {
                node.visited = false;
            }

            Stack<GraphNode> stack = new Stack<GraphNode>();
            stack.Push(startNode);
            while (stack.Count > 0)
            {
                var currentNode = stack.Pop();
                if (!currentNode.visited)
                {
                    onTraverse(currentNode);
                    currentNode.visited = true;
                }
                foreach (var edge in currentNode.edges)
                {
                    var otherNode = edge.node1 == currentNode ? edge.node2 : edge.node1;
                    if (!otherNode.visited)
                    {
                        stack.Push(otherNode);
                    }
                }
            }
        }
    }

    /// <summary>
    /// The node of graph.
    /// ||图的节点。
    /// </summary>
    public class GraphNode
    {
        public string id;
        public string name;
        public List<GraphEdge> edges = new List<GraphEdge>();
        public List<GraphEdge> inEdges = new List<GraphEdge>();
        public List<GraphEdge> outEdges = new List<GraphEdge>();
        public Graph hostGraph;
        public int dataIndex;
        public bool visited;
        public int depth = -1;

        public GraphNode(string id, string name, int dataIndex)
        {
            this.id = id;
            this.name = name;
            this.dataIndex = dataIndex;
        }

        public int degree { get { return edges.Count; } }

        public int inDegree { get { return inEdges.Count; } }

        public int outDegree { get { return outEdges.Count; } }

        public double totalValues
        {
            get
            {
                double totalValue = 0;
                if (inEdges.Count == 0)
                {
                    foreach (var edge in outEdges)
                    {
                        totalValue += edge.value;
                    }
                }
                else
                {
                    foreach (var edge in inEdges)
                    {
                        totalValue += edge.value;
                    }
                }
                return totalValue;
            }
        }
        public override string ToString()
        {
            return name;
        }
    }

    /// <summary>
    /// The edge of graph.
    /// ||图的边。
    /// </summary>
    public class GraphEdge
    {
        public string key;
        public GraphNode node1;
        public GraphNode node2;
        public double value;
        public Graph hostGraph;

        public List<Vector3> points = new List<Vector3>();
        public float width;
        public bool highlight;

        public GraphEdge(GraphNode node1, GraphNode node2, double value)
        {
            this.node1 = node1;
            this.node2 = node2;
            this.value = value;
        }
    }
}