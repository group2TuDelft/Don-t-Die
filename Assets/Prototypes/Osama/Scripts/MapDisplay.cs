using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    private Mesh createdMesh;
    public MeshCollider meshCollider;

    public Mesh DrawMesh(MeshData meshData, Texture2D texture)
    {
        Mesh createdMesh = meshData.CreateMesh();

        meshCollider.sharedMesh = createdMesh;
        meshFilter.sharedMesh = createdMesh;
        meshRenderer.sharedMaterial.mainTexture = texture;
        return createdMesh;
    }
}