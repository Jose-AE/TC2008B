using UnityEngine;

public class VectorTest : MonoBehaviour
{
    [Header("Point A")]
    [SerializeField][Range(-10, 10)] float ax;
    [SerializeField][Range(-10, 10)] float ay;
    [SerializeField][Range(-10, 10)] float az;

    [Header("Point B")]
    [SerializeField][Range(-10, 10)] float bx;
    [SerializeField][Range(-10, 10)] float by;
    [SerializeField][Range(-10, 10)] float bz;

    [Header("Point C")]
    [SerializeField][Range(-10, 10)] float cx;
    [SerializeField][Range(-10, 10)] float cy;
    [SerializeField][Range(-10, 10)] float cz;

    Vector3 a;
    Vector3 b;
    Vector3 c;
    Vector3 normal;


    Transform pointA;
    Transform pointB;
    Transform pointC;


    void Start()
    {
        pointA = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        pointB = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        pointC = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;

    }

    void Update()
    {
        a = new Vector3(ax, ay, az);
        b = new Vector3(bx, by, bz);
        c = new Vector3(cx, cy, cz);


        pointA.transform.position = a;
        pointB.transform.position = b;
        pointC.transform.position = c;


        Vector3 ab = b - a;
        Vector3 ac = c - a;

        normal = Vector3.Cross(ab, ac).normalized *2;

        // Draw triangle edges
        Debug.DrawLine(a, b, Color.red);
        Debug.DrawLine(a, c, Color.green);
        Debug.DrawLine(b, c, Color.yellow);

        Vector3 centroid = (a + b + c) / 3;
        Debug.DrawLine(centroid, centroid + normal, Color.blue);
    }
}
