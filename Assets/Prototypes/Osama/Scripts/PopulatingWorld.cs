using UnityEngine;
using System.Collections.Generic;

public class PopulatingWorld : MonoBehaviour
{

    private void Start()
    {
        Debug.Log("fsfs");
        MapGenerator generatedMap = GetComponent<MapGenerator>();
        generatedMap.seed = Random.Range(0, 10000000);

        generatedMap.GenerateMap();

        GameObject[] gameObjectMeshes = GameObject.FindGameObjectsWithTag("Mesh");

        List<GameObject> clusterObjects = GetComponent<ClusterObjects>().GetClusterObjects(generatedMap);
        List<GameObject> nonClusterObjects = GetComponent<NonClusterObjects>().GetNonClusterObjects(generatedMap);
        List<GameObject> allObjects = new List<GameObject>();

        allObjects.AddRange(nonClusterObjects);
        allObjects.AddRange(clusterObjects);

        foreach (GameObject gameObject in allObjects)
        {
            CalculateExactHeightObject(gameObject);
            if (gameObject.name != "Chest(Clone)")
            {
                gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }

    public void CalculateExactHeightObject(GameObject gameObject)
    {
        RaycastHit hit;
        Vector3 coordinates = gameObject.transform.position;

        Ray downRay = new Ray(coordinates, Vector3.down);
        Physics.Raycast(downRay, out hit, (1<<12));
        coordinates.y = coordinates.y - hit.distance;

        RaycastHit hit2;
        Ray downRay2 = new Ray(coordinates, Vector3.down);
        Physics.Raycast(downRay2, out hit2);
        coordinates.y = coordinates.y - hit2.distance;

        gameObject.transform.position = new Vector3(coordinates.x, coordinates.y, coordinates.z);

    }

    private void CalculateAngle(GameObject gameObject)
    {
        RaycastHit hit;
        Vector3 coordinates = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        Ray downRay = new Ray(coordinates, Vector3.down);
        Physics.Raycast(downRay, out hit);

        Vector3 generalDirection = Vector3.ProjectOnPlane(hit.normal, Vector3.up).normalized;
        if (!(generalDirection.y == 0))
        {
            Vector3 preciseDirection = Vector3.ProjectOnPlane(generalDirection, hit.normal).normalized;
            Quaternion rotation = Quaternion.LookRotation(preciseDirection, hit.normal);

            gameObject.transform.rotation = rotation;
        }
    }
}