using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// the data struct of graph.
    /// ||数据结构-图。
    /// </summary>
    public class GraphData
    {
        public bool directed;
        public List<GraphNode> nodes = new List<GraphNode>();
        public List<GraphEdge> edges = new List<GraphEdge>();

        public Dictionary<string, GraphNode> nodeMap = new Dictionary<string, GraphNode>();
        public Dictionary<string, GraphEdge> edgeMap = new Dictionary<string, GraphEdge>();

        public GraphData(bool directed)
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
                if (node.IsAnyInEdgesExpanded())
                {
                    totalValue += node.totalValues;
                }
            }
            return totalValue;
        }

        public static int GetExpandedNodesCount(List<GraphNode> nodes)
        {
            int count = 0;
            foreach (var node in nodes)
            {
                if (node.IsAnyInEdgesExpanded())
                {
                    count++;
                }
            }
            return count;
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
        //         XLog.Error("GraphData.GetNodeDeep(): recursiveCount > 50, maybe graph is ring");
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
                XLog.Error("GraphData.GetNodeDeep(): recursiveCount > 50, maybe graph is ring");
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

        public GraphNode AddNode(string nodeId, string nodeName, int dataIndex, double value)
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
                XLog.Warning("GraphData.AddEdge(): " + nodeId1 + " not exist");
                return null;
            }
            if (!nodeMap.TryGetValue(nodeId2, out node2))
            {
                XLog.Warning("GraphData.AddEdge(): " + nodeId2 + " not exist");
                return null;
            }
            if (node1 == null)
            {
                XLog.Warning("GraphData.AddEdge(): node1 is null");
                return null;
            }
            if (node2 == null)
            {
                XLog.Warning("GraphData.AddEdge(): node2 is null");
                return null;
            }
            if (directed && node1 == node2)
            {
                XLog.Warning("GraphData.AddEdge(): node1 == node2:" + node1);
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

        public void ExpandNode(string nodeId, bool flag)
        {
            var node = GetNode(nodeId);
            if (node != null)
            {
                node.Expand(flag);
            }
        }

        public void ExpandAllNodes(bool flag, int level = -1)
        {
            foreach (var node in nodes)
            {
                if (level < 0 || node.level == level)
                {
                    node.Expand(flag);
                }
            }
        }

        public bool IsAllNodeInZeroPosition()
        {
            foreach (var node in nodes)
            {
                if (node.position != Vector3.zero) return false;
            }
            return true;
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
        public double value;
        public List<GraphEdge> edges = new List<GraphEdge>();
        public List<GraphEdge> inEdges = new List<GraphEdge>();
        public List<GraphEdge> outEdges = new List<GraphEdge>();
        public GraphData hostGraph;
        public int dataIndex;
        public bool visited;
        public int depth = -1;
        public bool expand = true;
        public int level = 0;
        public Vector3 position = Vector3.zero;
        public Vector3 pp = Vector3.zero;
        public float weight;
        public float repulsion;

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

        public bool IsAllInEdgesCollapsed()
        {
            if (inEdges.Count == 0) return false;
            foreach (var edge in inEdges)
            {
                if (!edge.expand) return false;
            }
            return true;
        }

        public bool IsAnyInEdgesExpanded()
        {
            if (inEdges.Count == 0) return true;
            foreach (var edge in inEdges)
            {
                if (edge.expand) return true;
            }
            return false;
        }

        public void Expand(bool flag)
        {
            if (expand == flag) return;
            expand = flag;
            foreach (var edge in outEdges)
            {
                edge.expand = flag;
            }
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
        public GraphData hostGraph;

        public List<Vector3> upPoints = new List<Vector3>();
        public List<Vector3> downPoints = new List<Vector3>();
        public float width;
        public float distance;
        public bool highlight;
        public bool expand = true;

        public GraphEdge(GraphNode node1, GraphNode node2, double value)
        {
            this.node1 = node1;
            this.node2 = node2;
            this.value = value;
        }

        public bool IsPointInEdge(Vector2 point)
        {
            if (upPoints.Count == 0 || downPoints.Count == 0) return false;
            var lastCount = upPoints.Count - 1;
            if (point.x < upPoints[0].x || point.x > upPoints[lastCount].x) return false;
            if (point.y > upPoints[0].y && point.y > upPoints[lastCount].y) return false;
            if (point.y < downPoints[0].y && point.y < downPoints[lastCount].y) return false;

            for (int i = 0; i < upPoints.Count - 1; i++)
            {
                var diff = point.x - upPoints[i].x;
                if (diff <= 0)
                {
                    return point.y < upPoints[i].y && point.y > downPoints[i].y;
                }
            }
            return false;
        }
    }
}