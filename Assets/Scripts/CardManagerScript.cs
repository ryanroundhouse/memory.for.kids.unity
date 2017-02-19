using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class CardManagerScript : MonoBehaviour
{
    private List<GameObject> cards;
    public List<Material> AvailableMaterials;
    private SoundManagerScript soundManagerScript;

	// Use this for initialization
	void Start ()
	{
	    soundManagerScript = GameObject.Find("SoundManager").GetComponent<SoundManagerScript>();
	    var numbers = new List<int>();
	    for (var n = 0; n < (int)Utility.NumberOfCards/2; n++)
	    {
	        numbers.Add(n);
            numbers.Add(n);
	    }

        var imagesToUse = new List<int>();
	    for (var n = 0; n < AvailableMaterials.Count; n++)
	    {
	        imagesToUse.Add(n);
	    }
        
	    var shuffle = numbers.OrderBy(a => Guid.NewGuid()).ToArray();
	    var imageShuffle = imagesToUse.OrderBy(a => Guid.NewGuid()).ToArray();

        cards = new List<GameObject>();
        if(Utility.NumberOfCards == NumCards.Eight)
        {
            var position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 1f / 8f, Screen.height * 1f / 4f, 10));
            cards.Add(SpawnCard(position, shuffle[0], AvailableMaterials[imageShuffle[shuffle[0]]]));

            var position2 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 3f / 8f, Screen.height * 1f / 4f, 10));
            cards.Add(SpawnCard(position2, shuffle[1], AvailableMaterials[imageShuffle[shuffle[1]]]));

            var position3 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 5f / 8f, Screen.height * 1f / 4f, 10));
            cards.Add(SpawnCard(position3, shuffle[2], AvailableMaterials[imageShuffle[shuffle[2]]]));

            var position4 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 7f / 8f, Screen.height * 1f / 4f, 10));
            cards.Add(SpawnCard(position4, shuffle[3], AvailableMaterials[imageShuffle[shuffle[3]]]));

            var position5 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 1f / 8f, Screen.height * 3f / 4f, 10));
            cards.Add(SpawnCard(position5, shuffle[4], AvailableMaterials[imageShuffle[shuffle[4]]]));

            var position6 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 3f / 8f, Screen.height * 3f / 4f, 10));
            cards.Add(SpawnCard(position6, shuffle[5], AvailableMaterials[imageShuffle[shuffle[5]]]));

            var position7 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 5f / 8f, Screen.height * 3f / 4f, 10));
            cards.Add(SpawnCard(position7, shuffle[6], AvailableMaterials[imageShuffle[shuffle[6]]]));

            var position8 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 7f / 8f, Screen.height * 3f / 4f, 10));
            cards.Add(SpawnCard(position8, shuffle[7], AvailableMaterials[imageShuffle[shuffle[7]]]));
	        
	    }
        else if(Utility.NumberOfCards == NumCards.Six)
        {
            var position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 1f / 6f, Screen.height * 1f / 4f, 10));
            cards.Add(SpawnCard(position, shuffle[0], AvailableMaterials[imageShuffle[shuffle[0]]]));

            var position2 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 3f / 6f, Screen.height * 1f / 4f, 10));
            cards.Add(SpawnCard(position2, shuffle[1], AvailableMaterials[imageShuffle[shuffle[1]]]));

            var position3 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 5f / 6f, Screen.height * 1f / 4f, 10));
            cards.Add(SpawnCard(position3, shuffle[2], AvailableMaterials[imageShuffle[shuffle[2]]]));

            var position4 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 1f / 6f, Screen.height * 3f / 4f, 10));
            cards.Add(SpawnCard(position4, shuffle[3], AvailableMaterials[imageShuffle[shuffle[3]]]));

            var position5 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 3f / 6f, Screen.height * 3f / 4f, 10));
            cards.Add(SpawnCard(position5, shuffle[4], AvailableMaterials[imageShuffle[shuffle[4]]]));

            var position6 = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 5f / 6f, Screen.height * 3f / 4f, 10));
            cards.Add(SpawnCard(position6, shuffle[5], AvailableMaterials[imageShuffle[shuffle[5]]]));
        }
	}

    private GameObject SpawnCard(Vector3 position, int index, Material availableMaterial)
    {
        var card = Instantiate(Resources.Load("CardPrefab"), position, new Quaternion())
                        as GameObject;
        var cardScript = card.GetComponent<CardScript>();
        cardScript.cardIndex = index;
        cardScript.SetCardBack(availableMaterial);
        return card;
    }

    void Touched(GameObject card)
    {
        if (cards.Count(x => x.GetComponent<CardScript>().IsUnderScrutiny) < 2 &&
            !card.GetComponent<CardScript>().IsUnderScrutiny && card.GetComponent<CardScript>().IsInPlay)
        {
            soundManagerScript.PlaySound(SoundManagerScript.Sounds.Touch);
            card.SendMessage("Touched", SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            soundManagerScript.PlaySound(SoundManagerScript.Sounds.CantTouch);
        }
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (Time.time > 1)
	    {
	        var flippedCards = cards.Where(x => x.GetComponent<CardScript>().State == CardScript.CardState.ReadyToBeJudged);
	        if (flippedCards.Count() == 2)
	        {
	            if (flippedCards.First().GetComponent<CardScript>().cardIndex ==
	                flippedCards.Last().GetComponent<CardScript>().cardIndex)
	            {
	                foreach (var card in flippedCards)
	                {
	                    card.SendMessage("RemoveFromPlay", SendMessageOptions.DontRequireReceiver);
	                    soundManagerScript.PlaySound(SoundManagerScript.Sounds.Match);
	                }
	            }
	            else
	            {
	                foreach (var card in flippedCards)
	                {
	                    card.SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
	                    soundManagerScript.PlaySound(SoundManagerScript.Sounds.ResetCard);
	                }
	            }
	        }

	        if(cards.All(c => c.GetComponent<CardScript>().State == CardScript.CardState.RemovedFromPlay))
	        {
	            // end of game
                Application.LoadLevel("EndOfGameScene");
            }
        }
	}
}