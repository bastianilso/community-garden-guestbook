using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorInfoGrabber : MonoBehaviour {

	public float timeLeft;
	private float currentTimeLeft;
	// Checking interval
	// Timer stuff
	private LoadSaveData dataManager;
	private string url;

	public string lastVisitor;

	// Use this for initialization
	void Start () {
		lastVisitor = "";
		url = "http://192.168.4.1/read";
		dataManager = gameObject.GetComponent<LoadSaveData> ();
	}
	
	// Update is called once per frame
	void Update () {
		// if its time to fetch info
			// FetchInfo();
		currentTimeLeft -= Time.deltaTime;
		if (currentTimeLeft < 0) {
			Debug.Log ("checking text");
			StartCoroutine (FetchInfo());
			currentTimeLeft = timeLeft;
		}
	}

	IEnumerator FetchInfo()
	{
		WWW www = new WWW(url);
		yield return www;
		Debug.Log (www.text);
		if (lastVisitor != www.text) { // comparing timestamps
			Debug.Log("saving visitor to db");
			lastVisitor = www.text;
			GuestEntryData guestData = dataManager.LocalCopyOfData;

			System.DateTime.Now.ToString("MMMM");
			string currentDate = System.DateTime.Now.ToString("dd");
			currentDate += GetDaySuffix (System.DateTime.Now.Day);
			currentDate += " " + System.DateTime.Now.ToString("MMMM");
			string currentTime = System.DateTime.Now.ToString("hh:mm tt");
			guestData.guestAvatar.Add (	new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 });
			guestData.time.Add(currentTime);
			guestData.date.Add (currentDate);
			dataManager.SaveData(guestData);

		}

		// if currentVisitor == lastvisitor
			// the visitor hasnt changed
		// else
			// save the new visitor with a call to SaveData()
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
