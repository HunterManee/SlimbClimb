using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Default
[CreateAssetMenu(fileName = "Default Movement", menuName = "Movements/Default Movement")]
public class DefaultMovement : MovementScript
{

    [SerializeField] float Acceleration = 2;

    public override void Move(NewPlayerBrain newPlayerBrain = null)
    {
        float newTheta = (newPlayerBrain.getCurrentVelocityMatrix() == Vector3.zero) ?
            3 * Mathf.PI * .5f : MathV.ATan2(newPlayerBrain.getCurrentVelocityMatrix());

        Vector3 accelerationVector = new Vector3(Acceleration, 0, newTheta);

        newPlayerBrain.addAccelerationToTimeLine(accelerationVector);
    }

}
