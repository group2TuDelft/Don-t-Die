using UnityEngine;
using System.Collections.Generic;

public class ClusterObjects : MonoBehaviour
{
    private List<GameObject> allClusterObjects = new List<GameObject>();
    private Objects objects;
    public GameObjectArray[] gameObjectArray;

    public List<GameObject> GetClusterObjects(MapGenerator generatedMap)
    {

        allClusterObjects = new List<GameObject>();
        objects = new Objects(generatedMap);

        for (int i = 0; i < gameObjectArray.Length; i++)
        {
            int loopsPerCluster = 0, amountClustersCreated = 0;

            while (amountClustersCreated < gameObjectArray[i].amountClusters && loopsPerCluster < 500)
            {
                Vector3 positonObject = objects.RandomPositionObject();

                if (objects.CheckColoursEqual(positonObject, gameObjectArray[i].place, gameObjectArray[i].type) && objects.InsideMap(positonObject, gameObjectArray[i].type) &&
                    objects.SpreadObjects(positonObject, gameObjectArray[i].clusterRadius, gameObjectArray[i].radiusOtherLayers, gameObjectArray[i].type.layer))
                {
                    GameObject centerOfCluster = Instantiate(gameObjectArray[i].type, positonObject, Quaternion.identity);
                    allClusterObjects.Add(centerOfCluster);
                    amountClustersCreated++;
                    ObjectsAroundCenter(centerOfCluster, i);
                }
                loopsPerCluster++;
            }
        }
        return allClusterObjects;
    }

    public void ObjectsAroundCenter(GameObject centerObject, int index)
    {
        int amountObjectPlaced = 0, amountOfLoops = 0;

        while (amountObjectPlaced < gameObjectArray[index].amountObjectsPerCluster - 1 && amountOfLoops < 100)
        {
            Vector2 randomNumber = Random.insideUnitCircle * gameObjectArray[index].aroundClusterRadius;
            Vector3 positionObjectAroundCenter = new Vector3(centerObject.transform.position.x + randomNumber.x, centerObject.transform.position.y, centerObject.transform.position.z + randomNumber.y);


            if (objects.CheckColoursEqual(positionObjectAroundCenter, gameObjectArray[index].place, gameObjectArray[index].type) &&
                objects.InsideMap(positionObjectAroundCenter, gameObjectArray[index].type) &&
                objects.SpreadObjects(positionObjectAroundCenter, gameObjectArray[index].radiusSameLayer, gameObjectArray[index].radiusOtherLayers, gameObjectArray[index].type.layer))
            {
                GameObject gameObject = Instantiate(gameObjectArray[index].type, positionObjectAroundCenter, Quaternion.identity);
                allClusterObjects.Add(gameObject);
                amountObjectPlaced++;
            }
            amountOfLoops++;
        }
    }

    [System.Serializable]
    public struct GameObjectArray
    {
        public string name;
        public GameObject type;
        public int amountClusters;
        public int amountObjectsPerCluster;
        public string place;

        public float aroundClusterRadius;
        public float clusterRadius;

        public float radiusSameLayer;
        public float radiusOtherLayers;
    }
}