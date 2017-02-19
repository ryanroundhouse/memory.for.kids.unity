using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public GameObject CardManager;

	// Use this for initialization
	void Start ()
	{
	    var musicManager = GameObject.Find("MusicManager").GetComponent<MusicManagerScript>();
        musicManager.PlayMusic(Music.Gameplay);
	}
	
	// Update is called once per frame
	void Update ()
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
        if(interactionPoint != Vector2.zero)
        {
            var ray = camera.ScreenPointToRay(interactionPoint);
            var allHits = Physics.RaycastAll(ray);
            foreach (var firstCardHit in allHits.Where(c => c.transform.tag.Equals("Card")))
            {
                CardManager.SendMessage("Touched", firstCardHit.transform.gameObject, SendMessageOptions.RequireReceiver);
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
	}
}
