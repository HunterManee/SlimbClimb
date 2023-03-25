using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D)),
 RequireComponent(typeof(Rigidbody2D))]

public class NewPlayerComponents : MonoBehaviour
{
    protected GameObject ThisPlayer;

    protected Transform PlayerTransform;
    protected CircleCollider2D PlayerCircleCollider2D;
    protected Rigidbody2D PlayerRigidbody2D;

    protected virtual void Awake()
    {
        ThisPlayer = gameObject;

        PlayerTransform = ThisPlayer.GetComponent<Transform>();
        PlayerCircleCollider2D = ThisPlayer.GetComponent<CircleCollider2D>();
        PlayerRigidbody2D = ThisPlayer.GetComponent<Rigidbody2D>();
    }

    protected void setPlayerTransform(NewPlayerBrain newPlayerBrain)
    {

        PlayerTransform.localScale = new Vector3(newPlayerBrain.isFacingClockwise() ? 1 : -1,
                                                 PlayerTransform.localScale.y,
                                                 PlayerTransform.localScale.z);

        PlayerTransform.localEulerAngles =
            new Vector3(PlayerTransform.localEulerAngles.x,
                        PlayerTransform.localEulerAngles.y,
                        newPlayerBrain.getReferenceVector().z * Mathf.Rad2Deg);


         PlayerTransform.position = newPlayerBrain.getMatrixPosition();
        
    }

    public float getPlayerRadius()
    {
        return PlayerCircleCollider2D.radius;   
    }

}
