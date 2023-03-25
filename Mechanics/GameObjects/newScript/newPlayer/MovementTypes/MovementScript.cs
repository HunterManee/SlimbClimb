using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementScript : ScriptableObject
{
    protected Vector3 PlayerInput;
    protected float MovingDirection = 0;

    protected bool FlexCorner;

    public virtual void Move(NewPlayerBrain newPlayerBrain = null)
    {
        PlayerInput = newPlayerBrain.getPlayerInput();
    }

    
}