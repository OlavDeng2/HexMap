using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour {

    Vector3 oldPosition;


	// Use this for initialization
	void Start ()
    {
        oldPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //TODO: Code to click and drag camera
        // wasd
        // zoom in and out

        CheckIfCameraMoved();
	}

    public void PanToHex(Hex hex)
    {
        //TODO: Move camera to hex
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
