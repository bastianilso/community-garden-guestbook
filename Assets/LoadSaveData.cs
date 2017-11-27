using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

[System.Serializable]
public class GuestEntryData
{
	public List<byte[]> guestAvatar;
	public List<int> guestID;
	public List<string> date;
	public List<string> time;
	public List<string> text;

	public void init() {
		guestAvatar = new List<byte[]> ();
		guestID = new List<int> ();
		date = new List<string> ();
		time = new List<string> ();
		text = new List<string> ();
	}

}

public class LoadSaveData : MonoBehaviour {

	public GuestEntryData LocalCopyOfData = null;
	public CameraController cameraController;
	public bool IsSceneBeingLoaded = false;

	public void SaveData(GuestEntryData data)
	{
		//if (!Directory.Exists ("Saves")) {
		//	Directory.CreateDirectory ("Saves");
		//}

		BinaryFormatter formatter = new BinaryFormatter();
		FileStream saveFile = File.Create(Application.persistentDataPath + "save.binary");

		LocalCopyOfData = data;

		formatter.Serialize(saveFile, LocalCopyOfData);

		saveFile.Close();
	}

	public void LoadData()
	{
		//if (!Directory.Exists ("Saves")) {
		//	Directory.CreateDirectory ("Saves");
		//}

		if (!File.Exists(Application.persistentDataPath + "save.binary")) {
			File.Create(Application.persistentDataPath + "save.binary");
		}

		BinaryFormatter formatter = new BinaryFormatter();
		FileStream saveFile = File.Open(Application.persistentDataPath + "save.binary", FileMode.Open);
		try {
			LocalCopyOfData = (GuestEntryData)formatter.Deserialize(saveFile);
		} 
		catch (System.Exception e) {
			LocalCopyOfData = new GuestEntryData { };
			LocalCopyOfData.init ();
			return;
		}
		finally {
			saveFile.Close ();
		}
		return;
	}

	// Use this for initialization
	/*void Awake () {
		if (!Directory.Exists ("Saves")) {
			Directory.CreateDirectory ("Saves");
		}
		FileStream saveFile = File.Create("Saves/save.binary");


	}*/
	
	// Update is called once per frame
	void Update () {
		
	}
}
