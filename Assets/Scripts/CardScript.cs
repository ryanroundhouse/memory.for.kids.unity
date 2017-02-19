using System;
using Assets.Scripts;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public CardState State = CardState.FaceDown;
    public int cardIndex = 0;
    public float danceSpeed = 1f;
    public float RotateSpeedFactor = 0.8f;
    public float ResetSpeedFactor = 1f;
        
    private float rotatePercent;
    private float shrinkPercent;
    private float originalScale;
    private float delayTime;
    public bool IsUnderScrutiny
    {
        get
        {
            return State == CardState.FlippedUp || State == CardState.FlippingUp || State == CardState.FlippingDown ||
                   State == CardState.ReadyToBeJudged || State == CardState.Shaking;
        }
    }

    public bool IsInPlay
    {
        get { return State == CardState.FaceDown; }
    }

    // Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
	    switch (State)
	    {
	        case CardState.FlippingUp:
	            RotateCardToFront();
	            break;
	        case CardState.FlippedUp:
	            if (delayTime <= Time.time)
	            {
	                State = CardState.ReadyToBeJudged;
	            }
	            break;
	        case CardState.FlippingDown:
	            ResetCard();
	            break;
	        case CardState.RemoveFromPlay:
	            ShrinkCard();
	            break;
	        case CardState.SpinAndDrop:
	            SpinAndDropCard();
	            break;
	    }
	}

    private void SpinAndDropCard()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - 1.5f * Time.deltaTime, transform.position.z);
        transform.Rotate(0, rotatePercent, 0);
    }

    public void SetSpinAndDrop()
    {
        State = CardState.SpinAndDrop;
        rotatePercent = (float)Utility.Random.NextDouble() * 4f;
    }

    private void ShrinkCard()
    {
        if (shrinkPercent > 0)
        {
            shrinkPercent -= 0.5f*Time.deltaTime;
        }
        if (shrinkPercent <= 0)
        {
            shrinkPercent = 0;
            State = CardState.RemovedFromPlay;
        }
        transform.localScale = new Vector3(originalScale * shrinkPercent, originalScale * shrinkPercent, originalScale);
        transform.Rotate(0f, 0f, 3f * shrinkPercent);
    }

    public void SetCardBack(Material material)
    {
        gameObject.GetComponentInParent<Transform>().GetChild(1).renderer.material = material;
    }

    void RemoveFromPlay()
    {
        State = CardState.RemoveFromPlay;
        shrinkPercent = 1f;
        originalScale = transform.localScale.x;
    }

    void Touched()
    {
        if (State == CardState.FaceDown)
        {
            State = CardState.FlippingUp;
            rotatePercent = 0;
        }
    }

    void Reset()
    {
        rotatePercent = 0;
        State = CardState.FlippingDown;
    }

    void RotateCardToFront()
    {
        if (rotatePercent <= 1)
        {
            rotatePercent += RotateSpeedFactor * Time.deltaTime;
        }
        transform.eulerAngles = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 180, 0), rotatePercent);
        if (rotatePercent >= 1)
        {
            rotatePercent = 0;
            State = CardState.FlippedUp;
            delayTime = Time.time + 1;
        }
    }

    void ResetCard()
    {
        if(rotatePercent <= 1)
        {
            rotatePercent += 1.3f * ResetSpeedFactor * Time.deltaTime;
        }
        transform.eulerAngles = Vector3.Lerp(new Vector3(0, 180, 0), new Vector3(0, 0, 0), rotatePercent);
        if(rotatePercent >= 1)
        {
            rotatePercent = 0;
            State = CardState.FaceDown;
        }
    }

    public enum CardState
    {
        FaceDown,
        FlippingUp,
        FlippedUp,
        ReadyToBeJudged,
        Shaking,
        SpinAndDrop,
        FlippingDown,
        RemoveFromPlay,
        RemovedFromPlay
    }
}
