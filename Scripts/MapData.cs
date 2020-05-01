using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class MapData : MonoBehaviour
{
    public int width = 10;
    public int height = 5;


    // Start is called before the first frame update
    //void Start()
    //{
    //    int[,] mapInstance = MakeMap();
    //}

    public TextAsset textAsset;

    public Texture2D textureMap;

    public string resourcePath = "Mapdata";

    public Color32 openColor = Color.white;
    public Color32 blockedColor = Color.black;
    public Color32 lightTerrainColor = new Color32(124, 194, 78, 255);
    public Color32 mediumTerrainColor = new Color32(252, 255, 52, 255);
    public Color32 heavyTerrainColor = new Color32(255, 129, 12, 255);

    static Dictionary<Color32, NodeType> terrainLookupTable = new Dictionary<Color32, NodeType>();

     void Awake()
    {
        SetupLookupTable();
        if(textureMap == null)
        {
            string levelName = SceneManager.GetActiveScene().name;
            textureMap = Resources.Load(resourcePath + "/" + levelName) as Texture2D;
        }

        if (textAsset == null)
        {
            string levelName = SceneManager.GetActiveScene().name;
            textAsset = Resources.Load(resourcePath + "/" + levelName) as TextAsset;
        }
    }

    public List<string> GetMapFromTextFile(TextAsset tAsset)
    {
        List<string> lines = new List<string>();

        if (tAsset != null)
        {
            string textData = tAsset.text;
            string[] delimiters = { "\r\n", "\n" };
            lines.AddRange( textData.Split(delimiters, System.StringSplitOptions.None));
            lines.Reverse();
        }
        
        return lines;
    }

    public List<string> GetMapFromTextFile()
    {
        
        return GetMapFromTextFile(textAsset);
    }

    public List<string> GetMapFromTexture(Texture2D texture)
    {
        List<string> lines = new List<string>();

        if (texture != null)
        {

            for (int y = 0; y < texture.height; y++)
            {
                string newLine = "";

                for (int x = 0; x < texture.width; x++)
                {
                    Color pixelColor = texture.GetPixel(x, y);

                    if (terrainLookupTable.ContainsKey(pixelColor))
                    {
                        NodeType nodeType = terrainLookupTable[pixelColor];
                        int nodeTypeNum = (int)nodeType;
                        newLine += nodeTypeNum;
                    }
                    else
                    {
                        newLine += "0";
                    }

                    
                }
                lines.Add(newLine);
            }
        }
        return lines;
    }

    public void SetDimensions(List<string> textLines)
    {
        height = textLines.Count;
        foreach(string line in textLines)
        {
            if(line.Length > width)
            {
                width = line.Length;
            }
        }
    }

   public int[,] MakeMap()
    {
        List<string> lines = new List<string>();

        if(textureMap != null)
        {
            lines = GetMapFromTexture(textureMap);
        }
        else
        {
            lines = GetMapFromTextFile();
        }

        
        SetDimensions(lines);

        int[,] map = new int[width, height];

        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
               if (lines[y].Length > x) { 
                }
              map[x, y] = (int)Char.GetNumericValue(lines[y][x]);
            }
        }

        
       


        return map;
    }
   
    void SetupLookupTable()
    {
        terrainLookupTable.Add(openColor, NodeType.Open);
        terrainLookupTable.Add(blockedColor, NodeType.Blocked);
        terrainLookupTable.Add(lightTerrainColor, NodeType.LightTerrain);
        terrainLookupTable.Add(mediumTerrainColor, NodeType.MediumTerrain);
        terrainLookupTable.Add(heavyTerrainColor, NodeType.HeavyTerrain);

    }

}
