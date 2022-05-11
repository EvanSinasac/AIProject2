// Evan Sinasac - 1081418
// INFO6017 AI Project 2
// Description -
//          This is the main bulk of changes for this project so I'm putting my comment signature here.  The main
//          purpose of this project is to implement and showcase different flocking, formation and path following behaviours

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Coordinator : MonoBehaviour
{
    public GameObject[] vehicles;
    public Vector3[] circleFormation;
    public Vector3[] vFormation;
    public Vector3[] squareFormation;
    public Vector3[] TFormation;            // not used for project, keeping cause it's nice
    public Vector3[] lineFormation;
    public Vector3[] twoRowsFormation;
    Vector3[] positionOffset;
    public bool isSquareFormation = true;

    public GameObject[] goPositionOffsets;

    // Script controlling
    public PathFollowing pathFollowing;

    public bool flockingFollow = false;

    // Start is called before the first frame update
    void Start()
    {
        positionOffset = circleFormation;
        for (int index = 0; index != goPositionOffsets.Length; index++)
        {
            goPositionOffsets[index].transform.position = positionOffset[index];
        }


        // I have to generate the BOIDs as part of this project so I need to find them all
        //if (vehicles == null)
        //    vehicles = GameObject.FindGameObjectsWithTag("Unit");

        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Unit");

        for (int index = 0; index != gameObjects.Length; index++)
        {
            vehicles[index] = gameObjects[index];
        }


    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("space"))
        //{
        //    isSquareFormation = !isSquareFormation;
        //    if (isSquareFormation)
        //    {
        //        positionOffset = squareFormation;
        //    }
        //    else
        //    {
        //        positionOffset = TFormation;
        //    }
        //}
            
        if (Input.GetKeyDown("1"))
        {   // change formation to circle
            positionOffset = circleFormation;
            UpdatePositionOffsetObjects();
        }
        if (Input.GetKeyDown("2"))
        {   // change formation to V
            positionOffset = vFormation;
            UpdatePositionOffsetObjects();
        }
        if (Input.GetKeyDown("3"))
        {   // change formation to square
            positionOffset = squareFormation;
            UpdatePositionOffsetObjects();
        }
        if (Input.GetKeyDown("4"))
        {   // change formation to line
            positionOffset = lineFormation;
            UpdatePositionOffsetObjects();
        }
        if (Input.GetKeyDown("5"))
        {   // change formation to two rows
            positionOffset = twoRowsFormation;
            UpdatePositionOffsetObjects();
        }
        if (Input.GetKeyDown("6"))
        {   // switch to flocking behaviour
            for (int index = 0; index != vehicles.Length; index++)
            {
                vehicles[index].GetComponent<BOIDController>().SetFlocking();
            }
        }
        if (Input.GetKeyDown("7"))
        {   // return to formation behaviour
            for (int index = 0; index != vehicles.Length; index++)
            {
                vehicles[index].GetComponent<BOIDController>().SetFormation();
            }
        }
        if (Input.GetKeyDown("8"))
        {   // start following a path
            pathFollowing.followPath = true;
        }
        if (Input.GetKeyDown("9"))
        {   // reverse the path
            pathFollowing.ReversePath();
        }
        if (Input.GetKeyDown("0"))
        {   // stop following the path
            pathFollowing.followPath = false;
            // 
            UpdatePositionOffsetObjects();
        }
        if (Input.GetKeyDown("-"))
        {   // flocking follows the path
            flockingFollow = true;
            pathFollowing.followPath = true;
            for (int index = 0; index != vehicles.Length; index++)
            {
                vehicles[index].GetComponent<BOIDController>().SetFlocking();
            }
        }
        if (Input.GetKeyDown("="))
        {   // flocking stops following the path
            flockingFollow = false;
            pathFollowing.followPath = false;
            for (int i = 0; i < vehicles.Length; i++)
            {
                Flocking flockingUnit = vehicles[i].GetComponent<Flocking>();
                flockingUnit.followPath = false;
            }
            for (int index = 0; index != vehicles.Length; index++)
            {
                vehicles[index].GetComponent<BOIDController>().SetFormation();
            }
        }


        // realized the formation is meant to rotate when following the path, this (I believe) is the code that 
        // maintains the offset position around the pathFollowing unit
        for (int i = 0; i < vehicles.Length; i++)
        {
            Formation formationUnit = vehicles[i].GetComponent<Formation>();
            //formationUnit.SetTargetPosition(transform.position + positionOffset[i]);
            formationUnit.SetTargetPosition(/*transform.position + */goPositionOffsets[i].transform.position);
            // Debug.DrawLine(transform.position + positionOffset[i], vehicles[i].transform.position, Color.red);
            Debug.DrawLine(/*transform.position + */goPositionOffsets[i].transform.position, vehicles[i].transform.position, Color.red);
        }

        if (flockingFollow)
        {
            for (int i = 0; i < vehicles.Length; i++)
            {
                Flocking flockingUnit = vehicles[i].GetComponent<Flocking>();
                flockingUnit.followPath = true;
                flockingUnit.SetTargetPosition(transform.position);
                Debug.DrawLine(transform.position, vehicles[i].transform.position, Color.yellow);
            }
        }
        
    }

    private void UpdatePositionOffsetObjects()
    {
        for (int index = 0; index != goPositionOffsets.Length; index++)
        {
            goPositionOffsets[index].transform.position = transform.position + positionOffset[index];
        }
    }

}
