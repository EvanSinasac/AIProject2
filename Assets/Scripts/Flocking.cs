using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocking : SteeringBehaviour
{
    public float sepRadius = 1.0f;
    public float aliRadius = 1.0f;
    public float cohRadius = 1.0f;
    public float sepWeight = 1.0f;
    public float aliWeight = 1.0f;
    public float cohWeight = 1.0f;

    public bool flocking = false;
    public bool followPath = false;

    Vector3 targetPos;

    private Vector3 Separate()
    {
        Vector3 totalFlee = Vector3.zero;
        int neighbourhoodCount = 0;
        foreach (Transform i in transform.parent)
        {
            float distance = Vector3.Distance(rb.position, i.position);

            if (distance < sepRadius && distance > 0.0f)    // the boid is inside our neighbourhood
            {
                Vector3 fleeVector = rb.position - i.position;
                fleeVector.Normalize();
                fleeVector /= distance;
                totalFlee += fleeVector;
                neighbourhoodCount++;
            }
        }

        Vector3 sepForce = Vector3.zero;
        if (neighbourhoodCount > 0)
        {
            Vector3 desiredVelocity = totalFlee / neighbourhoodCount;
            desiredVelocity.Normalize();
            desiredVelocity *= maxSpeed;
            sepForce = Vector3.ClampMagnitude(desiredVelocity - rb.velocity, maxForce);
            sepForce = sepForce / mass;
        }

        return sepForce;
    }

    private Vector3 Align()
    {
        Vector3 totalVelocity = Vector3.zero;
        int neighbourhoodCount = 0;
        foreach (Transform i in transform.parent)
        {
            float distance = Vector3.Distance(rb.position, i.position);
            if (distance < aliRadius && distance > 0.0f)
            {
                totalVelocity += i.GetComponent<Rigidbody>().velocity;
                neighbourhoodCount++;
            }
        }

        Vector3 aliForce = Vector3.zero;
        if (neighbourhoodCount > 0)
        {
            Vector3 desiredVelocity = totalVelocity / neighbourhoodCount;
            desiredVelocity.Normalize();
            desiredVelocity *= maxSpeed;
            aliForce = Vector3.ClampMagnitude(desiredVelocity - rb.velocity, maxForce);
            aliForce = aliForce / mass;
        }

        return aliForce;
    }

    private Vector3 Cohesion()
    {
        Vector3 totalPosition = Vector3.zero;
        int neighbourhoodCount = 0;
        foreach (Transform i in transform.parent)
        {
            float distance = Vector3.Distance(rb.position, i.position);
            if (distance < cohRadius && distance > 0.0f)
            {
                totalPosition += i.position;
                neighbourhoodCount++;
            }
        }

        Vector3 cohForce = Vector3.zero;
        if (neighbourhoodCount > 0)
        {
            Vector3 targetPos = totalPosition / neighbourhoodCount;
            Vector3 direction = targetPos - transform.position;
            Vector3 desiredVelocity = direction.normalized * maxSpeed;
            cohForce = Vector3.ClampMagnitude(desiredVelocity - rb.velocity, maxForce);
            cohForce = cohForce / mass;
        }

        return cohForce;
    }

    public override Vector3 CalculateSteeringForce()
    {
        if (flocking)
        {
            Vector3 steer = Vector3.zero;

            // calculate flocking behaviour
            steer += Separate() * sepWeight;
            steer += Align() * aliWeight;
            steer += Cohesion() * cohWeight;

            if (followPath)
            {
                steer += Seek();
            }

            return steer;
        }
        else
        {
            return Vector3.zero;
        }
        
    }

    public void SetTargetPosition(Vector3 pos)
    {
        targetPos = pos;
    }

    private Vector3 Seek ()
    {
        // Calculate the direction
        // Seek uses target position - current position
        Vector3 direction = targetPos - transform.position;
        Vector3 desiredVelocity = direction.normalized * maxSpeed;

        // Approach Behaviour
        // Calculate the distance
        float distance = direction.magnitude;

        // If we are in the stopping radius then stop
        //if (distance < targetRadius)
        //{
        //    rb.velocity = Vector3.zero;
        //    return Vector3.zero;
        //}

        //// Apply approach behaviour
        //if (distance < arriveRadius)
        //{
        //    desiredVelocity *= (distance / arriveRadius);
        //}

        Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - rb.velocity, maxForce);

        steer = steer / mass;

        return steer;
    }
}

