using UnityEngine;
using System.Collections.Generic;

public class NonClusters : MonoBehaviour
{

    private Texture2D textureGeneratedMap;
    private MapGenerator generatedMap;

    public GameObjectArray[] gameObjectArray;
    private int amountObjectsPlaced;



    void Start()
    {
        generatedMap = GetComponent<MapGenerator>();
        generatedMap.seed = Random.Range(0, int.MaxValue);
        generatedMap.GenerateMap();
        textureGeneratedMap = generatedMap.getTexture();

        List<GameObject> allNonClusterObjects = new List<GameObject>();

        for (int i = 0; i < gameObjectArray.Length; i++)
        {
            int loopsPerObject = 0;
            Vector3 boundsOfGameObject = gameObjectArray[i].type.GetComponent<Renderer>().bounds.extents ;

            while (amountObjectsPlaced < gameObjectArray[i].amount && loopsPerObject < 100000)
            {
                float randomX = Random.Range(0 + 2 * boundsOfGameObject.x + 1, (generatedMap.mapWidth) - 2 * boundsOfGameObject.x - 1);
                float randomZ = Random.Range(0 + 2 * boundsOfGameObject.z + 1, (generatedMap.mapHeight) - 2 * boundsOfGameObject.z - 1);

                if (CheckColoursEqual(randomX, randomZ, i))
                {
                    Vector3 positionObject = CalcExactPosition(randomX, randomZ, boundsOfGameObject);
                    if (Spread(positionObject, i))
                    {
                        GameObject gameObject = Instantiate(gameObjectArray[i].type, positionObject, Quaternion.identity);
                        allNonClusterObjects.Add(gameObject);
                        amountObjectsPlaced++;
                    }
                }
                loopsPerObject++;
            }
        }


        for (int i = 0; i < allNonClusterObjects.Count; i++)
        {
            float xCoordObject = allNonClusterObjects[i].transform.position.x;
            float zCoordObject = allNonClusterObjects[i].transform.position.z;
            float heightObject = allNonClusterObjects[i].GetComponent<Renderer>().bounds.extents.y;
            allNonClusterObjects[i].transform.position += new Vector3(0f, CalculateHeightObject(xCoordObject, zCoordObject, heightObject), 0f);
        }


    }

    public float CalculateHeightObject(float randomX, float randomZ, float heightObject)
    {

        RaycastHit hit;
        Vector3 coordinates = new Vector3(randomX, 100f, randomZ);

        Ray downRay = new Ray(coordinates + new Vector3(0f, -heightObject, 0f), -Vector3.up);
        Physics.Raycast(downRay, out hit);
        coordinates.y = coordinates.y - hit.distance;
        return coordinates.y;

    }

    public bool CheckColoursEqual(float xCoord, float yCoord, int index)
    {
        int checkApproxEqual = 0;
        bool checkColoursEqual = false;
        string whereToPlace = gameObjectArray[index].place;
        Color colourOfTerrain = Color.black;

        for (int i = 0; i < generatedMap.regions.Length; i++)
        {
            if (whereToPlace == generatedMap.regions[i].name)
            {
                colourOfTerrain = generatedMap.regions[i].colour;
                break;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (Mathf.Abs(textureGeneratedMap.GetPixel((int)xCoord, (int)yCoord)[i] - colourOfTerrain[i]) < 0.01f)
            {
                checkApproxEqual++;
            }
        }

        if (checkApproxEqual == 4) { checkColoursEqual = true; }

        return checkColoursEqual;
    }

    public Vector3 CalcExactPosition(float randomX, float randomZ, Vector3 boundsOfGameObject)
    {
        randomZ = (generatedMap.mapHeight - boundsOfGameObject.z - 1 - randomZ);
        randomX += boundsOfGameObject.x;
        Vector3 positionObject = new Vector3(randomX, 0f, randomZ);
        return positionObject;
    }


    bool Spread(Vector3 pos, int index)
    {
        bool noHit = true;
        Collider[] colliders;

        colliders = Physics.OverlapSphere(new Vector3(pos.x, pos.y, pos.z), gameObjectArray[index].spreadRadius, 1 << gameObjectArray[index].type.layer);

        if (colliders.Length >= 1) { noHit = false; }
        return noHit;
    }

    [System.Serializable]
    public struct GameObjectArray
    {
        public string name;
        public GameObject type;
        public int amount;
        public string place;
        [Range(0, 255)]
        public float spreadRadius;
    }
}