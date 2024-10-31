using UnityEditor.SceneManagement;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField][Range(0, 360)] float rotationX;
    [SerializeField][Range(0, 360)] float rotationY;
    [SerializeField][Range(0, 360)] float rotationZ;

    [SerializeField][Range(0.1f, 10)] float scale;

    [SerializeField][Range(-10, 10)] float posX;
    [SerializeField][Range(-10, 10)] float posY;
    [SerializeField][Range(-10, 10)] float posZ;



    Vector3[] vertices;
    int[] triangles;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    Mesh mesh;

    void Awake()
    {
        mesh = new Mesh();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material.color = Color.grey;
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;



        SetVerticesAndTriangles();

        ApplyTransforms();
    }

    void SetVerticesAndTriangles()
    {
        vertices = new Vector3[]
        {
            new Vector3(1,1,-1),
            new Vector3(1,-1,-1),
            new Vector3(1,1,1),
            new Vector3(1,-1,1),
            new Vector3(-1,1,-1),
            new Vector3(-1,-1,-1),
            new Vector3(-1,1,1),
            new Vector3(-1,-1,1),
        };

        triangles = new int[]
        {
            5, 3, 1,
            3, 8, 4,
            7, 6, 8,
            2, 8, 6,
            1, 4, 2,
            5, 2, 6,
            5, 7, 3,
            3, 7, 8,
            7, 5, 6,
            2, 4, 8,
            1, 3, 4,
            5, 1, 2,
        };

        for (int i = 0; i < triangles.Length; i++) triangles[i]--;


    }


    void ApplyTransforms()
    {
        Matrix4x4 transaltionMatrix = VectorOperations.TranslationMatrix(new Vector3(posX, posY, posZ));


        for (int i = 0; i < vertices.Length; i++)
        {
            Vector4 vert = new Vector4(vertices[i].x, vertices[i].y, vertices[i].z, 1);
            vertices[i] = transaltionMatrix * vert;

        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

    }




}
