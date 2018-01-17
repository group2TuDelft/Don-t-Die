using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public MeshFilter[] meshFilter;
    public MeshRenderer[] meshRenderer;
    public MeshCollider[] meshCollider;

    public Mesh DrawMesh(MeshData meshData, Texture2D texture, int i)
    {
        Mesh createdMesh = meshData.CreateMesh();

        meshCollider[i].sharedMesh = createdMesh;
        meshFilter[i].sharedMesh = createdMesh;
        meshRenderer[i].sharedMaterial.mainTexture = texture;
        return createdMesh;
    }
}