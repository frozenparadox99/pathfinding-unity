using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinderPriorityQueue : MonoBehaviour
{
    Node m_startNode;
    Node m_goalNode;

    Graph m_graph;
    GraphView m_graphView;

    // Next nodes on deck to visit on the graph
    PriorityQueue<Node> m_frontierNodes;
    List<Node> m_exploredNodes;
    //Fastest Path nodes
    List<Node> m_pathNodes;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;
    public Color arrowColor = new Color(0.85f, 0.85f, 0.85f, 1f);
    public Color highlightColor = new Color(1f, 1f, 0.5f, 1f);

    public bool showIterations = true;
    public bool showColors = true;
    public bool showArrows = true;
    public bool exitOnGoal = true;

    public bool isComplete = false;
    int m_iterations = 0;

    public enum Mode
    {
        BreadthFirstSearch = 0,
        Dijkstra = 1
    }

    public Mode mode = Mode.BreadthFirstSearch;

    public void Init(Graph graph, GraphView graphView, Node start, Node goal)
    {
        if (start == null || goal == null || graph == null || graphView == null)
        {
            Debug.LogWarning("PATHFINDER ERROR: missing values");
            return;
        }

        if (start.nodeType == NodeType.Blocked || goal.nodeType == NodeType.Blocked)
        {
            Debug.Log(start.nodeType);
            Debug.Log(goal.nodeType);
            Debug.Log(start.xIndex);
            Debug.Log(start.yIndex);
            Debug.Log(goal.xIndex);
            Debug.Log(goal.yIndex);
            Debug.LogWarning("PATHFINDER ERROR: start and end nodes must not be walls");
            return;
        }

        m_graph = graph;
        m_graphView = graphView;
        m_startNode = start;
        m_goalNode = goal;
        ShowColors(graphView, start, goal);

        m_frontierNodes = new PriorityQueue<Node>();
        m_frontierNodes.Enqueue(start);

        m_exploredNodes = new List<Node>();
        m_pathNodes = new List<Node>();

        for (int x = 0; x < m_graph.Width; x++)
        {
            for (int y = 0; y < m_graph.Height; y++)
            {
                m_graph.nodes[x, y].Reset();
            }
        }

        isComplete = false;
        m_iterations = 0;
        m_startNode.distanceTraveled = 0;
    }

    void ShowColors()
    {
        ShowColors(m_graphView, m_startNode, m_goalNode);
    }

    private void ShowColors(GraphView graphView, Node start, Node goal)
    {
        if (graphView == null || start == null || goal == null)
        {
            return;
        }

        if (m_frontierNodes != null)
        {
            graphView.ColorNodes(m_frontierNodes.ToList(), frontierColor);
        }

        if (m_exploredNodes != null)
        {
            graphView.ColorNodes(m_exploredNodes, exploredColor);
        }

        if (m_pathNodes != null && m_pathNodes.Count > 0)
        {
            graphView.ColorNodes(m_pathNodes, pathColor);
        }

        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];
        if (startNodeView != null)
        {
            startNodeView.ColorNode(startColor);
        }

        NodeView goalNodeView = graphView.nodeViews[goal.xIndex, goal.yIndex];
        if (goalNodeView != null)
        {
            goalNodeView.ColorNode(goalColor);
        }
    }

    public IEnumerator SearchRoutine(float timeStep = 0.1f)
    {
        float timeStart = Time.time;

        yield return null;

        while (!isComplete)
        {
            if (m_frontierNodes.Count > 0)
            {
                Node currentNode = m_frontierNodes.Dequeue();
                m_iterations++;

                if (!m_exploredNodes.Contains(currentNode))
                {
                    m_exploredNodes.Add(currentNode);
                }

                if (mode == Mode.BreadthFirstSearch)
                {
                    ExpandFrontier(currentNode);
                }
                else if (mode == Mode.Dijkstra)
                {
                    ExpandFrontierDijkstra(currentNode);
                }



                if (m_frontierNodes.Contains(m_goalNode))
                {
                    m_pathNodes = GetPathNodes(m_goalNode);
                    if (exitOnGoal)
                    {
                        isComplete = true;
                    }
                }

                if (showIterations)
                {
                    ShowDiagnostics();

                    yield return new WaitForSeconds(timeStep);
                }
            }
            else
            {
                isComplete = true;
            }
        }

        ShowDiagnostics();
        Debug.Log("Pathfinder searchroutine : elapsed time = " + (Time.time - timeStart).ToString() + " seconds");
    }

    private void ShowDiagnostics()
    {
        if (showColors)
        {
            ShowColors();
        }


        if (m_graphView != null && showArrows)
        {
            m_graphView.ShowNodeArrows(m_frontierNodes.ToList(), arrowColor);

            if (m_frontierNodes.Contains(m_goalNode))
            {
                m_graphView.ShowNodeArrows(m_pathNodes, highlightColor);
            }
        }
    }

    void ExpandFrontier(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbors.Count; i++)
            {
                if (!m_exploredNodes.Contains(node.neighbors[i]) && !m_frontierNodes.Contains(node.neighbors[i]))
                {
                    node.neighbors[i].previous = node;
                    m_frontierNodes.Enqueue(node.neighbors[i]);
                }
            }
        }
    }

    void ExpandFrontierDijkstra(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbors.Count; i++)
            {
                if (!m_exploredNodes.Contains(node.neighbors[i]))
                {
                    float distanceToNeighbor = m_graph.GetNodeDistance(node, node.neighbors[i]);
                    float newDistanceTraveled = distanceToNeighbor + node.distanceTraveled;

                    if (float.IsPositiveInfinity(node.neighbors[i].distanceTraveled) || newDistanceTraveled < node.neighbors[i].distanceTraveled)
                    {
                        node.neighbors[i].previous = node;
                        node.neighbors[i].distanceTraveled = newDistanceTraveled;
                    }
                    if (!m_frontierNodes.Contains(node.neighbors[i]))
                    {
                        node.neighbors[i].priority = (int)node.neighbors[i].distanceTraveled;
                        m_frontierNodes.Enqueue(node.neighbors[i]);
                    }

                }
            }
        }
    }

    List<Node> GetPathNodes(Node endNode)
    {
        List<Node> path = new List<Node>();

        if (endNode == null)
        {
            return path;
        }
        path.Add(endNode);

        Node currentNode = endNode.previous;

        while (currentNode != null)
        {
            path.Insert(0, currentNode);
            currentNode = currentNode.previous;
        }

        return path;
    }
}
