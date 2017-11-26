using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hideOnFocus : MonoBehaviour {

	public InputField inputField;
	private Text placeholderText;
	private bool focusRegistered;
	private string text;

	// Use this for initialization
	void Start () {
		placeholderText = gameObject.GetComponent<Text> ();
		text = placeholderText.text;
	}
	
	// Update is called once per frame
	void Update () {
		if (inputField.isFocused && !focusRegistered) {
			placeholderText.text = "";
			focusRegistered = true;
		} else if (!inputField.isFocused && focusRegistered) {
			placeholderText.text = text;
			focusRegistered = false;
		}


	}
}
