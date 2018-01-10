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

        //divide column count with number of continents to get "equal" distrobution of continents
        int continentSpacing = columnCount/numContinents;

        //setting seed to 0 so as to be able to better reproduse for testing, comment out or in for testing purposes.
        //Random.InitState(0);

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
        float noiseResolution = 0.1f;

        //add some randomization for the Perlin noise to make the map generation more "natural", needs more taking look at to work properly and nicely
        Vector2 noiseOffset = new Vector2(0, 0);
        //Vector2 noiseOffset = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));

        float noiseScale = 2f; //larger value = more islands and lakes

        for (int column = 0; column < columnCount; column++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                Hex h = GetHexAt(column, row);
                float noise = Mathf.PerlinNoise(((float)column / Mathf.Max(columnCount, rowCount) / noiseResolution) + noiseOffset.x, ((float)row /Mathf.Max(columnCount, rowCount) / noiseResolution)) + noiseOffset.y - 0.5f;
                h.elevation += noise * noiseScale;

            }
        }


        //TODO: Write code to add moisture

        //add some randomness to the moisture
        float moistureResolution = 0.1f;

        //add some randomization for the Perlin noise to make the map generation more "natural", needs more taking look at to work properly and nicely
        Vector2 moistureOffset = new Vector2(0, 0);
        //Vector2 noiseOffset = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));

        float moistureScale = 2f; //larger value = more islands and lakes

        for (int column = 0; column < columnCount; column++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                Hex h = GetHexAt(column, row);
                float moisture = Mathf.PerlinNoise(((float)column / Mathf.Max(columnCount, rowCount) / moistureResolution) + moistureOffset.x, ((float)row / Mathf.Max(columnCount, rowCount) / moistureResolution)) + moistureOffset.y - 0.5f;
                h.moisture += moisture * moistureScale;

            }
        }

        //Set mesh to mountain/hill/flatt/water based on height

        //Simulate rainfall/moisture and set plains/gresslands + forest


        UpdateHexVisuals();
    }

    void ElevateArea(int q, int r, int range, float centerHeight = 0.8f)
    {
        //get the hex and elevate the hexes
        Hex centerHex = GetHexAt(q, r);
        //centerHex.elevation = 0.5f;

        Hex[] areaHexes = GetHexesWithinRangeOff(centerHex, range);

        foreach (Hex h in areaHexes)
        {
            //if (h.elevation < 0)
            //    h.elevation = 0;
            h.elevation = centerHeight * Mathf.Lerp(1f, 0.25f, Mathf.Pow(Hex.Distance(centerHex, h)/range, 2));
        }
    }

}
