using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowing : SteeringBehaviour
{
    public int currentNode = 0;
    public Vector3[] path;
    public GameObject[] pathObjects;
    public float arriveRadius = 0;
    public float pathRadius = 1.0f;
    public int pathDirection = 1;       // public just so we can watch it

    public bool followPath = false;

    public Material currentNodeMat;
    public Material otherNodesMat;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            gameObject.AddComponent<Rigidbody>();
            rb = GetComponent<Rigidbody>();
        }

        GameObject[] paths = GameObject.FindGameObjectsWithTag("Node");
        for (int index = 0; index != paths.Length; index++)
        {
            pathObjects[index] = paths[index];
        }

        UpdatePath();

    }

    public override Vector3 CalculateSteeringForce()
    {
        if (followPath)
        {
            Vector3 targetPos = path[currentNode];
            Debug.DrawLine(path[currentNode], transform.position);
            if (path.Length > 1)
            {
                if (Vector3.Distance(transform.position, targetPos) < pathRadius)
                {
                    //currentNode += 1;                     // stop at final or loop around nodes
                    //currentNode += pathDirection;           // return along the route
                    //                                        //if (currentNode >= path.Length)
                    //if (currentNode >= path.Length || currentNode < 0)
                    //{
                    //    //currentNode =  path.Length - 1;   // stop at final node
                    //    //currentNode = 0;                  // loop around nodes
                    //    pathDirection *= -1;
                    //    currentNode += pathDirection;
                    //}
                    currentNode += pathDirection;
                    if (currentNode >= path.Length)
                    {
                        currentNode = 0;
                    }
                    if (currentNode < 0)
                    {
                        currentNode = path.Length - 1;
                    }
                    for (int index = 0; index != pathObjects.Length; index++)
                    {
                        pathObjects[index].GetComponent<MeshRenderer>().material = otherNodesMat;
                    }
                    pathObjects[currentNode].GetComponent<MeshRenderer>().material = currentNodeMat;
                }
            }

            
            // Calculate the direction
            // Seek uses target position - current position
            Vector3 direction = targetPos - transform.position;
            Vector3 desiredVelocity = direction.normalized * maxSpeed;

            // Approach Behaviour
            // Calculate the distance
            float distance = direction.magnitude;

            // Apply approach behaviour
            if (distance < arriveRadius)
            {
                desiredVelocity *= (distance / arriveRadius);
            }

            Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - rb.velocity, maxForce);

            steer = steer / mass;

            return steer;
        }
        else
        {
            rb.velocity = Vector3.zero;
            return Vector3.zero;
        }
        
    }

    public void ReversePath()
    {
        //int start = 0;
        //int end = pathObjects.Length - 1;
        //while (start < end)
        //{
        //    GameObject temp = pathObjects[start];
        //    pathObjects[start] = pathObjects[end];
        //    pathObjects[end] = temp;
        //    start += 1;
        //    end -= 1;
        //}
        for (int index = 0; index != pathObjects.Length; index++)
        {
            pathObjects[index].GetComponent<MeshRenderer>().material = otherNodesMat;
        }
        //currentNode = pathObjects.Length - 1 - currentNode;
        pathDirection *= -1;
        currentNode += pathDirection;
        if (currentNode >= path.Length)
        {
            currentNode = 0;
        }
        if (currentNode < 0)
        {
            currentNode = path.Length - 1;
        }
        //UpdatePath();
        pathObjects[currentNode].GetComponent<MeshRenderer>().material = currentNodeMat;
    }

    public void UpdatePath()
    {
        for (int index = 0; index != path.Length; index++)
        {
            path[index] = pathObjects[index].transform.position;
        }
        pathObjects[currentNode].GetComponent<MeshRenderer>().material = currentNodeMat;
    }

}
