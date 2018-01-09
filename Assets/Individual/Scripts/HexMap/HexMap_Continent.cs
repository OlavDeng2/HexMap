using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap_Continent : HexMap {

    // Use this for initialization
    public override void GenerateMap()
    {
        //Call the base class to make all hexes
        base.GenerateMap();

        int numContinents = 2;
        int continentSpacing = 20;
        for(int c= 0; c <numContinents; c++)
        {
            //Make raised area
            int landMasses = Random.Range(3, 6);
            for (int i = 0; i < landMasses; i++)
            {
                int range = Random.Range(5, 10);
                int y = Random.Range(range, rowCount - range);
                int x = Random.Range(0, 10) - y / 2 + (c * continentSpacing);

                ElevateArea(x, y, range);
            }

        }


        //add lumpiness to the area

        //Set mesh to mountain/hill/flatt/water based on height

        //Simulate rainfall/moisture and set plains/gresslands + forest


        UpdateHexVisuals();
    }

    void ElevateArea(int q, int r, int range, float centerHeight = 1f)
    {
        //get the hex and elevate the hexes
        Hex centerHex = GetHexAt(q, r);
        //centerHex.elevation = 0.5f;

        Hex[] areaHexes = GetHexesWithinRangeOff(centerHex, range);

        foreach (Hex h in areaHexes)
        {
            //if (h.elevation < 0)
            //    h.elevation = 0;
            h.elevation += centerHeight * Mathf.Lerp(1f, 0.25f, Hex.Distance(centerHex, h)/range);
        }
    }

}
