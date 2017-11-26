using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizationGenerator : MonoBehaviour {

	// Public DATA Structure variable, sorted descending by date
	public LoadSaveData dataManager;

	public CameraController camControl;

	public RectTransform YPosObject;
	private float currentYpos;
	private string currentDate;
	private RawImage currentLineVertical;
	private RectTransform thisRect;

	public RawImage lineVerticalTemplate;
	public RawImage lineHorizontalTemplate;
	public Text dateTextTemplate;
	public Text timeTextTemplate;
	public Text visitorTextTemplate;
	public RawImage imageTemplate;
	public RawImage takenPicture;

	private float smallGap;
	private float largeGap;


	// Use this for initialization
	void Start () {
		currentYpos = YPosObject.anchoredPosition.y;
		smallGap = 85f;
		largeGap = 185f;
		currentDate = "";
		thisRect = gameObject.GetComponent<RectTransform> ();

		GenerateVisualization ();
	}
		
	public void GenerateVisualization() {
		// RESET existing vis?
		GuestEntryData guestData;// = new GuestEntryData();
		//guestData.init ();
		//dataManager.SaveData (guestData);
		guestData = dataManager.LoadData();

		for (int i = guestData.guestID.Count - 1; i >= 0; i--) {			
			Texture2D snap = null;
			if (guestData.guestAvatar [i] != null) {
				snap = new Texture2D (camControl.frontCam.width, camControl.frontCam.height);
				snap.LoadImage (guestData.guestAvatar [i]);
			}
			AddEntry (snap, guestData.text[i], guestData.date[i], guestData.time[i]);
		}
	}

	public void AddEntry(Texture2D snap, string text, string date, string time) {
		currentYpos -= smallGap;

		// expand the rect to make room for more visualizations
		float thisRectSizeDeltaX = thisRect.sizeDelta.x;
		float thisRectSizeDeltaY = thisRect.sizeDelta.y;
		thisRect.sizeDelta = new Vector2 (thisRectSizeDeltaX, thisRectSizeDeltaY+225f);

		// if the date doesn't match, Create a date header
		if (currentDate != date) {
			currentYpos -= smallGap;
			currentDate = date;
			Text currentDateObj = Instantiate (dateTextTemplate, gameObject.transform);
			currentDateObj.GetComponent<Text> ().text = date;
			//currentDateObj.transform.SetParent (gameObject.transform);
			//Debug.Log ("date-y-current:" + currentDateObj.rectTransform.anchoredPosition.y);
			float currentDateObjanchorPosX = currentDateObj.rectTransform.anchoredPosition.x;
			currentDateObj.rectTransform.anchoredPosition = new Vector2 (currentDateObjanchorPosX, currentYpos);
			//Debug.Log ("date-y-new:" + currentDateObj.rectTransform.anchoredPosition.y);
		}

		currentYpos -= largeGap;

		// Create vertical line
		currentLineVertical = Instantiate (lineVerticalTemplate, gameObject.transform);
		float curLineVerSizeDeltaY = currentLineVertical.rectTransform.sizeDelta.y;
		currentLineVertical.rectTransform.sizeDelta = new Vector2 (200f, curLineVerSizeDeltaY);
		float curLineVerRectPosX = currentLineVertical.rectTransform.anchoredPosition.x;
		currentLineVertical.rectTransform.anchoredPosition = new Vector2 (curLineVerRectPosX, currentYpos);

		// Create time text
		Text currentTimeText = Instantiate (timeTextTemplate, gameObject.transform);
		currentTimeText.GetComponent<Text> ().text = time;
		float curTimeTextRectPosX = currentTimeText.rectTransform.anchoredPosition.x;
		currentTimeText.rectTransform.anchoredPosition = new Vector2 (curTimeTextRectPosX, currentYpos);

		// Create horizontal line
		RawImage currentLineHorizontal = Instantiate (lineHorizontalTemplate, gameObject.transform);
		float curLineHorRectPosX = currentLineHorizontal.rectTransform.anchoredPosition.x;
		currentLineHorizontal.rectTransform.anchoredPosition = new Vector2(curLineHorRectPosX, currentYpos);


		RawImage currentImage = Instantiate (imageTemplate, gameObject.transform);
		float curImgRectPosX = currentImage.rectTransform.anchoredPosition.x;
		currentImage.rectTransform.anchoredPosition = new Vector2 (curImgRectPosX, currentYpos);

		// Create visitor image
		if (snap != null) {
			currentImage.texture = snap;
		}

		// Create visitor text
		Text visitorText = Instantiate (visitorTextTemplate, gameObject.transform);
		visitorText.GetComponent<Text> ().text = text;
		float visitorTextRectPosX = visitorText.rectTransform.anchoredPosition.x;
		visitorText.rectTransform.anchoredPosition = new Vector2 (visitorTextRectPosX, currentYpos);

	}
		
	// Update is called once per frame
	void Update () {
		
	}
}
