using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public AnimationCurve meshHeightCurve;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    private float[,] noiseMap;
    public bool autoUpdate;
    public TerrainType[] regions;

    private Color[] colourMap;
    private Texture2D texture;
    private Mesh meshMap;


    public float[,] getNoiseMap()
    {
        return noiseMap;
    }

    public Texture2D getTexture()
    {
        return texture;
    }

    public Mesh getMesh()
    {
        return meshMap;
    }

    public Color[] getColour()
    {
        return colourMap;
    }

    public void GenerateMap()
    {

        noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        colourMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapWidth + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }
        MapDisplay display = GetComponent<MapDisplay>();
        texture = TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight);     
        meshMap = display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), texture);
    }

    void OnValidate()
    {
        if (mapWidth < 1){mapWidth = 1;}
        if (mapHeight < 1){mapHeight = 1;}
        if (lacunarity < 1){lacunarity = 1;}
        if (octaves < 0){octaves = 0;}
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}