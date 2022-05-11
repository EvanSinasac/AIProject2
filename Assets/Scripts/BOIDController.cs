using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOIDController : MonoBehaviour
{
    public Formation Formation;
    public Flocking Flocking;

    public float flockingWeightChange = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Formation = this.gameObject.GetComponent<Formation>();
        Flocking = this.gameObject.GetComponent<Flocking>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("z"))
        {   // increase cohesion weight
            Flocking.cohWeight += flockingWeightChange;
            Flocking.sepWeight -= flockingWeightChange / 2.0f;
            Flocking.aliWeight -= flockingWeightChange / 2.0f;

            Debug.Log("New values are:\n Cohesion: " + Flocking.cohWeight + "\nSeparation: " + Flocking.sepWeight + "\nAlignment: " + Flocking.aliWeight);
        }
        else if (Input.GetKeyDown("x"))
        {   // decrease cohesion weight
            Flocking.cohWeight -= flockingWeightChange;
            Flocking.sepWeight += flockingWeightChange / 2.0f;
            Flocking.aliWeight += flockingWeightChange / 2.0f;

            Debug.Log("New values are:\n Cohesion: " + Flocking.cohWeight + "\nSeparation: " + Flocking.sepWeight + "\nAlignment: " + Flocking.aliWeight);
        }

        if (Input.GetKeyDown("c"))
        {   // increase separation weight
            Flocking.cohWeight -= flockingWeightChange / 2.0f;
            Flocking.sepWeight += flockingWeightChange;
            Flocking.aliWeight -= flockingWeightChange / 2.0f;
            Debug.Log("New values are:\n Cohesion: " + Flocking.cohWeight + "\nSeparation: " + Flocking.sepWeight + "\nAlignment: " + Flocking.aliWeight);
        }
        else if (Input.GetKeyDown("v"))
        {   // decrease separation weight
            Flocking.cohWeight += flockingWeightChange / 2.0f;
            Flocking.sepWeight -= flockingWeightChange;
            Flocking.aliWeight += flockingWeightChange / 2.0f;
            Debug.Log("New values are:\n Cohesion: " + Flocking.cohWeight + "\nSeparation: " + Flocking.sepWeight + "\nAlignment: " + Flocking.aliWeight);
        }

        if (Input.GetKeyDown("b"))
        {   // increase alignment weight
            Flocking.cohWeight -= flockingWeightChange / 2.0f;
            Flocking.sepWeight -= flockingWeightChange / 2.0f;
            Flocking.aliWeight += flockingWeightChange;
            Debug.Log("New values are:\n Cohesion: " + Flocking.cohWeight + "\nSeparation: " + Flocking.sepWeight + "\nAlignment: " + Flocking.aliWeight);
        }
        else if (Input.GetKeyDown("n"))
        {   // decrease alignment weight
            Flocking.cohWeight += flockingWeightChange / 2.0f;
            Flocking.sepWeight += flockingWeightChange / 2.0f;
            Flocking.aliWeight -= flockingWeightChange;
            Debug.Log("New values are:\n Cohesion: " + Flocking.cohWeight + "\nSeparation: " + Flocking.sepWeight + "\nAlignment: " + Flocking.aliWeight);
        }

        if (Flocking.cohWeight < 0.0f)
        {
            Flocking.cohWeight = 0.0f;
        }
        if (Flocking.sepWeight < 0.0f)
        {
            Flocking.sepWeight = 0.0f;
        }
        if (Flocking.aliWeight < 0.0f)
        {
            Flocking.aliWeight = 0.0f;
        }

        if (Flocking.cohWeight > 1.0f)
        {
            Flocking.cohWeight = 1.0f;
        }
        if (Flocking.sepWeight > 1.0f)
        {
            Flocking.sepWeight = 1.0f;
        }
        if (Flocking.aliWeight > 1.0f)
        {
            Flocking.aliWeight = 1.0f;
        }

    }

    public void SetFlocking ()
    {
        Formation.formation = false;
        Flocking.flocking = true;
    }

    public void SetFormation ()
    {
        Formation.formation = true;
        Flocking.flocking = false;
    }

}
