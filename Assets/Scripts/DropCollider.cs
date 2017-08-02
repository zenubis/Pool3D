using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCollider : MonoBehaviour {
    private GameManager gm;
    
	// Use this for initialization
	void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
    private void OnTriggerEnter(Collider other)
    {
        gm.DropBall(other.gameObject);
    }
}
