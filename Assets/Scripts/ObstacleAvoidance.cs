using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : SteeringBehaviour
{
    public DetectObjects radar;
    public float distanceBetween = 0.0f;
    public float projectionRange = 1.0f;

    public Vector3 FindTheNearestThreat(ICollection<Rigidbody> obstacles)
    {
        Vector3 threatPos = Vector3.zero;

        float shortestTimeToCollide = float.PositiveInfinity;

        float guardRadius = 0.0f;
        if (GetComponent<SphereCollider>() != null)
        {
            guardRadius = GetComponent<SphereCollider>().radius;
        }
        else if (GetComponent<CapsuleCollider>() != null)
        {
            guardRadius = GetComponent<CapsuleCollider>().radius;
        }

        // We are going to go through all the obstacles to find if any area threat that will collide with our vehicle
        foreach (Rigidbody r in obstacles)
        {
            // Calculate the time to collide
            Vector3 relativePos = rb.position - r.position;
            Vector3 relativeVel = rb.velocity - r.velocity;
            float relativeSpeed = relativeVel.magnitude;

            if (relativeSpeed == 0)
            {
                continue;   // No collision
            }

            float timeToCollide = -1 * Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);


            float obstacleRadius = 0.0f;
            if (r.GetComponent<SphereCollider>() != null)
            {
                obstacleRadius = r.GetComponent<SphereCollider>().radius;
            }
            else if (r.GetComponent<CapsuleCollider>() != null)
            {
                obstacleRadius = r.GetComponent<CapsuleCollider>().radius;
            }


            // Check if any collision will occur
            Vector3 separation = relativePos + relativeVel * timeToCollide;
            // Comparing the minimum separation to the radius of the threat to the radius of the guard
            //if (separation.magnitude > r.GetComponent<SphereCollider>().radius + GetComponent<SphereCollider>().radius + distanceBetween)
            if (separation.magnitude > obstacleRadius + guardRadius + distanceBetween)
            {

                continue;       // Enough of a gap between them, no collision
            }

            // Check if this obstacle has the shortest time to collision
            if (timeToCollide > 0 && timeToCollide < shortestTimeToCollide)
            {
                shortestTimeToCollide = timeToCollide;
                threatPos = r.position;
            }
        }


        return threatPos;
    }

    public override Vector3 CalculateSteeringForce()
    {
        Vector3 projectionPos = rb.position + rb.velocity.normalized * projectionRange;
        Debug.DrawLine(projectionPos, rb.position);
        Vector3 steer = Vector3.zero;
        Vector3 threatPos = FindTheNearestThreat(radar.obstacles);

        if (threatPos != Vector3.zero)
        {
            // threat is detected, calculate a steering force to avoid collision
            Vector3 direction = projectionPos - threatPos;      // was rb.position - threatPos
            Vector3 desiredVelocity = direction.normalized * maxSpeed;

            steer = Vector3.ClampMagnitude(desiredVelocity - rb.velocity, maxForce);

            steer = steer / mass;
        }

        return steer;
    }
}
