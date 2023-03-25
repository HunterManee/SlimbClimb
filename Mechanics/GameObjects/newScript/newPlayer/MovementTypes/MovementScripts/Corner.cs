using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Corner", menuName = "Movements/Corner")]
public class Corner : MovementScript
{
    [SerializeField] float timePerRevolution = 2;
    [SerializeField] float Acceleration = 1;

    public override void Move(NewPlayerBrain newPlayerBrain = null)
    {
        base.Move(newPlayerBrain);

        //Enviroment Information
        NewEnviromentBrain newEnviromentBrain =
            newPlayerBrain.getNewEnviromentBrain("CurrentConnection");

        GameObject ClockwiseCorner = newEnviromentBrain.getCorner(int.Parse(
            newPlayerBrain.getConnectionPoint("CurrentConnection").name));

        float radianClockwise =
            newEnviromentBrain.getCornerRadian(ClockwiseCorner.name);

        GameObject CounterClockwiseCorner =
            newEnviromentBrain.getCorner(int.Parse(ClockwiseCorner.name) + 1);

        float radianCounterClockwise =
            newEnviromentBrain.getCornerRadian(CounterClockwiseCorner.name);

        float cornerType =
            newEnviromentBrain.getCornerType(ClockwiseCorner.name);

        float onCornerRadian =
            MathV.ATan2(MathV.PositionalVector(radianCounterClockwise) +
                MathV.PositionalVector(radianClockwise));

        Vector3 onCornerNormVector =
            MathV.RotateVector(MathV.DirectionalVector(onCornerRadian),
                cornerType <= 0 ? Mathf.PI * .5f : Mathf.PI * 1.5f);

        Vector3 onCornerAbsNormVector =
            MathV.ABS(onCornerNormVector);

        if(cornerType > 0 && //Concave Corner
           onCornerAbsNormVector.x != onCornerAbsNormVector.y)
        {
            onCornerNormVector =
                new Vector3(onCornerAbsNormVector.x < onCornerAbsNormVector.y ?
                                                  0 : onCornerNormVector.x,
                            onCornerAbsNormVector.y < onCornerAbsNormVector.x ?
                                                  0 : onCornerNormVector.y,
                                                  0);
        }

        Vector3 cornerFlexInput =
            MathV.SignVector(onCornerNormVector);

        //Speed
        float initialRadian = newPlayerBrain.getReferenceVector().z;

        Vector3 currentVelocityMatrix = newPlayerBrain.getCurrentVelocityMatrix();

        float velocityDistance = MathV.Distance(currentVelocityMatrix);
        velocityDistance = (velocityDistance == 0) ?
            Acceleration * Time.fixedDeltaTime : velocityDistance;

        float resolution = timePerRevolution /
            (velocityDistance * newPlayerBrain.ScaledTime());

        //Input Space (Needs Work)
        float movingDirection = 0;

        if (Mathf.Abs(MathV.getRotationRadian(radianCounterClockwise, radianClockwise))
                >= Mathf.PI * .5f &&
            (PlayerInput == Vector3.zero ||
            (PlayerInput.x == cornerFlexInput.x &&
             PlayerInput.x != 0)||
            (PlayerInput.y == cornerFlexInput.y &&
             PlayerInput.y != 0)))
        {
            movingDirection =
                MathV.getRotationRadian(initialRadian, onCornerRadian);
        }
        else
        {
            float radClockwise =
                radianClockwise;

            Vector3 positionalClockwise =
                        MathV.PositionalVector(radClockwise);

            float radCounterClockwise =
                radianCounterClockwise + Mathf.PI;

            Vector3 positionalCounterClockwise =
                        MathV.PositionalVector(radCounterClockwise);

            if(PlayerInput.y < 0)
            {
                if(positionalClockwise.y <
                   positionalCounterClockwise.y)
                {
                    movingDirection =
                        MathV.getRotationRadian(initialRadian, radianClockwise);
                }
                else
                {
                    movingDirection =
                         MathV.getRotationRadian(initialRadian, radianCounterClockwise);
                }
            }
            else if (PlayerInput.y > 0)
            {
                if (positionalClockwise.y >
                   positionalCounterClockwise.y)
                {
                    movingDirection =
                        MathV.getRotationRadian(initialRadian, radianClockwise);
                }
                else
                {
                    movingDirection =
                        MathV.getRotationRadian(initialRadian, radianCounterClockwise);
                }
            }
            else if (PlayerInput.x > 0)
            {
                if (positionalClockwise.x >
                   positionalCounterClockwise.x)
                {
                    movingDirection =
                        MathV.getRotationRadian(initialRadian, radianClockwise);
                }
                else
                {
                    movingDirection =
                        MathV.getRotationRadian(initialRadian, radianCounterClockwise);
                }
            }
            else if (PlayerInput.x < 0)
            {
                if (positionalClockwise.x <
                   positionalCounterClockwise.x)
                {
                    movingDirection =
                        MathV.getRotationRadian(initialRadian, radianClockwise);
                }
                else
                {
                    movingDirection =
                        MathV.getRotationRadian(initialRadian, radianCounterClockwise);
                }
            }

        }

        movingDirection = movingDirection == 0 ? 0 :
            ((movingDirection > 0) ? 1 : -1);

        float deltaRadian =
            (2 * Mathf.PI / resolution) *
            movingDirection;


        float finalRadian = initialRadian + deltaRadian;

        
        //Concave
        float deltaPositionRadian =
            newEnviromentBrain.getCornerRotationRadian(ClockwiseCorner.name);

        float toPlayerTheta =
            (Mathf.PI - (deltaPositionRadian <= 0 ? 0 : deltaPositionRadian)) * .5f;

        float distanceFromCorner = toPlayerTheta <= 0 ? 0 :
            newPlayerBrain.getPlayerRadius() / Mathf.Tan(toPlayerTheta);

        if (movingDirection <= 0)//Clockwise
        {
            if (cornerType <= 0)
            {
                Debug.Log(MathV.getRotationRadian(radianCounterClockwise, initialRadian));
                Debug.Log(MathV.getRotationRadian(radianCounterClockwise, finalRadian));


                if (MathV.getRotationRadian(radianClockwise,
                                            initialRadian) >= 0 &&
                    MathV.getRotationRadian(radianClockwise,
                                            finalRadian) <= 0)
                {
                    newPlayerBrain.setConnectedCorner("CurrentConnection", ClockwiseCorner);
                    newPlayerBrain.setMatrixPositionWeight(.001f);

                    finalRadian = radianClockwise;
                }
            }
            else
            {
                if (MathV.getRotationRadian(radianCounterClockwise,
                            initialRadian) >= 0 &&
                    MathV.getRotationRadian(radianCounterClockwise,
                            finalRadian) <= 0)
                {
                    newPlayerBrain.setConnectedCorner("CurrentConnection", CounterClockwiseCorner);

                    float sideLength =
                        newEnviromentBrain.getSideDistance(CounterClockwiseCorner.name);
                    newPlayerBrain.setMatrixPositionWeight(sideLength - distanceFromCorner - .001f);

                    finalRadian = radianCounterClockwise;
                }

            }

        }
        else //Counter Clockwise
        {
            if (cornerType <= 0)
            {
                if (MathV.getRotationRadian(radianCounterClockwise,
                                            initialRadian) <= 0 &&
                    MathV.getRotationRadian(radianCounterClockwise,
                                            finalRadian) >= 0)
                {
                    newPlayerBrain.setConnectedCorner("CurrentConnection", CounterClockwiseCorner);

                    float sideLength =
                        newEnviromentBrain.getSideDistance(CounterClockwiseCorner.name);

                    newPlayerBrain.setMatrixPositionWeight(sideLength - .001f);

                    finalRadian = radianCounterClockwise;
                }
            }
            else
            {
                if (MathV.getRotationRadian(radianClockwise,
                                            initialRadian) <= 0 &&
                    MathV.getRotationRadian(radianClockwise,
                                            finalRadian) >= 0)
                {
                    newPlayerBrain.setConnectedCorner("CurrentConnection", ClockwiseCorner);
                    newPlayerBrain.setMatrixPositionWeight(distanceFromCorner + .001f);

                    finalRadian = radianClockwise;
                }
            }
        }
        

        Vector3 accelerationVector =
        new Vector3(0, 0, MathV.ClampRad(finalRadian));

        newPlayerBrain.addAccelerationToTimeLine(accelerationVector);

    }
}