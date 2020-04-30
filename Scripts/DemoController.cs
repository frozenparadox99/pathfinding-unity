using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoController : MonoBehaviour
{
    public MapData mapData;
    public Graph graph;

    public PathFinder pathFinder;
    public int startX = 1;
    public int startY = 0;
    public int goalX = 14;
    public int goalY = 1;
    private void Start()
    {

        if(mapData!=null && graph != null)
        {
            int[,] mapInstance = mapData.MakeMap();
            graph.Init(mapInstance);

            GraphView graphView = graph.gameObject.GetComponent<GraphView>();
            if (graphView != null)
            {
                graphView.Init(graph);
            }

            if(graph.IsWithinBounds(startX,startY) && graph.IsWithinBounds(goalX, goalY) && pathFinder!=null)
            {
                Node startNode = graph.nodes[startX, startY];
                Node goalNode = graph.nodes[goalX, goalY];
                pathFinder.Init(graph, graphView, startNode, goalNode);
            }
        }
    }
}
