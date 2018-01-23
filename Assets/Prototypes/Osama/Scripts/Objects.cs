using UnityEngine;
using System.Collections.Generic;

public class Objects
{
    private MapGenerator generatedMap;
    private GameObject[] gameObjectMeshes;
    private int layerMeshes;
    static private float BEGIN_HEIGHT = 20;
    public Objects(MapGenerator generatedMap)
    {
        this.generatedMap = generatedMap;
        gameObjectMeshes = GameObject.FindGameObjectsWithTag("Mesh");
        layerMeshes = gameObjectMeshes[0].layer;
    }

    public bool CheckColoursEqual(Vector3 positionObject, string whereToPlace, GameObject gameObject)
    {
        int checkApproxEqual = 0;
        bool checkColoursEqual = false;
        Color colourOfTerrain = Color.black;

        if(whereToPlace == "Any")
        {
            return true;
        }

        for (int i = 0; i < generatedMap.regions.Length; i++)
        {
            if (whereToPlace == generatedMap.regions[i].name)
            {
                colourOfTerrain = generatedMap.regions[i].colour;
                break;
            }
        }

        Vector3 colBoundsObject = gameObject.GetComponent<Collider>().bounds.extents;

        Vector3[] cornerPositions = new Vector3[4];

        cornerPositions[0] = new Vector3(positionObject.x + colBoundsObject.x, positionObject.y+1f, positionObject.z + colBoundsObject.z);
        cornerPositions[1] = new Vector3(positionObject.x + colBoundsObject.x, positionObject.y+1f, positionObject.z - colBoundsObject.z);
        cornerPositions[2] = new Vector3(positionObject.x - colBoundsObject.x, positionObject.y+1f, positionObject.z + colBoundsObject.z);
        cornerPositions[3] = new Vector3(positionObject.x - colBoundsObject.x, positionObject.y+1f, positionObject.z - colBoundsObject.z);

        Ray[] downRayFromCorners = new Ray[4];

        for (int i = 0; i < cornerPositions.Length; i++)
        {
            downRayFromCorners[i] = new Ray(cornerPositions[i], -Vector3.up);
        }

        for (int i = 0; i < downRayFromCorners.Length; i++)
        {
            RaycastHit hit;
            Physics.Raycast(downRayFromCorners[i], out hit);
            try
            {
                Texture2D textureMap = (Texture2D)hit.transform.GetComponent<Renderer>().material.mainTexture;
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= textureMap.width;
                pixelUV.y *= textureMap.height;

                for (int j = 0; j < 3; j++)
                {
                    if (Mathf.Abs(textureMap.GetPixel((int)pixelUV.x, (int)pixelUV.y)[j] - colourOfTerrain[j]) < 0.01f)
                    {
                        checkApproxEqual++;
                    }
                }

                if (checkApproxEqual == 12) { checkColoursEqual = true; }
            }
            catch (System.Exception) { checkColoursEqual = false; }
        }
        return checkColoursEqual;
    }

    public Vector3 RandomPositionObject()
    {
        Mesh[] meshMaps = generatedMap.getMesh();
        List<Vector3> verticesAllMeshMaps = new List<Vector3>();

        foreach (Mesh meshMap in meshMaps) { verticesAllMeshMaps.AddRange(meshMap.vertices); }

        int index = Random.Range(0, verticesAllMeshMaps.Count);

        float xCoord = verticesAllMeshMaps[index].x;
        float zCoord = verticesAllMeshMaps[index].z;
        float yCoord = 0.5f;

        Vector3 localCoords = new Vector3(xCoord, yCoord, zCoord);
        Vector3 worldCoords = new Vector3();

        for (int i = 0; i < meshMaps.Length; i++)
        {
            if (index < meshMaps[i].vertices.Length * i + meshMaps[i].vertices.Length)
            {
                worldCoords = gameObjectMeshes[i].transform.TransformPoint(localCoords);
                break;
            }
        }

        return worldCoords;
    }

    public bool SpreadObjects(Vector3 positionObject, float radiusSameLayer, float radiusOtherLayers, int layer)
    {
        bool noObjectHit = true;

        Collider[] collidersSameLayer;
        Collider[] collidersOtherLayers;

        collidersSameLayer = Physics.OverlapSphere(new Vector3(positionObject.x, positionObject.y, positionObject.z), radiusSameLayer, 1 << layer);

        collidersOtherLayers = Physics.OverlapSphere(new Vector3(positionObject.x, positionObject.y, positionObject.z), radiusOtherLayers, ~(1 << layerMeshes) & ~(1 << layer) & ~(1 << 8) & ~(1<<5));
        if (collidersSameLayer.Length >= 1 || collidersOtherLayers.Length >= 1) { noObjectHit = false; }

        return noObjectHit;
    }

    public bool InsideMap(Vector3 positionObject, GameObject gameObject)
    {
        bool objectInsideMap;

        Vector3 colBoundsObject = gameObject.GetComponent<Collider>().bounds.extents;
        Vector3[] cornerPositions = new Vector3[4];

        cornerPositions[0] = new Vector3(positionObject.x + colBoundsObject.x, positionObject.y+1f, positionObject.z + colBoundsObject.z);
        cornerPositions[1] = new Vector3(positionObject.x + colBoundsObject.x, positionObject.y+1f, positionObject.z - colBoundsObject.z);
        cornerPositions[2] = new Vector3(positionObject.x - colBoundsObject.x, positionObject.y+1f, positionObject.z + colBoundsObject.z);
        cornerPositions[3] = new Vector3(positionObject.x - colBoundsObject.x, positionObject.y+1f, positionObject.z - colBoundsObject.z);

        Ray[] downRayFromCorners = new Ray[4];

        for (int i = 0; i < cornerPositions.Length; i++)
        {
            downRayFromCorners[i] = new Ray(cornerPositions[i], -Vector3.up);
        }

        objectInsideMap = Physics.Raycast(downRayFromCorners[0]) && Physics.Raycast(downRayFromCorners[1]) &&
            Physics.Raycast(downRayFromCorners[2]) && Physics.Raycast(downRayFromCorners[3]) ? true : false;

        return objectInsideMap;
    }

    public void RotateObject(GameObject gameObject)
    {
        RaycastHit hit;
        Vector3 coordinates = gameObject.transform.position;
        Ray downRay = new Ray(new Vector3(coordinates.x, coordinates.y +1f, coordinates.z), Vector3.down);
        Physics.Raycast(downRay, out hit);

        bool whichRotate = Random.value < 0.5 ? true : false;

        if (hit.collider.name == "Mesh1")
        {
            if (whichRotate)
            {
                gameObject.transform.Rotate(0f, 180f, 0f);
            }
            else
            {
                gameObject.transform.Rotate(0f, 90f, 0f);
            }
        }
        else if (hit.collider.name == "Mesh2")
        {
            if (whichRotate)
            {
                gameObject.transform.Rotate(0f, 90f, 0f);
            }
            else
            {
                gameObject.transform.Rotate(0f, 180f, 0f);
            }

        }
        else if (hit.collider.name == "Mesh3")
            if (whichRotate)
            {
                gameObject.transform.Rotate(0f, 270f, 0f);
            }
            else
            {
                gameObject.transform.Rotate(0f, 180f, 0f);
            }
        else if (hit.collider.name == "Mesh4")
        {
            if (whichRotate)
            {
                gameObject.transform.Rotate(0f, 180f, 0f);
            }
            else
            {
                gameObject.transform.Rotate(0f, 90f, 0f);
            }
        }
        else if (hit.collider.name == "Mesh5")
        {
            if (whichRotate)
            {
                gameObject.transform.Rotate(0f, 90f, 0f);
            }
            else
            {
                gameObject.transform.Rotate(0f, 180f, 0f);
            }

        }
        else if (hit.collider.name == "Mesh6")
            if (whichRotate)
            {
                gameObject.transform.Rotate(0f, 270f, 0f);
            }
            else
            {
                gameObject.transform.Rotate(0f, 180f, 0f);
            }
        else if (hit.collider.name == "Mesh7")
        {
            if (whichRotate)
            {
                gameObject.transform.Rotate(0f, 180f, 0f);
            }
            else
            {
                gameObject.transform.Rotate(0f, 90f, 0f);
            }
        }
        else if (hit.collider.name == "Mesh8")
            if (whichRotate)
            {
                gameObject.transform.Rotate(0f, 0f, 0f);
            }
            else
            {
                gameObject.transform.Rotate(0f, 270f, 0f);
            }
        else if (hit.collider.name == "Mesh9")
        {
            if (whichRotate)
            {
                gameObject.transform.Rotate(0f, 0f, 0f);
            }
            else
            {
                gameObject.transform.Rotate(0f, 270f, 0f);
            }
        }
    }

    public float CalculateHeightObject(Vector3 positionObject)
    {
        RaycastHit hit;
        Vector3 coordinates = positionObject;

        Ray downRay = new Ray(coordinates, Vector3.down);
        Physics.Raycast(downRay, out hit, 1<<12);
        coordinates.y = coordinates.y - hit.distance;

        RaycastHit hit2;
        Ray downRay2 = new Ray(coordinates, Vector3.down);
        Physics.Raycast(downRay2, out hit2, 1<<12);


        if (hit2.distance >= 1000)
        {
            return coordinates.y;
        }
        else
        {
            coordinates.y = coordinates.y - hit2.distance;
        }
        return coordinates.y;
    }


    [System.Serializable]
    public struct GameObjectArray
    {
        public string name;
        public GameObject type;
        public int amount;
        public string place;
        public float objectRadius;
        public bool rotateOn;
    }
}