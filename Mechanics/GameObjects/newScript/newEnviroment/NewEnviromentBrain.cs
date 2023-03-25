using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnviromentBrain : NewEnviromentNavigation
{
    //Block Components
    protected GameObject ENVIROMENT;
    protected Transform EnviromentTransform;
    PolygonCollider2D EnviromentPolygonCollider2D;


    GameObject[] enviromentCorners;

    //Dictionary relates ThisBlocks sides to its corners
    private Dictionary<GameObject, float[,]> colliderSideDimentions = new Dictionary<GameObject, float[,]>();
    /*
                    0                       1
            0: Theta                    0:CornerPosition.x
            1: Distance of Side         1:CorenrPosition.y
            2: deltaRadian              2:cornerType
     */
    private void Awake()
    {
        ENVIROMENT = gameObject;
        EnviromentPolygonCollider2D = ENVIROMENT.GetComponent<PolygonCollider2D>();
        EnviromentTransform = ENVIROMENT.transform;


        //Calibrate rotation before placing corner points
        Vector3 currentRotation = EnviromentTransform.eulerAngles;
        EnviromentTransform.eulerAngles = Vector3.zero;

        //Stores created corner GameObjects into array
        enviromentCorners = new GameObject[EnviromentPolygonCollider2D.points.Length];
        for (int p = 0; p < enviromentCorners.Length; p++)
        {
            enviromentCorners[p] = new GameObject(p.ToString());
            enviromentCorners[p].transform.parent = EnviromentTransform;

            //After corner GameObjects has been created and named they are placed onto the points of the polygon collider
            enviromentCorners[p].transform.position
                = new Vector2(EnviromentTransform.position.x + EnviromentPolygonCollider2D.points[p].x,
                              EnviromentTransform.position.y + EnviromentPolygonCollider2D.points[p].y);
        }

        EnviromentTransform.eulerAngles = currentRotation;

    }

    private void FixedUpdate()
    {
        setEniromentDimentions();
    }

    public int getNumberOfBlockVerticies()
    {
        return enviromentCorners.Length;
    }

    public GameObject getCorner(int corner)
    {
        while (corner < 0)
        {
            corner += enviromentCorners.Length;
        }

        return enviromentCorners[corner % getNumberOfBlockVerticies()];
    }

    //Expand For Bridge
    private void setEniromentDimentions()
    {
        int Verticies = getNumberOfBlockVerticies();
        //Takes each corner and previous corners and store dimentions of side(polar form) 
        for (int corner = 0; corner < Verticies; corner++)
        {

            Vector2 originalCorner =
                getCorner(corner).transform.position;

            Vector2 cornerClockwise =
                getCorner(corner - 1).transform.position;

            Vector3 matrixClockwise =
                MathV.MatrixVector(cornerClockwise, originalCorner);

            float Radian = matrixClockwise.z;

            float SideDistance =
                MathV.Distance(matrixClockwise);

            //isConvex
            Vector2 cornerCounterClockwise =
                getCorner(corner + 1).transform.position;

            Vector3 matrixCounterClockwise =
                MathV.MatrixVector(originalCorner, cornerCounterClockwise);

            float deltaRadian =
                MathV.getRotationRadian(matrixCounterClockwise.z, matrixClockwise.z);

            float cornerType =
                deltaRadian == 0 ? 0 : (deltaRadian < 0 ? -1 : 1);

            colliderSideDimentions[enviromentCorners[corner]] = new float[2, 3]
            {
                { Radian, SideDistance, deltaRadian },
                { originalCorner.x, originalCorner.y, cornerType }
             };
            //Debug.Log("Is corner" + corner + " Corner Type: " + deltaRadian);
        }
    }
    public float getCornerRadian(string corner)
    {
        GameObject Corner = getCorner(int.Parse(corner));

        return colliderSideDimentions[Corner][0, 0];
    }

    public float getSideDistance(string corner)
    {
        GameObject Corner = getCorner(int.Parse(corner));

        return colliderSideDimentions[Corner][0, 1];
    }

    /*
    public Vector2 getCornerPosition(string corner)
    {
        GameObject Corner = getCorner(int.Parse(corner));


        return new Vector2(colliderSideDimentions[Corner][1, 0],
                           colliderSideDimentions[Corner][1, 1]);
    }
    /**/

    public float getCornerRotationRadian(string corner)
    {
        GameObject Corner = getCorner(int.Parse(corner));

        return colliderSideDimentions[Corner][0, 2];
    }

    public float getCornerType(string corner)
    {
        GameObject Corner = getCorner(int.Parse(corner));

        return (colliderSideDimentions[Corner][1, 2]);  
    }

    

}
