using UnityEngine;

public class Objects : MonoBehaviour {
    public int numberOfObjects; 
    private int currentObjects;
    public GameObject[] objectToPlaceArray; 

    void Start()
    {
        MapGenerator generatedMap =  GetComponent<MapGenerator>();
        generatedMap.seed = Random.Range(0, 10000);

        generatedMap.GenerateMap();

        /*Mesh meshMap = generatedMap.GenerateMap();
        float[,] noiseMap = generatedMap.generateNoiseMap();

        Debug.Log("Generated Map: " + generatedMap == null);
        Debug.Log("Mesh map: " + meshMap == null);
        Debug.Log("Noise map: " + noiseMap == null);

        Bounds bounds = meshMap.bounds;

        int objectPlaced = 0;
        int amountOfLoops = 0;
        Debug.Log(noiseMap.GetType());


        for (int j = 0; j < objectToPlaceArray.Length; j++)
        {
            float heightTerrain = generatedMap.regions[j].height;

            for (int i = 0; i < numberOfObjects; i++)
            {
                while (objectPlaced < numberOfObjects || amountOfLoops > 10000)
                {
                    Debug.Log("In while loop");
                    Vector2 randomNumberHeight = new Vector2(Random.Range(0, 1), Random.Range(0, 1));

                    if (noiseMap[(int)randomNumberHeight.x, (int)randomNumberHeight.y] <= heightTerrain)
                    {
                        Debug.Log("Height terrain at coordinates: " + heightTerrain);
                        Debug.Log("check: " + noiseMap[(int)randomNumberHeight.x, (int)randomNumberHeight.y]);
                        Vector3 randomNumber = new Vector3(randomNumberHeight.x, 0.5f, randomNumberHeight.y);
                        Instantiate(objectToPlaceArray[j], randomNumber, Quaternion.identity);
                        objectPlaced++;
                    }
                }
            }
        }
        */
    }
}
