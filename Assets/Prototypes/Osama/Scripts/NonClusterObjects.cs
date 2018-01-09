using UnityEngine;
using System.Collections.Generic;

public class NonClusterObjects : MonoBehaviour
{   
    private List<GameObject> allNonClusterObjects;
    private Objects objects;
    public GameObjectArray[] gameObjectArray;

    public List<GameObject> GetNonClusterObjects(MapGenerator generatedMap)
    {
        allNonClusterObjects = new List<GameObject>();
        objects = new Objects(generatedMap);

        for (int i = 0; i < gameObjectArray.Length; i++)
        {
            int loopsPerObject = 0, amountObjectsPlaced = 0;

            while (amountObjectsPlaced < gameObjectArray[i].amount && loopsPerObject < 500)
            {
                Vector3 positonObject = objects.RandomPositionObject();

                if (objects.CheckColoursEqual(positonObject, gameObjectArray[i].place, gameObjectArray[i].type) && objects.InsideMap(positonObject, gameObjectArray[i].type) &&
                objects.SpreadObjects(positonObject, gameObjectArray[i].radiusSameLayer, gameObjectArray[i].radiusOtherLayers, gameObjectArray[i].type.layer))
                {
                    GameObject gameObject = Instantiate(gameObjectArray[i].type, positonObject, Quaternion.identity);
                    //if (gameObjectArray[i].rotateOn) { objects.RotateObject(gameObject); }
                    allNonClusterObjects.Add(gameObject);
                    amountObjectsPlaced++;
                }
                loopsPerObject++;
            }
        }
        return allNonClusterObjects;
    }
    
    [System.Serializable]
    public struct GameObjectArray
    {
        public string name;
        public GameObject type;
        public int amount;
        public string place;       
        public bool rotateOn;

        public float radiusSameLayer;
        public float radiusOtherLayers;
    }
}