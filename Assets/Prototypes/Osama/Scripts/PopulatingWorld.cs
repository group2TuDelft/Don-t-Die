using UnityEngine;
using System.Collections.Generic;

public class PopulatingWorld : MonoBehaviour
{

    private void Start()
    {
        MapGenerator generatedMap = GetComponent<MapGenerator>();
        generatedMap.seed = Random.Range(0, 10000000);
        generatedMap.GenerateMap();
      
        List<GameObject> clusterObjects = GetComponent<ClusterObjects>().GetClusterObjects(generatedMap);
        List<GameObject> nonClusterObjects = GetComponent<NonClusterObjects>().GetNonClusterObjects(generatedMap);
        List<GameObject> allObjects = new List<GameObject>();

        allObjects.AddRange(nonClusterObjects);
        allObjects.AddRange(clusterObjects);

        foreach (GameObject gameObject in allObjects)
        {
            CalculateHeightObject(gameObject);
            CalculateAngle(gameObject);

            if (gameObject.name == "tree_1(Clone)" || gameObject.name == "dessert_rock_1(Clone)" || gameObject.name == "bush(Clone)") { gameObject.transform.Rotate(-90f, 0f, 0f); }
        }
    }





    public void CalculateHeightObject(GameObject gameObject)
    {
        RaycastHit hit;
        Vector3 coordinates = gameObject.GetComponent<Collider>().bounds.center;
        //Vector3 coordinates = gameObject.transform.position;

        Ray downRay = new Ray(coordinates, -Vector3.up);
        Physics.Raycast(downRay, out hit);
        coordinates.y = coordinates.y - hit.distance;

        gameObject.transform.position = new Vector3(coordinates.x, coordinates.y, coordinates.z);
    }






    private void CalculateAngle(GameObject gameObject)
    {
        RaycastHit hit;
        Vector3 coordinates = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        Ray downRay = new Ray(coordinates, -Vector3.up);
        Physics.Raycast(downRay, out hit);

        Vector3 generalDirection = Vector3.ProjectOnPlane(hit.normal, Vector3.up).normalized;

        if (!(generalDirection == Vector3.zero))
        {
            Vector3 preciseDirection = Vector3.ProjectOnPlane(generalDirection, hit.normal).normalized;
            Quaternion rotation = Quaternion.LookRotation(preciseDirection, hit.normal);

            gameObject.transform.rotation = rotation;
        }
    }
}

