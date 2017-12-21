using System.Collections.Generic;
using UnityEngine;

public class Clusters : MonoBehaviour
{

    private Texture2D textureGeneratedMap;
    private MapGenerator generatedMap;
    public GameObjectArray[] gameObjectArray;
    private int amountClustersCreated;

    private List<GameObject> allClusterObjects = new List<GameObject>();

    void Start()
    {
        generatedMap = GetComponent<MapGenerator>();
        generatedMap.seed = Random.Range(0, int.MaxValue);
        generatedMap.GenerateMap();
        textureGeneratedMap = generatedMap.getTexture();

        for (int i = 0; i < gameObjectArray.Length; i++)
        {
            int loopsPerCluster = 0;
            Vector3 boundsOfGameObject = gameObjectArray[i].type.GetComponent<Renderer>().bounds.extents;

            while (amountClustersCreated < gameObjectArray[i].clustersPerObject)
            {
                float randomX = Random.Range(0 + 2 * boundsOfGameObject.x + 1, (generatedMap.mapWidth) - 2 * boundsOfGameObject.x - 1);
                float randomZ = Random.Range(0 + 2 * boundsOfGameObject.z + 1, (generatedMap.mapHeight) - 2 * boundsOfGameObject.z - 1);

                if (CheckColoursEqual(randomX, randomZ, i))
                {
                    Vector3 positionObject = CalcExactPosition(randomX, randomZ, boundsOfGameObject);
                    if (Spread(positionObject, i))
                    {
                        GameObject centerOfCluster = Instantiate(gameObjectArray[i].type, positionObject, Quaternion.identity);

                        allClusterObjects.Add(centerOfCluster);
                        amountClustersCreated++;
                        Debug.Log("1: " + allClusterObjects.Count);
                        ObjectsInCluster(centerOfCluster, i);
                        Debug.Log("2: " + allClusterObjects.Count);
                    }
                }
                loopsPerCluster++;
                if (loopsPerCluster > 100000) { break; }
            }
        }

        for (int i = 0; i < allClusterObjects.Count; i++)
        {
            Debug.Log(allClusterObjects[i].transform.position.y);
        }

        for (int i = 0; i < allClusterObjects.Count; i++)
        {
            float xCoordObject = allClusterObjects[i].transform.position.x;
            float zCoordObject = allClusterObjects[i].transform.position.z;
            float heightObject = allClusterObjects[i].GetComponent<Renderer>().bounds.extents.y;
            Debug.Log(heightObject);
            allClusterObjects[i].transform.position += new Vector3(0f, CalculateHeightObject(xCoordObject, zCoordObject, heightObject), 0f);
        }
    }


    public void ObjectsInCluster(GameObject centerObject, int index)
    {
        int amountObjectPlaced = 0;
        int amountOfLoops = 0;
        Vector3 boundsGameObject = centerObject.GetComponent<Renderer>().bounds.extents;
        Vector2 check = ReverseCalc(centerObject.transform.position.x, centerObject.transform.position.z, boundsGameObject);

        while (amountObjectPlaced < gameObjectArray[index].objectPerCluster && amountOfLoops < 10000)
        {
            Vector2 ran = Random.insideUnitCircle * gameObjectArray[index].spreadAroundClusterRadius;
            if (CheckColoursEqual(check.x + ran.x, check.y + ran.y, 0))
            {
                Vector3 positionObject = CalcExactPosition(check.x + ran.x, check.y + ran.y, boundsGameObject);
                if (Spread(positionObject, index) && positionObject.x >= 2 * boundsGameObject.x && positionObject.z >= 2 * boundsGameObject.z
                    && positionObject.x < generatedMap.mapWidth - 2 * boundsGameObject.x && positionObject.z < generatedMap.mapHeight - 2 * boundsGameObject.z)
                {
                    GameObject gameObject = Instantiate(centerObject, positionObject , Quaternion.identity);
                    allClusterObjects.Add(gameObject);
                    amountObjectPlaced++;
                }
            }
            amountOfLoops++;
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

    public Vector2 ReverseCalc(float randomX, float randomZ, Vector3 boundsOfGameObject)
    {
        randomZ = -(-generatedMap.mapHeight + boundsOfGameObject.z + 1 + randomZ);
        randomX -= boundsOfGameObject.x;

        return new Vector2(randomX, randomZ);
    }

    bool Spread(Vector3 pos, int index)
    {
        bool noHit = true;
        Collider[] colliders;

        colliders = Physics.OverlapSphere(new Vector3(pos.x, pos.y, pos.z), gameObjectArray[index].spreadObjectRadius, 1 << gameObjectArray[index].type.layer);

        if (colliders.Length >= 1) { noHit = false; }
        return noHit;
    }


    [System.Serializable]
    public struct GameObjectArray
    {
        public string name;
        public GameObject type;
        public int clustersPerObject;
        public int objectPerCluster;
        public string place;

        public float spreadAroundClusterRadius;
        public float spreadObjectRadius;
        public float spreadClusterRadius;
    }

}