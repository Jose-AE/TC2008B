using UnityEngine;

public class MazeTest : MonoBehaviour
{
    private GameObject theCar;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Material material;

    // Animation states
    private int currentSegment = 0;
    private float rotationProgress = 0f;
    private bool isRotating = false;
    private Vector3 currentPosition;
    private float currentRotation = 0f;

    // Movement speed
    private const float MOVE_SPEED = 0.0001f;
    private const float ROTATION_SPEED = 1f;

    // Define path segments (you'll need to adjust these based on your specific maze)
    private struct PathSegment
    {
        public Vector3 start;
        public Vector3 end;
        public float rotationBeforeNext;

        public PathSegment(Vector3 start, Vector3 end, float rotation)
        {
            this.start = start;
            this.end = end;
            this.rotationBeforeNext = rotation;
        }
    }

    // Replace these with your actual maze path coordinates
    private PathSegment[] path;

    void Start()
    {
        // Create the cube
        theCar = new GameObject("TheCar");
        meshFilter = theCar.AddComponent<MeshFilter>();
        meshRenderer = theCar.AddComponent<MeshRenderer>();

        // Create material
        material = new Material(Shader.Find("Standard"));
        meshRenderer.material = material;
        SetColor(Color.blue);

        // Create cube mesh
        CreateCubeMesh();

        // Initialize path (replace with your maze's actual path)
        path = new PathSegment[]
        {
            new PathSegment(new Vector3(0, 0, 0), new Vector3(0, 0, 50), 90),
            new PathSegment(new Vector3(0, 0, 50), new Vector3(50, 0, 50), 90),
            // Add more segments based on your maze solution
        };

        currentPosition = path[0].start;
    }

    void CreateCubeMesh()
    {
        Mesh mesh = new Mesh();


        float length = 0.5f;
        Vector3 offset = Vector3.one / 2;

        // Cube vertices
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

        // Cube triangles
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


        mesh.vertices = vertices;
        mesh.triangles = triangles;


        meshFilter.mesh = mesh;
    }

    void Update()
    {
        if (currentSegment >= path.Length) return;

        PathSegment currentPath = path[currentSegment];

        if (isRotating)
        {
            // Handle rotation
            rotationProgress += ROTATION_SPEED;
            SetColor(Color.magenta);

            if (rotationProgress >= currentPath.rotationBeforeNext)
            {
                isRotating = false;
                rotationProgress = 0f;
                currentRotation += currentPath.rotationBeforeNext;
                currentSegment++;
                SetColor(Color.blue);
            }
        }
        else
        {
            // Handle movement
            Vector3 direction = (currentPath.end - currentPath.start).normalized;
            currentPosition += direction * MOVE_SPEED;

            // Check if we reached the end of current segment
            if (Vector3.Distance(currentPosition, currentPath.end) < 0.1f)
            {
                currentPosition = currentPath.end;
                if (currentPath.rotationBeforeNext != 0)
                {
                    isRotating = true;
                }
                else
                {
                    currentSegment++;
                }
            }

            // Check if we're in the final segment
            if (currentSegment == path.Length - 1)
            {
                SetColor(Color.green);
            }
        }

        // Apply transformations
        Matrix4x4 translation = VectorOperations.GetTranslationMatrix(new Vector3(currentPosition.x, currentPosition.y, currentPosition.z));
        Matrix4x4 rotation = VectorOperations.GetYRotationMatrix(currentRotation + rotationProgress);
        Matrix4x4 scale = VectorOperations.GetScaleMatrix(Vector3.one * 10);

        // Combine transformations
        Matrix4x4 transform = translation * rotation * scale;

        // Apply transformation to vertices
        Vector3[] originalVertices = meshFilter.mesh.vertices;
        Vector3[] transformedVertices = new Vector3[originalVertices.Length];

        for (int i = 0; i < originalVertices.Length; i++)
        {
            // Vector4 homogeneous = VectorOperations.ToHomogeneous(originalVertices[i]);
            // transformedVertices[i] = VectorOperations.FromHomogeneous(transform * homogeneous);
        }

        // Update mesh
        Mesh transformedMesh = new Mesh();
        transformedMesh.vertices = transformedVertices;
        transformedMesh.triangles = meshFilter.mesh.triangles;
        transformedMesh.RecalculateNormals();

        meshFilter.mesh = transformedMesh;
    }

    private void SetColor(Color color)
    {
        material.color = color;
    }
}