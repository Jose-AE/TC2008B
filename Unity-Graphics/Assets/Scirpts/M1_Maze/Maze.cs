using System;
using UnityEngine;

public class Maze : MonoBehaviour
{

    [SerializeField] bool showPath = true;

    Mesh cubeMesh;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    private int currentPathSegment = 0;
    private bool isRotating = false;
    private bool isTranslating = false;
    private Vector3 currentTranslation;
    private float currentRotation = 0f;
    private float moveSpeed = 0.1f; // Units per frame
    private float rotateSpeed = 1f; // Degrees per frame


    private struct PathSegment
    {
        public Vector3 targetPos;
        public float rotationBeforeNext;

        public PathSegment(Vector3 target, float rotation)
        {
            targetPos = target;
            rotationBeforeNext = rotation;
        }
    }

    PathSegment[] path = new PathSegment[]
    {
        new PathSegment(new Vector3(-2, 0, 0), 90),
        new PathSegment(new Vector3(0, 0, 2), -90),
        new PathSegment(new Vector3(-2, 0, 0), -90),
        new PathSegment(new Vector3(0, 0, -2), 90),
        new PathSegment(new Vector3(-1, 0, 0), 90),
        new PathSegment(new Vector3(0, 0, 2), -90),
        new PathSegment(new Vector3(-1, 0, 0), -90),
        new PathSegment(new Vector3(0, 0, -2), 90),
        new PathSegment(new Vector3(-2, 0, 0), 90),
        new PathSegment(new Vector3(0, 0, 3), 90),
        new PathSegment(new Vector3(2, 0, 0), -90),
        new PathSegment(new Vector3(0, 0, 1), 90),
        new PathSegment(new Vector3(1, 0, 0), -90),
        new PathSegment(new Vector3(0, 0, 1), -90),
        new PathSegment(new Vector3(-4, 0, 0), -90),
        new PathSegment(new Vector3(0, 0, -1), 90),
        new PathSegment(new Vector3(-1, 0, 0), 90),
        new PathSegment(new Vector3(0, 0, 2), 90),
        new PathSegment(new Vector3(1, 0, 0), -90),
        new PathSegment(new Vector3(0, 0, 1), -90),
        new PathSegment(new Vector3(-1, 0, 0), 90),
        new PathSegment(new Vector3(0, 0, 1), 90),
        new PathSegment(new Vector3(3, 0, 0), -90),
        new PathSegment(new Vector3(0, 0, 2), 90),
        new PathSegment(new Vector3(1, 0, 0), 90),
        new PathSegment(new Vector3(0, 0, -3), -90),
        new PathSegment(new Vector3(2, 0, 0), -90),
        new PathSegment(new Vector3(0, 0, 1), -90),
        new PathSegment(new Vector3(-1, 0, 0), 90),
        new PathSegment(new Vector3(0, 0, 3), -90),
        new PathSegment(new Vector3(-1, 0, 0), 90),
        new PathSegment(new Vector3(0, 0, 5), 90),
        new PathSegment(new Vector3(1, 0, 0), -90),
        new PathSegment(new Vector3(0, 0, 1), -90),
        new PathSegment(new Vector3(-4, 0, 0), -90),
        new PathSegment(new Vector3(0, 0, -1), 90),
        new PathSegment(new Vector3(-1, 0, 0), 90),
        new PathSegment(new Vector3(0, 0, 2), 90),
        new PathSegment(new Vector3(1, 0,0), -90),
        new PathSegment(new Vector3(0, 0,1), 90),
        new PathSegment(new Vector3(1, 0,0), 90),
        new PathSegment(new Vector3(0, 0,-1), -90),
        new PathSegment(new Vector3(1, 0,0), -90),
        new PathSegment(new Vector3(0, 0,1), 90),
        new PathSegment(new Vector3(1, 0,0), 90),
        new PathSegment(new Vector3(0, 0,-1), -90),
        new PathSegment(new Vector3(1, 0,0), -90),
        new PathSegment(new Vector3(0, 0,1), 90),
        new PathSegment(new Vector3(1, 0,0), 90),
        new PathSegment(new Vector3(0, 0,-1), -90),
        new PathSegment(new Vector3(1, 0,0), -90),
        new PathSegment(new Vector3(0, 0,1),90),
        new PathSegment(new Vector3(2, 0,0), -90),
    };

    void CreateCubeMesh(Vector3 offset, float sideLength = 1)
    {
        float length = sideLength / 2;

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-length, -length, length) + offset,
            new Vector3(length, -length, length)+ offset,
            new Vector3(length, length, length)+ offset,
            new Vector3(-length, length, length)+ offset,
            new Vector3(length, -length, -length)+ offset,
            new Vector3(length, length, -length)+ offset,
            new Vector3(-length, -length, -length)+ offset,
            new Vector3(-length, length, -length)+ offset
        };

        int[] triangles = new int[]
        {
            1, 2, 3,
            1, 3, 4,
            2, 5, 6,
            2, 6, 3,
            5, 7, 8,
            5, 8, 6,
            7, 1, 4,
            7, 4, 8,
            4, 3, 6,
            4, 6, 8,
            7, 5, 2,
            7, 2, 1,
        };

        for (int i = 0; i < triangles.Length; i++) triangles[i]--;

        cubeMesh.vertices = vertices;
        cubeMesh.triangles = triangles;


        meshRenderer.material.color = Color.blue;

    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject cube = new GameObject("TheCar");
        cube.transform.position = Vector3.zero;
        cube.transform.localScale = new Vector3(10, 10, 10);
        meshFilter = cube.AddComponent<MeshFilter>();
        meshRenderer = cube.AddComponent<MeshRenderer>();
        cubeMesh = new Mesh();
        meshFilter.mesh = cubeMesh;

        CreateCubeMesh(new Vector3(0.0f, 0.0f, 0.0f));
        isTranslating = true;
        isRotating = false;
        currentTranslation = Vector3.zero;
        currentRotation = 0;
        currentPathSegment = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPathSegment >= path.Length) { meshRenderer.material.color = Color.green; return; };

        //set initial pos
        CreateCubeMesh(new Vector3(0.0f, 0.0f, 0.0f));
        Matrix4x4 transform = VectorOperations.GetTranslationMatrix(new Vector3(0.5f, 0.5f, 0.5f));
        transform *= VectorOperations.GetTranslationMatrix(new Vector3(0, 0, -10));

        //insta move to the previous segment
        for (int i = 0; i < currentPathSegment + (isTranslating ? 0 : 1); i++)
        {
            transform *= VectorOperations.GetTranslationMatrix(path[i].targetPos);
        }


        if (isTranslating)
        {
            meshRenderer.material.color = Color.blue;


            currentTranslation += path[currentPathSegment].targetPos.normalized * moveSpeed;

            transform *= VectorOperations.GetTranslationMatrix(currentTranslation);


            if (currentTranslation == path[currentPathSegment].targetPos)
            {
                isTranslating = false;
                isRotating = true;
                currentTranslation = Vector3.zero;
            }

        }
        else if (isRotating)
        {
            meshRenderer.material.color = Color.magenta;

            currentRotation += rotateSpeed * (path[currentPathSegment].rotationBeforeNext < 0 ? -1 : 1);

            if (currentRotation == path[currentPathSegment].rotationBeforeNext)
            {
                isTranslating = true;
                isRotating = false;
                currentRotation = 0;
                currentPathSegment++;
            }
            else
            {

                transform *= VectorOperations.GetYRotationMatrix(currentRotation);
            }

        }

        VectorOperations.ApplyTransformMatrixToMesh(transform, cubeMesh);


        if (currentPathSegment == path.Length - 1) meshRenderer.material.color = Color.green;
    }


    private void OnDrawGizmos()
    {

        if (path == null || path.Length == 0 || !showPath)
            return;

        Vector3 offset = new Vector3(5f, 0, 5f);
        float cellSize = 10f;
        Vector3 mazeStart = new Vector3(0, 0, -10f) * cellSize + offset;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(mazeStart, 1f);

        Vector3 currentPos = mazeStart;
        for (int i = 0; i < path.Length; i++)
        {
            Vector3 nextPos = currentPos + (path[i].targetPos * cellSize);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(currentPos, nextPos);

            Gizmos.color = path[i].rotationBeforeNext == 90 ? Color.blue : path[i].rotationBeforeNext == -90 ? Color.magenta : Color.black;
            Gizmos.DrawSphere(nextPos, 3f);

            currentPos = nextPos;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, 2f);
    }

}