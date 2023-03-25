using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Side", menuName = "Movements/Side")]
public class Side : MovementScript
{
    [SerializeField] float Acceleration = 1;

    public override void Move(NewPlayerBrain newPlayerBrain)
    {
        base.Move(newPlayerBrain);

        float newRadian = newPlayerBrain.getNewEnviromentBrain("CurrentConnection").
             getCornerRadian(newPlayerBrain.getConnectedCorner("CurrentConnection").name);

        Vector3 inputSignVector = MathV.SignVector(PlayerInput);

        Vector3 refVector =
            newPlayerBrain.getReferenceVector();
        Vector3 positionalSignVector =
            MathV.SignVector(refVector);

        Vector3 playerInputRef = refVector;

        if (inputSignVector == Vector3.zero)
        {
            playerInputRef = inputSignVector;
        }
        else
        {
            if (inputSignVector.x != positionalSignVector.x &&
               inputSignVector.x != 0)
            {
                playerInputRef = MathV.RotateVector(refVector, Mathf.PI);
            }
            else if (inputSignVector.y != positionalSignVector.y &&
                     inputSignVector.y != 0)
            {
                playerInputRef = MathV.RotateVector(refVector, Mathf.PI);
            }

            /*
            newPlayerBrain.setFacingDirection
                (playerInputRef == newPlayerBrain.getReferenceVector());
            */

            MovingDirection =
               (playerInputRef == refVector) ? 1 : -1;
        }

        Vector3 accelerationVector = new Vector3(MovingDirection * Acceleration, 0, newRadian);

        newPlayerBrain.addAccelerationToTimeLine(accelerationVector);

    }
}
