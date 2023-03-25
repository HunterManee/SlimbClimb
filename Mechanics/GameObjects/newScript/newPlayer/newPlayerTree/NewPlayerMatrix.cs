using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewPlayerMatrix : NewPlayerComponents
{
    private float PlayerRadian = 0;
    protected Dictionary<string, Vector3> initialPlayerMatrix =
                                            new Dictionary<string, Vector3>();

    protected override void Awake()
    {
        base.Awake();

        initialPlayerMatrix["Pos"] = PlayerTransform.position;

        PlayerRadian = PlayerTransform.localEulerAngles.z * Mathf.Deg2Rad;
        setMatrixRotation(PlayerRadian);
    }

    protected Dictionary<string, Vector3> getInitialPlayerMatrix()
    {
        return initialPlayerMatrix;
    }


    public void setMatrixRotation(float newRad)
    {
        initialPlayerMatrix = MathM.RotateMatrix(initialPlayerMatrix, newRad);
    }


    public Vector3 getReferenceVector()
    {
        return initialPlayerMatrix["Ref"];
    }

    public Vector3 getVectorNorm()
    {
        return initialPlayerMatrix["Norm"];
    }
    
    public Vector3 getMatrixPosition()
    {
        return new Vector3(initialPlayerMatrix["Pos"].x,
                           initialPlayerMatrix["Pos"].y,
                           0);
    }
    
    
    public void setMatrixPosition(Vector3 newPosition)
    {
        initialPlayerMatrix["Pos"] = new Vector3(newPosition.x,
                                                 newPosition.y,
                                                 initialPlayerMatrix["Pos"].z);
    }

    public float getMatrixPositionWeight()
    {
        return initialPlayerMatrix["Pos"].z;
    }

    
    public void setMatrixPositionWeight(float newWeight)
    {
        initialPlayerMatrix["Pos"] = new Vector3(initialPlayerMatrix["Pos"].x,
                                                 initialPlayerMatrix["Pos"].y,
                                                 newWeight);
    }

    
    
}

