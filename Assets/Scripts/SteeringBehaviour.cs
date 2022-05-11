using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaviour : MonoBehaviour
{
    public Transform target;
    public float weight = 1.0f;
    public float mass = 1.0f;

    public float maxSpeed = 1.0f;      // how fast does the agent move to/away from target
    public float maxForce = 1.0f;      // capps how much steering force is applied
    public float maxTurnSpeed = 1.0f;  // how fast does the agent turn

    public float startingRotation = 0.0f;

    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            gameObject.AddComponent<Rigidbody>();
            rb = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 steer = CalculateSteeringForce();
        ApplySteer(steer);
        LookWhereYouAreGoing();
    }


    public virtual Vector3 CalculateSteeringForce()
    {
        return Vector3.zero;
    }

    public void ApplySteer(Vector3 accel)
    {
        rb.velocity += accel * Time.deltaTime * weight;

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    public void LookWhereYouAreGoing()
    {
        Vector3 direction = rb.velocity;

        LookAtDirection(direction);
    }

    public void LookAtDirection(Vector3 direction)
    {
        direction.Normalize();
        // If we have a non zero direction then look towards that direction, otherise do nothing
        if (direction.sqrMagnitude > 0.001f)
        {
            // Determine how the agent should rotate to face the direction on the Y axis
            // Atan2 returns the angle in radians whose tan is direction.z / direction.x
            float rotY = -1 * Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            // Determine the amount to turn from it's current rotation to it's desired rotation based on masx turn speed
            float rotationAmount = Mathf.LerpAngle(transform.rotation.eulerAngles.y, rotY + startingRotation, Time.deltaTime * maxTurnSpeed);
            // Convert angle to a Quaternion and apply the rotation to the agent.
            transform.rotation = Quaternion.Euler(0, rotationAmount, 0);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
