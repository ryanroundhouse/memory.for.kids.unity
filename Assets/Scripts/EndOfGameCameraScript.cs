using System.Linq;
using UnityEngine;
using System.Collections;

public class EndOfGameCameraScript : MonoBehaviour
{
    public GameObject youWinObject;
    public GameObject tapToPlayAgainObject;

    private EndState state = EndState.MovingIn;
    private float youWinYCoord;
    private float tapToPlayAgainYCoord;
    private float pauseDeadline;

	// Use this for initialization
	void Start ()
    {
        var musicManager = GameObject.Find("MusicManager").GetComponent<MusicManagerScript>();
        musicManager.PlayMusic(Music.Victory);
	    youWinYCoord = -8f;
	    tapToPlayAgainYCoord = -8f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }

	    switch (state)
	    {
            case EndState.MovingIn:
            {
                if (youWinObject.transform.position.y < 0)
                {
                    youWinYCoord -= -8f*0.3f*Time.deltaTime;
                    youWinObject.transform.position = new Vector3(youWinObject.transform.position.x, youWinYCoord);
                }
                else
                {
                    pauseDeadline = Time.time + 3;
                    state = EndState.Pausing;
                }
	            break;
	        }
            case EndState.Pausing:
	        {
	            if (Time.time > pauseDeadline)
	            {
	                state = EndState.MovingOut;
	            }
	            break;
	        }
            case EndState.MovingOut:
	        {
	            if (youWinObject.transform.position.y < 8)
	            {
	                youWinYCoord += 8f*0.3f*Time.deltaTime;
	                youWinObject.transform.position = new Vector3(youWinObject.transform.position.x, youWinYCoord);
	            }
                if(tapToPlayAgainObject.transform.position.y < 0)
                {
                    tapToPlayAgainYCoord -= -8f * 0.3f * Time.deltaTime;
                    tapToPlayAgainObject.transform.position = new Vector3(tapToPlayAgainObject.transform.position.x, tapToPlayAgainYCoord);
                }
                if(Input.touches.Any() || Input.GetMouseButton(0))
                {
                    Application.LoadLevel("NumberOfCardSelectionScene");
                }
	            break;
	        }
	    }
	}

    public enum EndState
    {
        MovingIn,
        Pausing,
        MovingOut
    }
}
