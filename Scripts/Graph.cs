using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Node[,] nodes;
    public List<Node> walls = new List<Node>();

    int[,] m_mapData;
    int m_width;
    int m_height;

    public void Init(int[,] mapData) 
    {
        m_mapData = mapData;
        m_width = mapData.GetLength(0);
        m_height = mapData.GetLength(1);

        nodes = new Node[m_width,m_height];

        for(int y = 0; y < m_height; y++)
        {
            for(int x = 0; x < m_width; x++)
            {
                NodeType type = (NodeType)mapData[x,y];
                Node newNode = new Node(x, y, type);
                nodes[x, y] = newNode;

                newNode.position = new Vector3(x, 0, y);

                if(type == NodeType.Blocked)
                {
                    walls.Add(newNode);
                }
            }
        }
    }

}
