﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour {

    Vector3 oldPosition;

    //scrolling speed for WASD keys
    public float scrollSpeed = 1f;
    public float zoomSpeed = 1f;

    //variables for max distance zoomed in and max distance zoomed out, max zoom out distance being based on map size
    public float maxZoomIn = 10f;
    private float maxZoomOut;



    // Use this for initialization
    void Start ()
    {
        //set the "old position" of the camera to its current position for later use in CheckIfCameraMoves
        oldPosition = this.transform.position;

        //temporarily hard coded for the continents and also temporarily hardcoded multiplyer
        maxZoomOut = GameObject.Find("HexMap").GetComponent<HexMap_Continent>().rowCount;
        print(maxZoomOut);
	}
	
	// Update is called once per frame
	void Update ()
    {
        MoveCamera();
        ZoomCamera();

        CheckIfCameraMoved();
    }

    //Function for moving the camera
    private void MoveCamera()
    {
        //TODO: Code to click and drag camera

        //TODO: Limit the movement of the camera up/down and left/right based on if wrapping is allowed in the directions or not

        //move the camera with button presses
        if (Input.GetButton("MapUp"))
            transform.position += transform.up * scrollSpeed;
        if (Input.GetButton("MapDown"))
            transform.position -= transform.up * scrollSpeed;
        if (Input.GetButton("MapLeft"))
            transform.position -= transform.right * scrollSpeed;
        if (Input.GetButton("MapRight"))
            transform.position += transform.right * scrollSpeed;
    }

    //Function for zooming in and out camera
    private void ZoomCamera()
    {
        // zoom in and out
        if (Input.GetButton("ZoomIn") && transform.position.y >= maxZoomIn)
            transform.position += transform.forward * zoomSpeed;
        if (Input.GetButton("ZoomOut") && transform.position.y <= maxZoomOut)
            transform.position -= transform.forward * zoomSpeed;
    }

    void CheckIfCameraMoved()
    {
        if(oldPosition != this.transform.position)
        {
            //camera was moved
            oldPosition = this.transform.position;

            //TODO: Hexmap will have dictionary of all these later
            HexBehaviour[] hexes = GameObject.FindObjectsOfType<HexBehaviour>();
            foreach(HexBehaviour hex in hexes)
            {
                hex.UpdatePosition();
            }
        }
    }
}
