using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude < 0.3)
        {
            animator.SetFloat("Movement", 0);
        }
        else
        {
            animator.SetFloat("Movement", 1);
        }
    }
}
