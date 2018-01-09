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
    public bool autoUpdate;
    public TerrainType[] regions;

    private Mesh[] meshMaps;

    public Mesh[] getMesh()
    {
        return meshMaps;
    }

    public void GenerateMap()
    {   
        MapDisplay display = GetComponent<MapDisplay>();
        int amountOfMeshes = display.meshRenderer.Length;
        Color[][] colourMaps = new Color[amountOfMeshes][];
        meshMaps = new Mesh[amountOfMeshes];

        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        float[][,] noiseMapPerMesh = new float[amountOfMeshes][,];

        for(int i = 0; i < noiseMapPerMesh.Length; i++)
        {
            noiseMapPerMesh[i] = new float[mapWidth, mapHeight];
        }

        int mapWidthMultiplier = 0, mapHeightMultiplier = 0;

        for (int i = 0; i < amountOfMeshes; i++)
        {
            if (i % Mathf.Sqrt(amountOfMeshes) == 0 && i != 0)
            {
                mapWidthMultiplier = 0;
                mapHeightMultiplier++;
            }

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    noiseMapPerMesh[i][x, y] = noiseMap[x + mapWidth * mapWidthMultiplier, y + mapHeight * mapHeightMultiplier];
                }
            }
            mapWidthMultiplier++;
        }    

        for (int i = 0; i < amountOfMeshes; i++)
        {
            colourMaps[i] = new Color[mapWidth * mapHeight];
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    float currentHeight = noiseMapPerMesh[i][x, y];
                    for (int j = 0; j < regions.Length; j++)
                    {
                        if (currentHeight <= regions[j].height)
                        {
                            colourMaps[i][y * mapWidth + x] = regions[j].colour;
                            break;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < amountOfMeshes; i++)
        {
            Texture2D texture = TextureGenerator.TextureFromColourMap(colourMaps[i], mapWidth, mapHeight);
            meshMaps[i] = display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMapPerMesh[i], meshHeightMultiplier, meshHeightCurve), texture, i);
        }
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