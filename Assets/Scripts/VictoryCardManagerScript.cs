using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using System.Collections;

public class VictoryCardManagerScript : MonoBehaviour
{
    private List<GameObject> cards = new List<GameObject>();
    private float spawnCardDelay = 0f;

    public List<Material> AvailableMaterials;

	// Use this for initialization
	void Start () 
    {
	    for (int n = 0; n < 5; n++)
	    {
	        // 6 to -2
            SpawnCard((float)Utility.Random.NextDouble() * 8 - 2);
	    }
    }

    private void SpawnCard(float y)
    {
        var position = new Vector3((float)Utility.Random.NextDouble() * 16 - 8f, y, Utility.Random.Next(1, 10));
        var card = Instantiate(Resources.Load("CardPrefab"), position, new Quaternion())
            as GameObject;
        var scale = (float)Utility.Random.NextDouble() + 0.5f;
        card.transform.localScale = new Vector3(scale, scale, scale);
        var cardScript = card.GetComponent<CardScript>();
        cardScript.SetSpinAndDrop();
        cardScript.SetCardBack(AvailableMaterials[Utility.Random.Next(0, AvailableMaterials.Count)]);
        cards.Add(card);
        spawnCardDelay = Time.fixedTime + 2f * (float)Utility.Random.NextDouble();
    }
	
	// Update is called once per frame
	void Update () 
    {
	    if (Time.fixedTime > spawnCardDelay)
	    {
            SpawnCard(6);
	    }
    }
}
