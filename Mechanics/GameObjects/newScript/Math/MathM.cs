using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public static class MathM
{
    public static Dictionary<string, Vector3> RotateMatrix(
    Dictionary<string, Vector3> initialMatrix, float newRadians)
    {
        Dictionary<string, Vector3> newMatrix = new Dictionary<string, Vector3>();


        newMatrix["Ref"] =
            new Vector3(Mathf.Cos(newRadians), Mathf.Sin(newRadians), newRadians);
        newMatrix["Norm"] =
            new Vector3(-Mathf.Sin(newRadians), Mathf.Cos(newRadians), newRadians + Mathf.PI * .5f);

        newMatrix["Pos"] = initialMatrix["Pos"];

        return newMatrix;
    }

    public static Dictionary<string, Vector3> ScaleMatrix(
    Dictionary<string, Vector3> initialMatrix, float Scalar)
    {
        Dictionary<string, Vector3> newMatrix = new Dictionary<string, Vector3>();

        newMatrix["Ref"] =
            new Vector3(Scalar * initialMatrix["Ref"].x, Scalar * initialMatrix["Ref"].y,
                        initialMatrix["Ref"].z);

        newMatrix["Norm"] =
            new Vector3(Scalar * initialMatrix["Norm"].x, Scalar * initialMatrix["Norm"].y,
                        initialMatrix["Norm"].z);

        newMatrix["Pos"] = initialMatrix["Pos"];

        return newMatrix;

    }


    public static Vector3 TransformVectorMatrix
    (Dictionary<string, Vector3> Matrix, Vector3 vectorMatrix)
    {
        Vector3 R = new Vector3
                    (Matrix["Ref"].x, Matrix["Ref"].y, Matrix["Ref"].z),
                N = new Vector3
                    (Matrix["Norm"].x, Matrix["Norm"].y, Matrix["Norm"].z);

        Vector3 transfromedVectorMatrix = new Vector3
        ( //new Ref Vector           new Norm Vector    original Position
            (vectorMatrix.x * R.x + vectorMatrix.y * N.x),      //new Position.x
            (vectorMatrix.x * R.y + vectorMatrix.y * N.y),      //new Position.y
            0                                                   //Place Holder
        );

        return transfromedVectorMatrix;
    }

}
