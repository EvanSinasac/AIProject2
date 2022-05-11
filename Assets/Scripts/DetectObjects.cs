using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObjects : MonoBehaviour
{
    // detects all objects that enter the guard's radar
    HashSet<Rigidbody> _obstacles = new HashSet<Rigidbody>();

    public HashSet<Rigidbody> obstacles
    {
        get
        {
            // Removes any rigidbodies that have been destroyed
            _obstacles.RemoveWhere(IsNull);
            return _obstacles;
        }
    }

    static bool IsNull(Rigidbody r)
    {
        return (r == null || r.Equals(null));
    }

    void AddToRadar(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            _obstacles.Add(rb);
            //Debug.Log("new target added");
        }
    }

    void RemoveFromRadar(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            _obstacles.Remove(rb);
            //Debug.Log("target removed");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            AddToRadar(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RemoveFromRadar(other);
    }
}
