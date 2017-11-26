using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
	//public GuestEntryData guestData;
	private bool camAvailable;
	public bool pictureTaken;
	public WebCamTexture frontCam;
	private Texture defaultBackground;
	public Texture2D snapshot;
	public GameObject pictureAvatar;

	public RawImage background; // fallback background
	public AspectRatioFitter fit;
	// Use this for initialization
	void Start ()
	{
		//guestData = dataManager.LoadData ();
		//Debug.Log (guestData);
		//if (guestData.guestID.Count > 0) {
	//		currentID = (guestData.guestID [guestData.guestID.Count - 1]) + 1;
	//	} else {
	//		currentID = 1;
	//	}
			
		defaultBackground = background.texture;
		snapshot = null;
		WebCamDevice[] devices = WebCamTexture.devices; // an array of our webcam devices

		if (devices.Length == 0) {
			Debug.Log ("No cameras detected");
			camAvailable = false;
			return;
		}
			
		for(int i = 0; i < devices.Length; i++)
		{
			if (devices[i].isFrontFacing)
			{
				frontCam = new WebCamTexture (devices [i].name, Screen.width, Screen.height);
			}
		}

		if (frontCam == null)
		{
			Debug.Log("Unable to find front camera");
			return;
		}

		frontCam.Play();
		background.texture = frontCam;
		background.color = new Color (1f, 1f, 1f, 1f);

		camAvailable = true;	 //false; //true;
		pictureTaken = false;
	}

	// Update is called once per frame
	void Update ()
	{
		if (!camAvailable) {
			return;
		}

		if (pictureTaken) {
			return;
		}

		float ratio = (float)frontCam.width / (float)frontCam.height;
		fit.aspectRatio = ratio;

		float scaleY = frontCam.videoVerticallyMirrored ? -1f: 1f;
		background.rectTransform.localScale = new Vector3 (1f, scaleY, 1f);

		int orient = -frontCam.videoRotationAngle;
		background.rectTransform.localEulerAngles = new Vector3 (0, 0, orient);

	}

	public void takePicture ()
	{
		if (camAvailable) {
			frontCam.Pause ();
			snapshot = new Texture2D (frontCam.width, frontCam.height);
			snapshot.SetPixels (frontCam.GetPixels ());
			snapshot.Apply ();
			RawImage rawAvatar = pictureAvatar.GetComponent<RawImage> ();
			rawAvatar.texture = frontCam;
			//guestData.guestAvatar.Add(snap.EncodeToPNG());
		} 

		pictureTaken = true;
		// TODO: save picture
	}
}