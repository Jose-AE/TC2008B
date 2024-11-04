using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{




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
    private float speedMult = 2f;



    private struct PathSegment
    {
        public Vector3 targetPos;
        public float rotationBeforeNext;

        public PathSegment(Vector3 target, float rotation)
        {
            this.targetPos = target;
            this.rotationBeforeNext = rotation;
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





    };






    void InitCube(Vector3 offset, float sideLength = 1)
    {
        float length = sideLength / 2;

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(length, length, -length) + offset,
            new Vector3(length, -length, -length) + offset,
            new Vector3(length, length, length) + offset,
            new Vector3(length, -length, length) + offset,
            new Vector3(-length, length, -length) + offset,
            new Vector3(-length, -length, -length) + offset,
            new Vector3(-length, length, length) + offset,
            new Vector3(-length, -length, length) + offset,
        };

        int[] triangles = new int[]
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



        InitCube(new Vector3(0.0f, 0.0f, 0.0f));
        isTranslating = true;
        isRotating = false;
        currentTranslation = Vector3.zero;
        currentRotation = 0;
        currentPathSegment = 0;

        rotateSpeed *= speedMult;
        moveSpeed *= speedMult;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentPathSegment >= path.Length) { meshRenderer.material.color = Color.green; return; };



        //set initial pos
        InitCube(new Vector3(0.0f, 0.0f, 0.0f));
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


            // Calculate translation progress
            currentTranslation += path[currentPathSegment].targetPos.normalized * moveSpeed;

            transform *= VectorOperations.GetTranslationMatrix(currentTranslation);


            if (Vector3.Distance(currentTranslation, path[currentPathSegment].targetPos) < 0.001f)
            {
                isTranslating = false;
                isRotating = true;
                currentTranslation = Vector3.zero;
            }

        }
        else if (isRotating)
        {
            meshRenderer.material.color = Color.magenta;


            // Calculate translation progress
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

        ApplyFunctionToMeshVertices(cubeMesh, vertex => VectorOperations.ApplyMatrixToVector(transform, vertex));


        if (currentPathSegment == path.Length - 1) meshRenderer.material.color = Color.green;
    }


    void ApplyFunctionToMeshVertices(Mesh mesh, Func<Vector3, Vector3> function)
    {
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
            vertices[i] = function(vertices[i]);


        mesh.vertices = vertices;
    }



    private void OnDrawGizmos()
    {
        if (path == null || path.Length == 0)
            return;

        Vector3 offset = new Vector3(5f, 0, 5f);
        float cellSize = 10f;
        Vector3 mazeStart = new Vector3(0, 0, -10f) * cellSize + offset;

        // Draw start position
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(mazeStart, 1f);

        // Draw path segments
        Vector3 currentPos = mazeStart;
        for (int i = 0; i < path.Length; i++)
        {
            Vector3 nextPos = currentPos + (path[i].targetPos * cellSize);

            // Draw line for the path segment
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(currentPos, nextPos);

            // Draw sphere at segment point
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(nextPos, 1f);

            // Update current position for next segment
            currentPos = nextPos;
        }

        // Draw reference point at origin
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, 2f);
    }

}
