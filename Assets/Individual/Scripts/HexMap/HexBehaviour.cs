﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexBehaviour : MonoBehaviour {
    public Hex Hex;
    public HexMap HexMap;

    //Update the position of the hex based on camera position
    public void UpdatePosition()
    {
        this.transform.position = Hex.PositionFromCamera(Camera.main.transform.position, HexMap.rowCount, HexMap.columnCount);
    }
}
