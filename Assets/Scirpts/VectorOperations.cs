
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class VectorOperations
{
    public static float DotProduct(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * a.z * b.z;

    }

    public static Vector3 Centroid(Vector3 a, Vector3 b, Vector3 c)
    {
        return a + b + c / 3;
    }

    public static Vector3 CrossProduct(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x
        );
    }

    public static Matrix4x4 TranslationMatrix(Vector3 transform)
    {
        Matrix4x4 matrix = Matrix4x4.identity;
        matrix[0, 3] = transform.x;
        matrix[1, 3] = transform.y;
        matrix[2, 3] = transform.z;
        return matrix;
    }


    public static Matrix4x4 ScaleMatrix(Vector3 scale)
    {
        Matrix4x4 matrix = Matrix4x4.identity;
        matrix[0, 0] = scale.x;
        matrix[1, 1] = scale.y;
        matrix[2, 2] = scale.z;
        return matrix;
    }


    public static Matrix4x4 RotateXMatrix(float degrees)
    {
        Matrix4x4 matrix = Matrix4x4.identity;
        matrix[1, 1] = Mathf.Cos(Mathf.Deg2Rad * degrees);
        matrix[1, 2] = -Mathf.Sin(Mathf.Deg2Rad * degrees);

        matrix[2, 1] = Mathf.Sin(Mathf.Deg2Rad * degrees);
        matrix[2, 2] = Mathf.Cos(Mathf.Deg2Rad * degrees);


        return matrix;
    }

    public static Matrix4x4 RotateYMatrix(float degrees)
    {
        Matrix4x4 matrix = Matrix4x4.identity;
        matrix[0, 0] = Mathf.Cos(Mathf.Deg2Rad * degrees);
        matrix[0, 2] = Mathf.Sin(Mathf.Deg2Rad * degrees);

        matrix[2, 0] = -Mathf.Sin(Mathf.Deg2Rad * degrees);
        matrix[2, 2] = Mathf.Cos(Mathf.Deg2Rad * degrees);

        return matrix;
    }

    public static Matrix4x4 RotateZMatrix(float degrees)
    {
        Matrix4x4 matrix = Matrix4x4.identity;
        matrix[0, 0] = Mathf.Cos(Mathf.Deg2Rad * degrees);
        matrix[0, 1] = -Mathf.Sin(Mathf.Deg2Rad * degrees);

        matrix[1, 0] = Mathf.Sin(Mathf.Deg2Rad * degrees);
        matrix[1, 1] = Mathf.Cos(Mathf.Deg2Rad * degrees);

        return matrix;
    }


}