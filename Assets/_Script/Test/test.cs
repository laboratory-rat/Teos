using UnityEngine;
using System.Collections;
using Spider;

public class test : MonoBehaviour {

	void Update() {
		if (Input.GetKeyDown (KeyCode.E)) {
			Spider.Spider.StopApp ();
			print("Ok");
		}
	}
}
