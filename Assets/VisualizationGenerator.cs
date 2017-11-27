using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualizationGenerator : MonoBehaviour {

	// Public DATA Structure variable, sorted descending by date
	public LoadSaveData dataManager;

	public CameraController camControl;

	/*private RectTransform YPosObject;
	private float startYpos;
	private float currentYpos;
	private RawImage currentLineVertical;


	public RawImage lineVerticalTemplate;
	public RawImage lineHorizontalTemplate;
	public Text dateTextTemplate;
	public Text timeTextTemplate;
	public Text visitorTextTemplate;
	public RawImage imageTemplate;
	public RawImage takenPicture;
	private float smallGap;
	private float largeGap;
	*/

	public GameObject visController;
	public GameObject visTemplate;
	public GameObject dateTemplate;
	public GameObject visTextOnlyTemplate;
	public GameObject sensorTemplate;

	private RectTransform thisRect;

	private Transform bg;
	private Transform lineVertical;
	private Transform lineHorizontal;
	private Transform timeText;
	private Transform visitorText;
	private Transform image;
	private Transform visitorCount;

	private GameObject dateBox;

	private int entryCount;
	private string currentDate;
	private string currentTime;
	bool needsDate;


	// Use this for initialization
	void Start () {
		needsDate = true;
		//smallGap = 40f;
		//largeGap = 120f;
		currentDate = "";
		currentTime = "";
		entryCount = 0;
		thisRect = gameObject.GetComponent<RectTransform> ();
		GenerateVisualization ();
	}
		
	public void GenerateVisualization() {
		// RESET existing vis?
		GuestEntryData guestData;// = new GuestEntryData();
		//guestData.init ();
		//dataManager.SaveData (guestData);
		dataManager.LoadData();
		guestData = dataManager.LocalCopyOfData;
		for (int i = 0; i < guestData.time.Count; i++) {			
			Texture2D snap = null;
			if (i < guestData.guestAvatar.Count) {
				snap = new Texture2D (camControl.frontCam.width, camControl.frontCam.height);
				snap.LoadImage (guestData.guestAvatar [i]);
			}
			AddEntry (snap, guestData.text[i], guestData.date[i], guestData.time[i]);
		}
	}

	public void AddEntry(Texture2D snap, string text, string date, string time) {
		entryCount += 1;
		GameObject vis = null;

		if (snap != null) {
			vis = Instantiate (visTemplate, visController.transform);
		} else {
			vis = Instantiate (visTextOnlyTemplate, visController.transform);
		}
		RectTransform visRect = vis.GetComponent<RectTransform> ();
		visRect.SetAsFirstSibling ();

		// grow the scrollable rect to make space
		float rectGrowth = 300f;

		// if the date doesn't match, use the beginVisTemplate
		if (currentDate != date) {
			dateBox = Instantiate (dateTemplate, visController.transform);
			Transform dateObj = dateBox.transform.GetChild (1);
			dateObj.GetComponent<Text> ().text = date;
			currentDate = date;

			// grow the scrollable rect to make space for the date header
			rectGrowth += 50f;

		}

		// make sure date is on top
		RectTransform dateBoxRect = dateBox.GetComponent<RectTransform> ();
		dateBoxRect.SetAsFirstSibling ();

		// get children of the vis
		//bg = vis.transform.GetChild (0);
		//lineVertical = vis.transform.GetChild (1);
		//lineHorizontal = vis.transform.GetChild (2);
		timeText = vis.transform.GetChild (3);
		visitorText = vis.transform.GetChild (4);
		if (currentTime == time) {
			timeText.GetComponent<Text> ().text = "";
		} else {
			timeText.GetComponent<Text> ().text = time;
		}

		visitorText.GetComponent<Text> ().text = text;

		// Create visitor image
		if (snap != null) {
			image = vis.transform.GetChild (5);
			image.GetComponent<RawImage> ().texture = snap;
		} else {
			visitorCount = vis.transform.GetChild (5);
			visitorCount.GetComponent<Text> ().text = "Visitor " + entryCount.ToString();
		}

		//currentYpos = -smallGap;
		// expand the Visualization rect to make room for more visualizations
		float thisRectSizeDeltaX = thisRect.sizeDelta.x;
		float thisRectSizeDeltaY = thisRect.sizeDelta.y;
		thisRect.sizeDelta = new Vector2 (thisRectSizeDeltaX, thisRectSizeDeltaY + rectGrowth);

	}
	/*
		/// __----



		// if the date doesn't match, Create a date header
			currentDateObj = Instantiate (dateTextTemplate, vis.transform);

			// update the date header position to currentYPos
			float currentDateObjanchorPosX = currentDateObj.rectTransform.anchoredPosition.x;
			currentDateObj.rectTransform.anchoredPosition = new Vector2 (currentDateObjanchorPosX, currentYpos);

		currentYpos -= smallGap;

		currentLineVertical = Instantiate (lineVerticalTemplate, vis.transform);
		float curLineVerSizeDeltaX = currentLineVertical.rectTransform.sizeDelta.x;
		currentLineVertical.rectTransform.sizeDelta = new Vector2 (curLineVerSizeDeltaX, 240f);
		float curLineVerRectPosX = currentLineVertical.rectTransform.anchoredPosition.x;
		currentLineVertical.rectTransform.anchoredPosition = new Vector2 (curLineVerRectPosX, currentYpos);

		currentYpos -= largeGap;

		// Create time text
		Text currentTimeText = Instantiate (timeTextTemplate, vis.transform);

		float curTimeTextRectPosX = currentTimeText.rectTransform.anchoredPosition.x;
		currentTimeText.rectTransform.anchoredPosition = new Vector2 (curTimeTextRectPosX, currentYpos);

		// Create horizontal line
		RawImage currentLineHorizontal = Instantiate (lineHorizontalTemplate, vis.transform);
		float curLineHorRectPosX = currentLineHorizontal.rectTransform.anchoredPosition.x;
		currentLineHorizontal.rectTransform.anchoredPosition = new Vector2(curLineHorRectPosX, currentYpos);

		RawImage currentImage = Instantiate (imageTemplate, vis.transform);
		float curImgRectPosX = currentImage.rectTransform.anchoredPosition.x;
		currentImage.rectTransform.anchoredPosition = new Vector2 (curImgRectPosX, currentYpos);



		// Create visitor text
		Text visitorText = Instantiate (visitorTextTemplate, vis.transform);

		float visitorTextRectPosX = visitorText.rectTransform.anchoredPosition.x;
		visitorText.rectTransform.anchoredPosition = new Vector2 (visitorTextRectPosX, currentYpos);

		currentYpos -= largeGap;

		// move RenderedVis downwards
		//float renderVisPosX = renderVisRect.anchoredPosition.x;
		//float renderVisPosY = renderVisRect.anchoredPosition.y;
		//renderVisRect.anchoredPosition = new Vector2(renderVisPosX, renderVisPosY-320f);
		//float renderVisSizeDeltaX = renderVisRect.sizeDelta.x;
		//float renderVisSizeDeltaY = renderVisRect.sizeDelta.y;
		//renderVisRect.sizeDelta = new Vector2 (renderVisSizeDeltaX, renderVisSizeDeltaY+320f);
		currentYpos = YPosObject.anchoredPosition.y;
		currentYpos += largeGap;
		currentYpos += smallGap;
		currentYpos += largeGap;
		currentYpos += largeGap;
		currentYpos += largeGap;
		currentYpos += smallGap;

	}*/
		
	// Update is called once per frame
	void Update () {
		
	}
}
