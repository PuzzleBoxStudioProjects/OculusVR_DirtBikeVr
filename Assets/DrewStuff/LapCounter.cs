using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LapCounter : MonoBehaviour
{
    public static int lapCount = 2;
    public static bool isRaceFinished = false;

    public List<GameObject> racers;

    public int rank = 0;

    void OnGUI()
    {
        if (isRaceFinished)
        {
            for (int i = 0; i < racers.Count; i++)
            {
                GUI.Label(new Rect(10, 40 * i, 500, 500), "Racer: " + racers[i].name + " placed " + (i + 1));
            }
        }
    }

    public void RecordRank(GameObject racer)
    {
        racers.Add(racer);
        rank++;
        print(racer.name + " is " + rank + " place.");
    }
}
