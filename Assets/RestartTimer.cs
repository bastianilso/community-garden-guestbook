using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestartTimer : MonoBehaviour {

	public float timeLeft;
	private float currentTimeLeft;
	private bool shouldCount;
	private int lastNumber;
	public Text countdown;

	// Use this for initialization
	void Start () {
		shouldCount = false;
		currentTimeLeft = timeLeft;
	}

	public void ActivateCounter() {
		shouldCount = true;
	}

	// Update is called once per frame
	void Update () {
		if (shouldCount) {
			currentTimeLeft -= Time.deltaTime;
			//Debug.Log (currentTimeLeft);
			if (currentTimeLeft < 0) {
				SceneManager.LoadScene( SceneManager.GetActiveScene().name );
			}
				
		}
		if (Input.touchCount > 0) {
			currentTimeLeft = timeLeft;
		}
		
	}
}
