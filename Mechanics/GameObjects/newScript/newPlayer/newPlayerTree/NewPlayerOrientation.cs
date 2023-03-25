using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerOrientation : NewPlayerMatrix
{
    [SerializeField, Range(0, 5)] protected float TimeScale = 1;
    float scaledDeltaTime = 0;
    public float ScaledTime() { return scaledDeltaTime; }

    protected virtual void Update()
    {
        scaledDeltaTime = Time.fixedDeltaTime * TimeScale;
    }


    Vector3 CurrentVelocityMatrix = Vector3.zero;
    public Vector3 getCurrentVelocityMatrix() { return CurrentVelocityMatrix; }
    protected void setCurrentVelocityMatrix(Vector3 velocityVector)
    { CurrentVelocityMatrix = velocityVector; }


    protected void updateVelocityMatrixFromTimeLine()
    {

        if (getTimeLineCount() > 0)
        {
            Vector3 AccelerationMatrix = getAccelerationMatrixFromTimeLine();

            setMatrixRotation(AccelerationMatrix.z);

            Dictionary<string, Vector3> newMatrix = getInitialPlayerMatrix();

            newMatrix = MathM.ScaleMatrix(newMatrix, Time.fixedDeltaTime);

            CurrentVelocityMatrix = MathM.TransformVectorMatrix(newMatrix, AccelerationMatrix);
        }

    }

    bool movingClockwise = true;
    public bool isMovingClockwise()
    {
        if (CurrentVelocityMatrix != Vector3.zero)
        {
            movingClockwise = (Mathf.Round(MathV.ATan2(CurrentVelocityMatrix) * 10) / 10 ==
                              Mathf.Round(getReferenceVector().z * 10) / 10);
        }

        return movingClockwise;

    }

    List<Vector3> AccelerationMatrixTimeLine = new List<Vector3>();
    // x: Cos, y: Sin, z: Updated Matrix Rotation

    public int getTimeLineCount()
    {
        return AccelerationMatrixTimeLine.Count;
    }


    public void addAccelerationToTimeLine(Vector3 AccelerationMatrix)
    {
        AccelerationMatrixTimeLine.Add(AccelerationMatrix);
    }

    protected Vector3 getAccelerationMatrixFromTimeLine()
    {
        Vector3 AccelerationMatrix = AccelerationMatrixTimeLine[0];
        AccelerationMatrixTimeLine.RemoveAt(0);

        return AccelerationMatrix;
    }

    protected void clearTimeLine()
    {
        AccelerationMatrixTimeLine = new List<Vector3>();
    }

}
