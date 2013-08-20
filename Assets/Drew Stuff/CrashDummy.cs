using UnityEngine;
using System.Collections;

public class CrashDummy : MonoBehaviour
{
    public bool hasCrashed = false;
    
    void OnTriggerEnter(Collider col)
    {
        if (col.transform.name == "LevelTerrain")
        {
            hasCrashed = true;
        }
    }
}
