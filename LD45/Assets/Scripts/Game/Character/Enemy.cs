using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {
	void OnTriggerEnter(Collider other) {
        Debug.Log(other.name);
	}

	void OnTriggerStay2D(Collider2D collision) {
        //if (collision.tag == "Player") 
			//MoveTo(collision.transform.position);
    }
}
