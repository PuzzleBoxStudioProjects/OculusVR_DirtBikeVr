using UnityEngine;
using System.Collections.Generic;

public class LevelScripts : MonoBehaviour 
{
	
	public static bool canRace = false;

	public AudioClip[] clipHolder;
	public GameObject[] lightHolder;
	public GameObject[] gateHolder;
	
	public int currentPosition;
	public int totalLaps;
	public int lapsLeft = 0;
	public int currentLap = 0;
	
	private float timeLength = 3.0f;
	private float lightTimer;
	private bool  isGreen = false;
	private bool  playOnce = false;
	private float waitTimer = 0.0f;
	private int lightCount = 0;
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
			if(clipCount == 0)
			{
				timeLength = Time.time + clipHolder[clipCount].length;
				audio.PlayOneShot(clipHolder[clipCount]);
				clipCount++;
				timeLength = Time.time + clipHolder[clipCount].length;
			}
		
		
			else if(clipCount == 1)
			{
				if(!playOnce){
				audio.PlayOneShot(clipHolder[clipCount]);
				playOnce = true;
				}
				if(!IsInvoking("LightActivation"))
				{
					Invoke("LightActivation", waitTimer);
					
				}	
				
		
			}
		}
	}
//		
//	 if(clipCount == 8 )
//		{
//			if(currentLap == totalLaps && currentPosition == 1)
//			{
//				audio.PlayOneShot(clipHolder[clipCount]);
//				timeLength += Time.time + 10;
//				clipCount  = 9;
//			}
//			else
//			{
//					audio.PlayOneShot(clipHolder[clipCount+1]);
//					timeLength = Time.time +10;
//					clipCount = 9;
//			}
//		}
//		if (clipCount == 9)
//		{
//				// go to the win screen;
//		}
//	}
//	
	
	void LightActivation()
	{
			waitTimer = 0.7f;
			if(lightCount <= 3)
			{
			lightHolder[lightCount].SetActive(true);
			
			}
		else
		{
			clipCount++;
		}
			lightCount++;
	}
	
}