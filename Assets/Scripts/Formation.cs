using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation : SteeringBehaviour
{
    Vector3 targetPos;

    public float arriveRadius = 0;
    public float targetRadius = 0;

    public bool formation = true;

    public override Vector3 CalculateSteeringForce()
    {
        if (formation)
        {
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

            if (distance < targetRadius)
            {
                rb.velocity = Vector3.zero;
                return Vector3.zero;
            }

            Vector3 steer = Vector3.ClampMagnitude(desiredVelocity - rb.velocity, maxForce);

            steer = steer / mass;

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

}
