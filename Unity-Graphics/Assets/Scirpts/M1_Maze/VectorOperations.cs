
using System.Collections;
using System.Collections.Generic;

using UnityEngine;




public class VectorOperations
{




    public static float DotProduct(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * a.z * b.z;

    }

    public static Vector3 CrossProduct(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x
        );
    }

    public static Matrix4x4 GetTranslationMatrix(Vector3 transform)
    {
        Matrix4x4 matrix = Matrix4x4.identity;
        matrix[0, 3] = transform.x;
        matrix[1, 3] = transform.y;
        matrix[2, 3] = transform.z;
        return matrix;
    }


    public static Matrix4x4 GetScaleMatrix(Vector3 scale)
    {
        Matrix4x4 matrix = Matrix4x4.identity;
        matrix[0, 0] = scale.x;
        matrix[1, 1] = scale.y;
        matrix[2, 2] = scale.z;
        return matrix;
    }


    public static Matrix4x4 GetXRotationMatrix(float degrees)
    {
        Matrix4x4 matrix = Matrix4x4.identity;
        matrix[1, 1] = Mathf.Cos(Mathf.Deg2Rad * degrees);
        matrix[1, 2] = -Mathf.Sin(Mathf.Deg2Rad * degrees);

        matrix[2, 1] = Mathf.Sin(Mathf.Deg2Rad * degrees);
        matrix[2, 2] = Mathf.Cos(Mathf.Deg2Rad * degrees);


        return matrix;
    }

    public static Matrix4x4 GetYRotationMatrix(float degrees)
    {
        Matrix4x4 matrix = Matrix4x4.identity;
        matrix[0, 0] = Mathf.Cos(Mathf.Deg2Rad * degrees);
        matrix[0, 2] = Mathf.Sin(Mathf.Deg2Rad * degrees);

        matrix[2, 0] = -Mathf.Sin(Mathf.Deg2Rad * degrees);
        matrix[2, 2] = Mathf.Cos(Mathf.Deg2Rad * degrees);

        return matrix;
    }

    public static Matrix4x4 GetZRotationMatrix(float degrees)
    {
        Matrix4x4 matrix = Matrix4x4.identity;
        matrix[0, 0] = Mathf.Cos(Mathf.Deg2Rad * degrees);
        matrix[0, 1] = -Mathf.Sin(Mathf.Deg2Rad * degrees);

        matrix[1, 0] = Mathf.Sin(Mathf.Deg2Rad * degrees);
        matrix[1, 1] = Mathf.Cos(Mathf.Deg2Rad * degrees);

        return matrix;
    }


    public static Vector3 ApplyMatrixToVector(Matrix4x4 matrix, Vector3 vector)
    {
        Vector4 homogeneousVector = vector;
        homogeneousVector.w = 1;

        return matrix * homogeneousVector;
    }


    public static Vector3 Scale(Vector3 vector, Vector3 scale)
    {
        Vector4 homogeneousVector = vector;
        homogeneousVector.w = 1;

        Matrix4x4 matrix = GetScaleMatrix(scale);

        return matrix * homogeneousVector;
    }

    public static Vector3 Translate(Vector3 vector, Vector3 units)
    {
        Vector4 homogeneousVector = vector;
        homogeneousVector.w = 1;

        Matrix4x4 matrix = GetTranslationMatrix(units);

        return matrix * homogeneousVector;
    }

    public static Vector3 RotateX(Vector3 vector, float degrees, Vector3 pivot)
    {
        Vector3 translatedVector = vector - pivot;

        Vector4 homogeneousVector = new Vector4(translatedVector.x, translatedVector.y, translatedVector.z, 1);

        Matrix4x4 matrix = GetXRotationMatrix(degrees);

        Vector4 rotatedVector = matrix * homogeneousVector;

        return new Vector3(rotatedVector.x, rotatedVector.y, rotatedVector.z) + pivot;
    }

    public static Vector3 RotateY(Vector3 vector, float degrees, Vector3 pivot)
    {
        Vector3 translatedVector = vector - pivot;

        Vector4 homogeneousVector = new Vector4(translatedVector.x, translatedVector.y, translatedVector.z, 1);

        Matrix4x4 matrix = GetYRotationMatrix(degrees);

        Vector4 rotatedVector = matrix * homogeneousVector;

        return new Vector3(rotatedVector.x, rotatedVector.y, rotatedVector.z) + pivot;
    }


    public static Vector3 RotateZ(Vector3 vector, float degrees, Vector3 pivot)
    {
        Vector3 translatedVector = vector - pivot;

        Vector4 homogeneousVector = new Vector4(translatedVector.x, translatedVector.y, translatedVector.z, 1);

        Matrix4x4 matrix = GetZRotationMatrix(degrees);

        Vector4 rotatedVector = matrix * homogeneousVector;

        return new Vector3(rotatedVector.x, rotatedVector.y, rotatedVector.z) + pivot;
    }




}