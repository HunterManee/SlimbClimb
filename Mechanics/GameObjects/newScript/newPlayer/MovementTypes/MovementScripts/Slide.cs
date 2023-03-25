using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slide", menuName = "Movements/Slide")]
public class Slide : MovementScript
{
    [SerializeField] float Acceleration = 1;

    [SerializeField] float MinimumSlideDeg = 15;


    public override void Move(NewPlayerBrain newPlayerBrain = null)
    {
        float newRadian = newPlayerBrain.getNewEnviromentBrain("CurrentConnection").
                    getCornerRadian(newPlayerBrain.getConnectedCorner("CurrentConnection").name);

        if (Mathf.Abs(Mathf.Sin(newRadian)) >
            Mathf.Abs(Mathf.Sin(MinimumSlideDeg * Mathf.Deg2Rad)))
        {
            Vector3 positionalVector = MathV.PositionalVector(newRadian);

            Vector3 positionalSignVector = MathV.SignVector(positionalVector);

            newPlayerBrain.setFacingDirection(positionalSignVector.y > 0);

            Vector3 accelerationVector =
                new Vector3(-positionalSignVector.y * Acceleration, 0, newRadian);

            newPlayerBrain.addAccelerationToTimeLine(accelerationVector);
        }
        else
        {
            newPlayerBrain.addAccelerationToTimeLine(new Vector3(0, 0, newRadian));
        }
    }
}
