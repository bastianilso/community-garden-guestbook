using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class borderHighlighter : MonoBehaviour {

	public GameObject inputFieldObject;
	private InputField inputField;

	private RectTransform rect;
	private float height;
	private float width;

	private float defaultWidth;
	private float defaultHeight;

	public bool changeWidth;
	public bool changeHeight;

	private bool focusRegistered;
	// Use this for initialization
	void Start () {
		inputField = inputFieldObject.GetComponent<InputField> ();
		rect = gameObject.GetComponent<RectTransform>();
		defaultHeight = rect.sizeDelta.y;
		defaultWidth = rect.sizeDelta.x;
		height = defaultHeight;
		width = defaultWidth;
	}
	
	// Update is called once per frame
	void Update () {
		if (inputField.isFocused && !focusRegistered) {
			if (changeWidth) {
				width = defaultWidth * 2;
			}
			if (changeHeight) {
				height = defaultHeight * 2;
			}

			rect.sizeDelta = new Vector2 (width, height);			
			focusRegistered = true;
		} else if (!inputField.isFocused && focusRegistered) {
			rect.sizeDelta = new Vector2 (defaultWidth, defaultHeight);
			focusRegistered = false;
		}
	}
}
