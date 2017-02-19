using UnityEngine;
using System.Collections;

public class DeleteOnCollisionScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Debug.Log("start");
	
	}

    private void OnTriggerEnter(Collider col)
    {
        Destroy(col.gameObject);
    }
}
