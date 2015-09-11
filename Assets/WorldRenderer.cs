using UnityEngine;
using System.Collections;

public class WorldRenderer : MonoBehaviour {

	private Builder builder = null;



	void Awake() {
		this.builder = this.GetComponent<Builder>();
	}

	void Start() {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
