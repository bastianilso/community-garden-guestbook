using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldAndDrawLine : MonoBehaviour {

	public GameObject lineObject;
	public GameObject currentLine;
	private LineRenderer line;
	private PointsList pointsList;
	private bool isMousePressed;
	private Vector3 mousePos;

	// Structure for line points
	struct myLine
	{
		public Vector3 StartPoint;
		public Vector3 EndPoint;
	};

	void Awake ()
	{
		// Create line renderer component and set its property
		isMousePressed = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		// If mouse button down, remove old line and set its color to green
		if (Input.GetMouseButtonDown(0) && isMousePressed == false) {
			currentLine = Instantiate (lineObject);
			isMousePressed = true;
			line = currentLine.GetComponent<LineRenderer> ();
			pointsList = currentLine.GetComponent<PointsList> ();
			line.SetVertexCount (0);
		}
		if (Input.GetMouseButtonUp (0)) {
			isMousePressed = false;

		}
		// Drawing line when mouse is moving(presses)
		if (isMousePressed) {
			mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			mousePos.z = 0;
			if (!pointsList.points.Contains (mousePos)) {
				pointsList.points.Add (mousePos);
				line.SetVertexCount (pointsList.points.Count);
				line.SetPosition (pointsList.points.Count - 1, (Vector3)pointsList.points [pointsList.points.Count - 1]);
			}
		}
	}
}
