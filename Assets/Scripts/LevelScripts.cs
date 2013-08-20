using UnityEngine;
using System.Collections.Generic;

public class LevelScripts : MonoBehaviour {
	
	public static bool canRace = false;

	public AudioClip[] clipHolder;
	public GameObject[] lightHolder;
	public int currentPosition;
	public int totalLaps;
	public int lapsLeft = 0;
	public int currentLap = 0;
	
	private float timeLength = 3.0f;
	public float currentTime;
	public bool playNext = false;
	
	public int clipCount = 0;
	void Awake()
	{
	  
	}
	
	
	// Use this for initialization
	void Start () 
	{
	  timeLength += Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
	 
		if(Time.time > timeLength)
		{			
			if(clipCount < 3)
			{
				timeLength = Time.time + clipHolder[clipCount].length;
				audio.PlayOneShot(clipHolder[clipCount]);
				clipCount++;
			}
				
			else if(clipCount == 3 )
			{
				if(currentLap == totalLaps && currentPosition == 1)
				{
					audio.PlayOneShot(clipHolder[clipCount]);
					timeLength += Time.time + 10;
					clipCount  = 9;
				}
				else
				{
					audio.PlayOneShot(clipHolder[clipCount+1]);
					timeLength = Time.time +10;
					clipCount = 9;
				}
			}
			if (clipCount == 9)
			{
				// go to the win screen;
			}
		}
	
	}
}