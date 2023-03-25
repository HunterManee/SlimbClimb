using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewPlayerBrain : NewPlayerOrientation
{
    protected Dictionary<string, GameObject[]> ConnectionInformation = new Dictionary<string, GameObject[]>();
    /*
     GameObject[#]:
        0: collisionPoint
        1: the corner of the corresponding side
        2: the parent block to the corner
     */

    bool FacingClockwise = true;
    public bool isFacingClockwise() { return FacingClockwise; }
    public void setFacingDirection(bool isClockwise) { FacingClockwise = isClockwise; }

    [SerializeField] string CurrentMovement = "Default Movement";
    public string getCurrentMovement() { return CurrentMovement; }
    public void setCurrentMovement(string movementKey) { CurrentMovement = movementKey; }

    [SerializeField] private ScriptableObject[] Movements;
    private Dictionary<string, MovementScript> movementTypes = new Dictionary<string, MovementScript>();

    protected void Start()
    {

        foreach(ScriptableObject movement in Movements)
        {
            movementTypes[movement.name] = (MovementScript)movement;
        }
    }

    protected override void Update()
    {
        base.Update();

        if(countConnectionInformation() == 2)
        {
            CurrentMovement = "Two Point Connection";
        }
        else if (countConnectionInformation() == 1)
        {

            NewEnviromentBrain newEnviromentBrain =
                getNewEnviromentBrain("CurrentConnection").GetComponent<NewEnviromentBrain>();

            newEnviromentBrain.setMatrixOnEnviroment(this);

            newEnviromentBrain.getBaseMovementType(this);

        }
        else //No Connection
        {
            CurrentMovement = "Default Movement";
        }

        movementTypes[CurrentMovement].Move(this);

        updateVelocityMatrixFromTimeLine();

        movePlayer();

        setPlayerTransform(this);
    }

    public Vector3 getPlayerInput()
    {
        Vector3 PlayerInput = Vector3.zero;

        if (Input.GetAxisRaw("Vertical") != 0)
        {
            PlayerInput = new Vector3 (0,Input.GetAxisRaw("Vertical"), 0); 
        }
        else if (Input.GetAxisRaw("Horizontal") != 0)
        {
            PlayerInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
        }

        return PlayerInput;
    }

    private void movePlayer()
    {

        Vector3 currentVectorMatrix = getCurrentVelocityMatrix();


        float Distance = (isMovingClockwise() ? 1 : -1) *
            MathV.Distance(currentVectorMatrix) * ScaledTime();


        setMatrixPositionWeight(Distance + getMatrixPositionWeight());


        Vector3 initialPosition = getMatrixPosition();

        Vector3 deltaPosition = currentVectorMatrix * ScaledTime();

        Vector3 projectedPosition = initialPosition + deltaPosition;

        setMatrixPosition(projectedPosition);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject ThisObject = collision.gameObject;

        if (ThisObject.TryGetComponent(out NewEnviromentBrain newEnviromentBrain))
        {
            clearTimeLine();

            PlayerRigidbody2D.gravityScale = 0;

            PlayerRigidbody2D.velocity = Vector3.zero;

            GameObject ConnectionPoint = new GameObject("MEOW");


            if (countConnectionInformation() == 0)
            {
                GameObject ConnectedCorner =
                    newEnviromentBrain.getEnviromentSideCorner(this,
                    ConnectionPoint, newEnviromentBrain);


                newEnviromentBrain.setMatrixOnContact(this,
                    ConnectedCorner, newEnviromentBrain);

                ConnectionInformation["CurrentConnection"] =
                    new GameObject[] { ConnectionPoint, ConnectedCorner, newEnviromentBrain.gameObject };
            }
            else
            {
                if(getNewEnviromentBrain("CurrentConnection") != newEnviromentBrain)
                {
                    Debug.Log("Previous Connection");
                    Destroy(ConnectionPoint);
                }
                else
                {
                    Destroy(ConnectionPoint);
                }
            }

        }
    }

    public int countConnectionInformation()
    {
        return ConnectionInformation.Count;
    }

    public GameObject getConnectionPoint(string ConnectionKey)
    {
        return ConnectionInformation[ConnectionKey][0];
    }

    public GameObject getConnectedCorner(string ConnectionKey)
    {
        return ConnectionInformation[ConnectionKey][1];
    }
    public void setConnectedCorner(string ConnectionKey, GameObject newCorner)
    {
        ConnectionInformation[ConnectionKey][1] = newCorner;
    }

    public NewEnviromentBrain getNewEnviromentBrain(string ConnectionKey)
    {
        return ConnectionInformation[ConnectionKey][2].GetComponent<NewEnviromentBrain>();
    }


}
