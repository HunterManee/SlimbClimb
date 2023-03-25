using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathV
{
    public static float ClampRad(float Radian)
    {
        //0 <= Radian < 2 * Mathf.PI

        while (Radian < 0 || Radian >= 2 * Mathf.PI)
        {
            while (Radian < 0)
            {
                Radian += 2 * Mathf.PI;
            }

            while (Radian >= 2 * Mathf.PI)
            {
                Radian -= 2 * Mathf.PI;
            }
        }

        return Radian;
    }

    public static float ATan2(Vector3 Vector)
    {
        //0 >= theta < 2 * Mathf.Pi Radians

        float rad = Mathf.Atan2(Vector.y, Vector.x);

        rad = ClampRad(rad);

        return rad;
    }


    public static float Distance(Vector3 Vector)
    {
        //Always Positive
        return Mathf.Sqrt(Vector.x * Vector.x +
                          Vector.y * Vector.y);
    }

    public static Vector3 DirectionalVector(float Radians)
    {
        Radians = ClampRad(Radians);

        return new Vector3(Mathf.Cos(Radians), Mathf.Sin(Radians), Radians);
    }

    public static Vector3 PositionalVector(float Radians)
    {

        Radians = ClampRad(Radians);

        return new Vector3(Mathf.Cos(Radians), Mathf.Sin(Radians), 0);
    }


    public static Vector3 SignVector(Vector3 Vector)
    {

        float x = 0;
        float y = 0;

        if (Vector.x != 0)
        {
            x = Vector.x > 0 ? 1 : -1;
        }

        if (Vector.y != 0)
        {
            y = Vector.y > 0 ? 1 : -1;
        }

        Vector3 signVector =
               new Vector3(x, y, Vector.z);

        return signVector;
    }

    public static Vector3 MatrixVector(Vector3 to, Vector3 from)
    {
        Vector3 pDistanceVector = to - from;

        float pRadian = ATan2(pDistanceVector);

        float pMagnitude = Distance(pDistanceVector);

        Vector3 matrixVector = new Vector3(pMagnitude * Mathf.Cos(pRadian),
                                             pMagnitude * Mathf.Sin(pRadian),
                                             pRadian);


        return matrixVector;
    }

    public static Vector3 RotateVector(Vector3 Vector, float deltaRadians = Mathf.PI * .5f)
    {
        float vDistance = Distance(Vector);

        float vRadians = ATan2(Vector);

        float rad = ClampRad(vRadians + deltaRadians);

        Vector3 rotatedVector = PositionalVector(rad) * vDistance;

        return new Vector3(rotatedVector.x,
                           rotatedVector.y,
                           rad);
    }

    public static float getRotationRadian(float RadA, float RadB)
    {
        // -Pi <= return < Pi

        float radA = ClampRad(RadA);
        float radB = ClampRad(RadB);

        if (0 <= radA && radA < Mathf.PI)
        {
            if (radA <= radB && radB < radA + Mathf.PI)
            {
                return radB - radA;
            }
            else
            {
                if (radA > radB)
                {
                    return -(radA - radB);
                }
                else if (radB >= radA + Mathf.PI)
                {
                    return -((radA + 2 * Mathf.PI) - radB);
                }
            }
        }
        else if (Mathf.PI <= radA && radA < 2 * Mathf.PI)
        {
            if (radA > radB && radB >= radA - Mathf.PI)
            {
                return -(radA - radB);
            }
            else
            {
                if (radA < radB)
                {
                    return radB - radA;
                }
                else if (radB < radA - Mathf.PI)
                {
                    return ((radB + 2 * Mathf.PI) - radA);
                }
            }
        }


        return 0;
    }


    public static float DotProduct(Vector3 MatrixVector, float RefRadian)
    {
        float rad =
            Mathf.Abs(getRotationRadian(RefRadian, MatrixVector.z));

        float distance = Distance(MatrixVector);

        return distance * Mathf.Cos(rad);
    }


    public static float CrossProduct(Vector3 MatrixVector, float RefRadian)
    {

        float rad = MatrixVector.z - ClampRad(RefRadian);

        float distance = Distance(MatrixVector);

        return Mathf.Abs(Mathf.Sin(rad) * distance);
    }

    
    public static Vector3 RoundVector(Vector3 vector, float percision = .01f)
    {
        vector = new Vector3(Mathf.Round(vector.x / percision) * percision,
                             Mathf.Round(vector.y / percision) * percision,
                             0);

        return new Vector3(vector.x, vector.y, vector.z == 0 ? 0 : ATan2(vector));
    }

    public static Vector3 ABS(Vector3 Vector)
    {
        //Calculations only from the first quadrent ++

        Vector3 newVector = new Vector3(Mathf.Abs(Vector.x),
                                        Mathf.Abs(Vector.y),
                                        0);

        newVector = new Vector3(newVector.x, newVector.y, ATan2(newVector));

        return newVector;
    }
    

}
