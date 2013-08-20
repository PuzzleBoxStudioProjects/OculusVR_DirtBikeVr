using UnityEngine;
using System.Collections;

public class CrashDummy : MonoBehaviour
{
    public Transform bike;

    private DrewBikePhysics bikePhysics;

    void Awake()
    {
        bikePhysics = bike.GetComponent<DrewBikePhysics>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.name == "LevelTerrain")
        {
            bikePhysics.hasCrashed = true;
        }
    }
}
