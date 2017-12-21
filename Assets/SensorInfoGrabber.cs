using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorInfoGrabber : MonoBehaviour {

	public float timeLeft;
	private float currentTimeLeft;

	public float visTime;
	private float currentVisTimeLeft;

	public VisualizationGenerator visGen;
	// Checking interval
	// Timer stuff
	private LoadSaveData dataManager;
	private string url;

	public string lastVisitor;
	private int lastVisitorCount;
	private string currentTime;
	private string currentDate;

	// Use this for initialization
	void Start () {
		lastVisitor = "";
		lastVisitorCount = 0;
		currentVisTimeLeft = visTime;
		url = "http://192.168.4.1/read";
		dataManager = gameObject.GetComponent<LoadSaveData> ();
	}
	
	// Update is called once per frame
	void Update () {
		// if its time to fetch info
			// FetchInfo();
		currentTimeLeft -= Time.deltaTime;
		currentVisTimeLeft -= Time.deltaTime;
		if (currentTimeLeft < 0) {
			//Debug.Log ("checking text");
			StartCoroutine (FetchInfo());
			currentTimeLeft = timeLeft;
		}
		if (currentVisTimeLeft < 0 && lastVisitorCount > 0) {
			//Debug.Log ("Vistime rand out:" + currentVisTimeLeft);
			Texture2D snap = null;
			visGen.AddEntry (snap, "", currentDate, currentTime, lastVisitorCount);
			currentVisTimeLeft = visTime;
			//Debug.Log ("Setting  currentVisTimeLeft to: " + currentVisTimeLeft);
			GuestEntryData guestData = dataManager.LocalCopyOfData;
			guestData.guestAvatar.Add (	new byte[] {  }); // 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20
			guestData.time.Add(currentTime);
			guestData.date.Add (currentDate);
			guestData.count.Add (lastVisitorCount);
			//Debug.Log ("lastVisitorCount: " + lastVisitorCount.ToString());
			guestData.text.Add ("");
			lastVisitorCount = 0;
			dataManager.SaveData(guestData);
		}
	}

	IEnumerator FetchInfo()
	{

		WWW www = null;
		try {
			www = new WWW(url);
		} catch (System.Exception  e) {
			// do nothing
		}

		yield return www;
		if (www == null) {
			yield break;
		}
			
		if (!string.IsNullOrEmpty(www.error))
			Debug.Log(www.error);
		//Debug.Log ("www.text: " + www.text);

		if (lastVisitor != www.text) { // comparing timestamps
			//Debug.Log("saving visitor to db");
			lastVisitorCount += 1;
			lastVisitor = www.text;
			System.DateTime.Now.ToString("MMMM");
			currentDate = System.DateTime.Now.ToString("dd");
			currentDate += GetDaySuffix (System.DateTime.Now.Day);
			currentDate += " " + System.DateTime.Now.ToString("MMMM");
			currentTime = System.DateTime.Now.ToString("hh:mm tt");

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
