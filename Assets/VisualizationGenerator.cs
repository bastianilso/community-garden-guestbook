using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizationGenerator : MonoBehaviour {

	// Public DATA Structure variable, sorted descending by date
	public LoadSaveData dataManager;

	public CameraController camControl;

	public RectTransform YPosObject;
	private float startYpos;
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
	public GameObject renderVis;
	private RectTransform renderVisRect;
	private Text currentDateObj;


	// Use this for initialization
	void Start () {
		startYpos = YPosObject.anchoredPosition.y;
		currentYpos = YPosObject.anchoredPosition.y;
		smallGap = 40f;
		largeGap = 120f;
		currentDate = "";
		thisRect = gameObject.GetComponent<RectTransform> ();
		renderVisRect = renderVis.GetComponent<RectTransform>();
		GenerateVisualization ();
	}
		
	public void GenerateVisualization() {
		// RESET existing vis?
		GuestEntryData guestData;// = new GuestEntryData();
		//guestData.init ();
		//dataManager.SaveData (guestData);
		guestData = dataManager.LoadData();

		for (int i = 0; i < guestData.guestID.Count; i++) {			
			Texture2D snap = null;
			if (guestData.guestAvatar [i] != null) {
				snap = new Texture2D (camControl.frontCam.width, camControl.frontCam.height);
				snap.LoadImage (guestData.guestAvatar [i]);
			}
			AddEntry (snap, guestData.text[i], guestData.date[i], guestData.time[i]);
		}
	}

	public void AddEntry(Texture2D snap, string text, string date, string time) {

		// expand the Visualization rect to make room for more visualizations
		float thisRectSizeDeltaX = thisRect.sizeDelta.x;
		float thisRectSizeDeltaY = thisRect.sizeDelta.y;
		thisRect.sizeDelta = new Vector2 (thisRectSizeDeltaX, thisRectSizeDeltaY+240f);

		// if the date doesn't match, Create a date header
		if (currentDate != date) {
			currentDateObj = Instantiate (dateTextTemplate, renderVis.transform);
			currentDateObj.GetComponent<Text> ().text = date;
		}

		// update the date header position to currentYPos
		float currentDateObjanchorPosX = currentDateObj.rectTransform.anchoredPosition.x;
		currentDateObj.rectTransform.anchoredPosition = new Vector2 (currentDateObjanchorPosX, currentYpos);

		currentYpos -= smallGap;
		currentYpos -= largeGap;

		// Create time text
		Text currentTimeText = Instantiate (timeTextTemplate, renderVis.transform);
		currentTimeText.GetComponent<Text> ().text = time;
		float curTimeTextRectPosX = currentTimeText.rectTransform.anchoredPosition.x;
		currentTimeText.rectTransform.anchoredPosition = new Vector2 (curTimeTextRectPosX, currentYpos);

		// Create horizontal line
		RawImage currentLineHorizontal = Instantiate (lineHorizontalTemplate, renderVis.transform);
		float curLineHorRectPosX = currentLineHorizontal.rectTransform.anchoredPosition.x;
		currentLineHorizontal.rectTransform.anchoredPosition = new Vector2(curLineHorRectPosX, currentYpos);

		RawImage currentImage = Instantiate (imageTemplate, renderVis.transform);
		float curImgRectPosX = currentImage.rectTransform.anchoredPosition.x;
		currentImage.rectTransform.anchoredPosition = new Vector2 (curImgRectPosX, currentYpos);

		// Create visitor image
		if (snap != null) {
			currentImage.texture = snap;
		}

		// Create visitor text
		Text visitorText = Instantiate (visitorTextTemplate, renderVis.transform);
		visitorText.GetComponent<Text> ().text = text;
		float visitorTextRectPosX = visitorText.rectTransform.anchoredPosition.x;
		visitorText.rectTransform.anchoredPosition = new Vector2 (visitorTextRectPosX, currentYpos);

		currentYpos -= largeGap;

		if (currentDate != date) {
			// instantiate a new timeline line
			// set the timeline line as current timeline line
			// Create vertical line
			currentLineVertical = Instantiate (lineVerticalTemplate, renderVis.transform);
			float curLineVerSizeDeltaX = currentLineVertical.rectTransform.sizeDelta.x;
			currentLineVertical.rectTransform.sizeDelta = new Vector2 (curLineVerSizeDeltaX, 240f);
			float curLineVerRectPosX = currentLineVertical.rectTransform.anchoredPosition.x;
			currentLineVertical.rectTransform.anchoredPosition = new Vector2 (curLineVerRectPosX, currentYpos);
		} else {
			// if it's the same date, simply grow the existing timeline line
			float curLineVerSizeDeltaX = currentLineVertical.rectTransform.sizeDelta.x;
			float curLineVerSizeDeltaY = currentLineVertical.rectTransform.sizeDelta.y;
			currentLineVertical.rectTransform.sizeDelta = new Vector2 (curLineVerSizeDeltaX, curLineVerSizeDeltaY + 320f);
		}

		currentYpos = startYpos;

		// move RenderedVis downwards
		float renderVisPosX = renderVisRect.anchoredPosition.x;
		float renderVisPosY = renderVisRect.anchoredPosition.y;
		renderVisRect.anchoredPosition = new Vector2(renderVisPosX, renderVisPosY-320f);
		float renderVisSizeDeltaX = renderVisRect.sizeDelta.x;
		float renderVisSizeDeltaY = renderVisRect.sizeDelta.y;
		renderVisRect.sizeDelta = new Vector2 (renderVisSizeDeltaX, renderVisSizeDeltaY+320f);
		/*currentYpos = YPosObject.anchoredPosition.y;
		currentYpos += largeGap;
		currentYpos += smallGap;
		currentYpos += largeGap;
		currentYpos += largeGap;
		currentYpos += largeGap;
		currentYpos += smallGap;*/

		// update date
		currentDate = date;

	}
		
	// Update is called once per frame
	void Update () {
		
	}
}
