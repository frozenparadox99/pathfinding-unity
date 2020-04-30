using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathFinder : MonoBehaviour
{
    Node m_startNode;
    Node m_goalNode;

    Graph m_graph;
    GraphView m_graphView;

    // Next nodes on deck to visit on the graph
    Queue<Node> m_frontierNodes;
    List<Node> m_exploredNodes;
    //Fastest Path nodes
    List<Node> m_pathNodes;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;

    public bool isComplete = false;
    int m_iterations = 0;

    public void Init(Graph graph,GraphView graphView,Node start,Node goal)
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

        m_frontierNodes = new Queue<Node>();
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

    }

    void ShowColors()
    {
        ShowColors(m_graphView, m_startNode, m_goalNode);
    }

    private void ShowColors(GraphView graphView, Node start, Node goal)
    {
        if(graphView == null || start==null || goal == null)
        {
            return;
        }

        if(m_frontierNodes != null)
        {
            graphView.ColorNodes(m_frontierNodes.ToList(), frontierColor);
        }

        if (m_exploredNodes != null)
        {
            graphView.ColorNodes(m_exploredNodes, exploredColor);
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
}
