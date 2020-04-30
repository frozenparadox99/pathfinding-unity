using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Graph))]
public class GraphView : MonoBehaviour
{
    public GameObject nodeViewPrefab;

    public NodeView[,] nodeViews;

    public Color baseColor = Color.white;
    public Color wallColor = Color.black;

    public void Init(Graph graph)
    {
        if(graph == null)
        {
            Debug.LogWarning("Graphview : No graph init");
            return;
        }

        nodeViews = new NodeView[graph.Width, graph.Height];

        foreach(Node n in graph.nodes)
        {
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
            NodeView nodeView = instance.GetComponent<NodeView>();

            if (nodeView != null)
            {
                nodeView.Init(n);

                nodeViews[n.xIndex, n.yIndex] = nodeView;

                if(n.nodeType == NodeType.Blocked)
                {
                    nodeView.ColorNode(wallColor);
                }
                else
                {
                    nodeView.ColorNode(baseColor);
                }
            }
        }
    }

    public void ColorNodes(List<Node> nodes,Color color)
    {
        foreach (Node n in nodes)
        {
            if (n != null)
            {
                NodeView nodeView = nodeViews[n.xIndex, n.yIndex];

                if (nodeView != null)
                {
                    nodeView.ColorNode(color);
                }
            }
        }
    }

    public void ShowNodeArrows(Node node,Color color)
    {
        if (node != null)
        {
            NodeView nodeView = nodeViews[node.xIndex, node.yIndex];
            if (nodeView != null)
            {
                nodeView.ShowArrow(color);
            }
        }
    }

    public void ShowNodeArrows(List<Node> nodes,Color color)
    {
        foreach(Node n in nodes)
        {
            ShowNodeArrows(n,color);
        }
    }

}
