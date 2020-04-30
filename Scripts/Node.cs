using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum NodeType
{
    Open = 0,
    Blocked =1
}

public class Node
{
    //Variable to see if the node is blocked or open for traversing or moving into
    public NodeType nodeType = NodeType.Open;

    //Variables to keep track of position in the grid
    public int xIndex = -1;
    public int yIndex = -1;

    public Vector3 position;

    //List to track neighbors
    public List<Node> neighbors = new List<Node>();

    public float distanceTraveled = Mathf.Infinity;

    public Node previous = null;

    public Node(int xIndex,int yIndex,NodeType nodeType)
    {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        this.nodeType = nodeType;
    }

    public void Reset()
    {
        previous = null;
    }
}
