using UnityEngine;
using System.Collections;

public class CheckPoints : MonoBehaviour
{
    public Transform currentCheckpoint;

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Checkpoint")
        {
            currentCheckpoint = col.transform;
        }
    }
}
