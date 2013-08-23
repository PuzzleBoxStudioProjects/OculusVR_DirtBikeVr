using UnityEngine;
using System.Collections;

public class LapCounter : MonoBehaviour
{
    public static int curLap = 1;

    void OnTriggerExit(Collider col)
    {
        if (col.transform.name == "lap counter")
        {
            curLap++;
        }
    }
}
