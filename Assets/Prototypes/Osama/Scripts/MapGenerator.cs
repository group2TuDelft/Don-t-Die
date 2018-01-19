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

    private Mesh[] meshMaps;

    public Mesh[] getMesh()
    {
        return meshMaps;
    }

    public void GenerateMap()
    {
        MapDisplay display = GetComponent<MapDisplay>();
        int amountOfMeshes = display.meshRenderer.Length;

        noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        float[][,] noiseMapPerMesh = new float[amountOfMeshes][,];

        for(int i = 0; i < amountOfMeshes; i++)
        {
            noiseMapPerMesh[i] = new float[mapWidth, mapHeight];
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                /*noiseMapPerMesh[0][x, y] = noiseMap[x, y];
                noiseMapPerMesh[1][x, y] = noiseMap[x + mapWidth, y];
                noiseMapPerMesh[2][x, y] = noiseMap[x, y + mapHeight];
                noiseMapPerMesh[3][x, y] = noiseMap[x + mapWidth, y + mapHeight];*/

                noiseMapPerMesh[0][x, y] = noiseMap[x, y];
                noiseMapPerMesh[1][x, y] = noiseMap[x + mapWidth, y];
                noiseMapPerMesh[2][x, y] = noiseMap[x + 2 * mapWidth, y];
                noiseMapPerMesh[3][x, y] = noiseMap[x, y + mapHeight];
                noiseMapPerMesh[4][x, y] = noiseMap[x + mapWidth, y + mapHeight];
                noiseMapPerMesh[5][x, y] = noiseMap[x + 2 * mapWidth, y + mapHeight];
                noiseMapPerMesh[6][x, y] = noiseMap[x, y + 2 * mapHeight];
                noiseMapPerMesh[7][x, y] = noiseMap[x + mapWidth, y + 2 * mapHeight];
                noiseMapPerMesh[8][x, y] = noiseMap[x + 2 * mapWidth, y + 2 * mapHeight];



            }
        }

        Color[][] colourMaps = new Color[amountOfMeshes][];
        meshMaps = new Mesh[amountOfMeshes];
        for(int i = 0; i < amountOfMeshes; i++)
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
            Mesh meshMap = display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMapPerMesh[i], meshHeightMultiplier, meshHeightCurve),
            TextureGenerator.TextureFromColourMap(colourMaps[i], mapWidth, mapHeight), i);
            meshMaps[i] = meshMap;
        }
    }

    void OnValidate()
    {
        if (mapWidth < 1) { mapWidth = 1; }
        if (mapHeight < 1) { mapHeight = 1; }
        if (lacunarity < 1) { lacunarity = 1; }
        if (octaves < 0) { octaves = 0; }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}