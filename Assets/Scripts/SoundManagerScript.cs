using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class SoundManagerScript : MonoBehaviour
{
    private List<AudioSource> sources;
	// Use this for initialization
	void Start () 
    {
        sources = GetComponents<AudioSource>().ToList();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void PlaySound(Sounds sound)
    {
        sources[(int)sound].Play();
    }

    public enum Sounds
    {
        CantTouch = 0,
        Match = 1,
        ResetCard = 2,
        Touch = 3
    }
}
