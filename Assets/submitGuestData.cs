using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class submitGuestData : MonoBehaviour {

	public RawImage pictureAvatar; //preview
	public CameraController camControl; // the actual 2d texture we store
	public LoadSaveData dataManager;
	public VisualizationGenerator visGen;
	public InputField inputField;
	public GuestEntryData guestData;

	public void sendData() {
		guestData = dataManager.LoadData ();
		System.DateTime.Now.ToString("MMMM");
		string currentDate = System.DateTime.Now.ToString("dd");
		currentDate += GetDaySuffix (System.DateTime.Now.Day);
		currentDate += " " + System.DateTime.Now.ToString("MMMM");
		string currentTime = System.DateTime.Now.ToString("HH:mm");
		guestData.time.Add(currentTime);
		guestData.date.Add (currentDate);
		guestData.text.Add (inputField.text);
		if (camControl.snapshot != null) {
			guestData.guestAvatar.Add (camControl.snapshot.EncodeToPNG ());
		}

		visGen.AddEntry(camControl.snapshot, inputField.text, currentDate, currentTime);
		dataManager.SaveData (guestData);

		// reset guestbook
		inputField.text = "";
		pictureAvatar.texture = null;
		camControl.snapshot = null;
		camControl.pictureTaken = false;
		pictureAvatar.enabled = false;
		// visGen.GenerateVisualization(); or maybe updateVisualization?
		// prepare everything in a GuestEntryData struct
		// call SaveData
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	string GetDaySuffix(int day)
	{
		switch (day)
		{
		case 1:
		case 21:
		case 31:
			return "st";
		case 2:
		case 22:
			return "nd";
		case 3:
		case 23:
			return "rd";
		default:
			return "th";
		}
	}

}
