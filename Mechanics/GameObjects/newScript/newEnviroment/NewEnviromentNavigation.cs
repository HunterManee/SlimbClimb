using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnviromentNavigation : MonoBehaviour
{
    //Before Collision
    public GameObject getEnviromentSideCorner(NewPlayerBrain newPlayerBrain,
        GameObject ConnectionPoint, NewEnviromentBrain newEnviromentBrain)
    {
        float smallestCrossProduct = 1;
        int cornerOfBlockSide = 0;
        for (int corner = 0; corner < newEnviromentBrain.getNumberOfBlockVerticies(); corner++)
        {
            GameObject Corner = newEnviromentBrain.getCorner(corner);

            Vector2 cornerPosition = Corner.transform.position;

            float cornerRefTheta =
                newEnviromentBrain.getCornerRadian(Corner.name);

            float sideLength = newEnviromentBrain.getSideDistance(Corner.name);

            float dotProduct = MathV.DotProduct(MathV.MatrixVector(newPlayerBrain.getMatrixPosition(),
                                                                      cornerPosition), cornerRefTheta);

            if (0 <= dotProduct && dotProduct <= sideLength)
            {
                Vector2 cornerRefVector = MathV.PositionalVector(cornerRefTheta) * dotProduct;

                Vector2 cornerNormVector = MathV.RotateVector(cornerRefVector) * newPlayerBrain.getPlayerRadius();


                Vector2 projectedPlayerPosition = cornerPosition + cornerRefVector + cornerNormVector;


                Vector2 matrixVector =
                    MathV.MatrixVector(newPlayerBrain.getMatrixPosition(), projectedPlayerPosition);

                float crossProduct = MathV.CrossProduct(matrixVector, cornerRefTheta);

                if (crossProduct < smallestCrossProduct)
                {
                    smallestCrossProduct = crossProduct;
                    cornerOfBlockSide = corner;
                }
            }

        }

        ConnectionPoint.name = newEnviromentBrain.getCorner(cornerOfBlockSide).name;
        return newEnviromentBrain.getCorner(cornerOfBlockSide);

    }

    public void setMatrixOnContact(NewPlayerBrain newPlayerBrain,
        GameObject connectedCorner, NewEnviromentBrain newEnviromentBrain)
    {

        Vector3 cornerPosition = connectedCorner.transform.position;
        float cornerRadian = newEnviromentBrain.getCornerRadian(connectedCorner.name);

        Vector3 matrixVector =
            MathV.MatrixVector(newPlayerBrain.getMatrixPosition(),
                               cornerPosition);
        float sideLength =
            newEnviromentBrain.getSideDistance(connectedCorner.name);

        float dotProduct =
            Mathf.Clamp(MathV.DotProduct(matrixVector, cornerRadian), 0, sideLength);

        newPlayerBrain.setMatrixPositionWeight(dotProduct);

        if (0 < dotProduct && dotProduct < sideLength)
        {
            newPlayerBrain.setMatrixRotation(cornerRadian);
        }
        else
        {
            cornerPosition =
                cornerPosition + MathV.PositionalVector(cornerRadian) * dotProduct;

            Vector3 matrixNorm =
                MathV.MatrixVector(newPlayerBrain.getMatrixPosition(), cornerPosition);

            Vector3 matrixRef =
                MathV.RotateVector(matrixNorm, -Mathf.PI * .5f);

            newPlayerBrain.setMatrixRotation(matrixRef.z);
        }
    }

    //After Collision
    public void getBaseMovementType(NewPlayerBrain newPlayerBrain)
    {

        GameObject connectedCorner = newPlayerBrain.getConnectedCorner("CurrentConnection");

        GameObject cornerAdjacent = newPlayerBrain.getNewEnviromentBrain("CurrentConnection").
                         getCorner(int.Parse(connectedCorner.name) - 1);

        GameObject movingCorner = newPlayerBrain.getNewEnviromentBrain("CurrentConnection").
            getCorner(int.Parse(newPlayerBrain.getConnectionPoint("CurrentConnection").name));

        if (newPlayerBrain.getCurrentMovement() != "Corner")
        {
            movingCorner =
                   newPlayerBrain.isMovingClockwise() ?
                           cornerAdjacent : connectedCorner;

        }

        newPlayerBrain.getConnectionPoint("CurrentConnection").name = movingCorner.name;

        if (DistanceFromMovingCorner(newPlayerBrain) <= 0)
        {
            newPlayerBrain.setCurrentMovement("Corner");
        }
        else
        {
            if(newPlayerBrain.getPlayerInput() == Vector3.zero)
            {
                newPlayerBrain.setCurrentMovement("Slide");
            }
            else 
            {
                newPlayerBrain.setCurrentMovement("Side");
            }
        }
    }

    //Needs clean up
    private float DistanceFromMovingCorner(NewPlayerBrain newPlayerBrain)
    {

        NewEnviromentBrain newEnviromentBrain =
                newPlayerBrain.getNewEnviromentBrain("CurrentConnection");

        GameObject ClockwiseCorner =
            newEnviromentBrain.getCorner(int.Parse(
            newPlayerBrain.getConnectionPoint("CurrentConnection").name));

        GameObject CounterClockwiseCorner =
            newEnviromentBrain.
            getCorner(int.Parse(ClockwiseCorner.name) + 1);

        GameObject connectedCorner =
            newPlayerBrain.getConnectedCorner("CurrentConnection");

        float refRadian =
            newEnviromentBrain.
            getCornerRadian(connectedCorner.name);


        Vector3 positionConcaveCorner = ClockwiseCorner.transform.position;

        if (newEnviromentBrain.getCornerType(ClockwiseCorner.name) > 0)
        {
            float radianClockwise =
                    newEnviromentBrain.
                    getCornerRadian(ClockwiseCorner.name);

            float radianCounterClockwise =
                    MathV.ClampRad(newEnviromentBrain.
                                   getCornerRadian(CounterClockwiseCorner.name) + Mathf.PI);

            Vector3 positionalClockwise = MathV.PositionalVector(radianClockwise);
            Vector3 positionalCounterClockwise =
                MathV.PositionalVector(radianCounterClockwise);

            float deltaPositionRadian =
                newEnviromentBrain.getCornerRotationRadian(ClockwiseCorner.name);

            float toPlayerTheta = deltaPositionRadian <= 0 ? 0 :
                             (Mathf.PI - deltaPositionRadian) * .5f;

            float distanceFromCorner =
                newPlayerBrain.getPlayerRadius() / Mathf.Tan(toPlayerTheta);

            if (ClockwiseCorner.name == connectedCorner.name)
            {
                positionConcaveCorner +=
                    positionalClockwise * distanceFromCorner;
            }
            else
            {
                positionConcaveCorner +=
                    positionalCounterClockwise * distanceFromCorner;
            }
        }

        Vector3 matrixVector =
             MathV.MatrixVector(newPlayerBrain.getMatrixPosition(),
             positionConcaveCorner);

        float refDotPoduct =
            MathV.DotProduct(matrixVector,
            (ClockwiseCorner.name == connectedCorner.name) ?
            refRadian : refRadian + Mathf.PI);

        return refDotPoduct;
    }

    public void setMatrixOnEnviroment(NewPlayerBrain newPlayerBrain)
    {

        NewEnviromentBrain newEnviromentBrain =
            newPlayerBrain.getNewEnviromentBrain("CurrentConnection");

        GameObject connectedCorner = newEnviromentBrain.getCorner(int.Parse
           (newPlayerBrain.getConnectedCorner("CurrentConnection").name));

        Vector3 newPosition = connectedCorner.transform.position;

        float cornerRad = newEnviromentBrain.getCornerRadian(connectedCorner.name);

        newPosition +=
            MathV.PositionalVector(cornerRad) *
            newPlayerBrain.getMatrixPositionWeight();

        newPlayerBrain.getConnectionPoint("CurrentConnection").transform.position =
            newPosition;
        newPlayerBrain.getConnectionPoint("CurrentConnection").transform.localEulerAngles =
            new Vector3(0, 0, newPlayerBrain.getReferenceVector().z * Mathf.Rad2Deg);

        GameObject movingCorner = newEnviromentBrain.getCorner(int.Parse
            (newPlayerBrain.getConnectionPoint("CurrentConnection").name));

        float cornerType =
            newEnviromentBrain.getCornerType(movingCorner.name);

        if (cornerType >= 0)
        {
            newPosition +=
                MathV.RotateVector(MathV.PositionalVector(cornerRad)) *
                newPlayerBrain.getPlayerRadius();
        }
        else
        {
            newPosition +=
                MathV.PositionalVector(newPlayerBrain.getVectorNorm().z) *
                newPlayerBrain.getPlayerRadius();
        }

        newPlayerBrain.setMatrixPosition(newPosition);
    }
}