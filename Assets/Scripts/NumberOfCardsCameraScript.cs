using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using System.Collections;

public class NumberOfCardsCameraScript : MonoBehaviour
{
    private SoundManagerScript soundManagerScript;
    private bool quantitySelected = false;
    private float timeToExecuteButton = 0f;

	// Use this for initialization
	void Start ()
    {
        soundManagerScript = GameObject.Find("SoundManager").GetComponent<SoundManagerScript>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if (!quantitySelected)
	    {
	        var interactionPoint = Vector2.zero;
	        if (Input.GetMouseButtonUp(0))
	        {
	            interactionPoint = Input.mousePosition;
	        }
	        else if (Input.touches.Any())
	        {
	            interactionPoint = Input.touches.First().position;
	        }
	        if (interactionPoint != Vector2.zero)
	        {
	            var ray = camera.ScreenPointToRay(interactionPoint);
	            var allHits = Physics.RaycastAll(ray);
	            foreach (var buttonHit in allHits.Where(c => c.transform.tag.Equals("Button")))
	            {
	                if (buttonHit.transform.name.Equals("EightCardButton"))
	                {
	                    Utility.NumberOfCards = NumCards.Eight;
                        quantitySelected = true;
                        GameObject.Find("SixCardButton").renderer.enabled = false;
	                }
	                else if (buttonHit.transform.name.Equals("SixCardButton"))
	                {
	                    Utility.NumberOfCards = NumCards.Six;
	                    quantitySelected = true;
	                    GameObject.Find("EightCardButton").renderer.enabled = false;
	                }
	            }

	            soundManagerScript.PlaySound(!quantitySelected
	                ? SoundManagerScript.Sounds.CantTouch
	                : SoundManagerScript.Sounds.Match);
	        }
	        if (quantitySelected)
	        {
	            timeToExecuteButton = Time.time + 1f;
	        }
	    }
	    else
	    {
	        if (Time.time > timeToExecuteButton)
            {
                Application.LoadLevel("MainScene");
	        }
	    }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
