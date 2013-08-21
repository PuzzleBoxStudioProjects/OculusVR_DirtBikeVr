using UnityEngine;
using System.Collections;

public class LapCounter : MonoBehaviour
{
    public int curLap = 1;
    public bool isNextLap = false;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.name == "lap counter" && !isNextLap)
        {
            curLap++;
            isNextLap = true;
            
        }
    }
}
