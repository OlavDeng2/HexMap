using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap_Continent : HexMap {

    [Header("Continent Generator")]
    public int numContinents = 2;

    // Use this for initialization
    public override void GenerateMap()
    {
        //Call the base class to make all hexes
        base.GenerateMap();

        GenerateContinents();
        GenerateMoisture();

        UpdateHexVisuals();
    }

    private void GenerateContinents()
    {

        //divide column count with number of continents to get "equal" distrobution of continents
        int continentSpacing = columnCount / numContinents;

        //setting seed to 0 so as to be able to better reproduse for testing, comment out or in for testing purposes.
        //Random.InitState(0);

        for (int c = 0; c < numContinents; c++)
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
        //Vector2 noiseOffset = new Vector2(Random.Range(0f, 1f ), Random.Range(0f, 1f));

        float noiseScale = 2f; //larger value = more islands and lakes

        for (int column = 0; column < columnCount; column++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                Hex h = GetHexAt(column, row);
                float noise = Mathf.PerlinNoise(((float)column / Mathf.Max(columnCount, rowCount) / noiseResolution) + noiseOffset.x, ((float)row / Mathf.Max(columnCount, rowCount) / noiseResolution)) + noiseOffset.y - 0.5f;
                h.elevation += noise * noiseScale;
            }
        }
    }

    private void GenerateMoisture()
    {
        //TODO: Write code to add moisture


        //code for randomness of moisture
        float moistureResolution = 0.1f;

        //add some randomization for the Perlin noise to make the map generation more "natural", needs more taking look at to work properly and nicely
        Vector2 moistureOffset = new Vector2(0, 0);
        //Vector2 moistureOffset = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));

        float moistureScale = 0.5f; //larger value = more forrests, lower value = more dessert




        for (int column = 0; column < columnCount; column++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                Hex h = GetHexAt(column, row);

                //TODO: Improve the algorithm for generating dessert, grassland and forrest as it is highly based on elevation right now

                //create basic strip of dessert, grasslands and forrests to form our base.
                //TODO: if possible, simplify the formula to get the lerpscale. Seems unnecesarrily complex right now(but functional)
                float lerpScale = (((Mathf.Abs(row - (Mathf.Abs((float)rowCount / 2f - (float)rowCount)))/100f) * (float)rowCount)*2)/10-1; //lerpscale = 1 means dessert, =0 means forrest
                lerpScale = Mathf.Abs(lerpScale);

                //Generate the initial moisture
                h.moisture = Mathf.Lerp(1f, 0f, lerpScale);
         
                print("row: " + h.R + " collumn: " + h.Q + " moisture: " + h.moisture);


                
                //add some randomness to the moisture
                float moisture = Mathf.PerlinNoise(((float)column / Mathf.Max(columnCount, rowCount) / moistureResolution) + moistureOffset.x, ((float)row / Mathf.Max(columnCount, rowCount) / moistureResolution)) + moistureOffset.y - 0.5f;
                h.moisture += moisture * moistureScale;
                
            }
        }
    }

    void ElevateArea(int q, int r, int range, float centerHeight = 0.8f)
    {
        //get the hex and elevate the hexes
        Hex centerHex = GetHexAt(q, r);
        //centerHex.elevation = 0.5f;

        Hex[] areaHexes = GetHexesWithinRangeOff(centerHex, range);

        foreach (Hex h in areaHexes)
        {
            h.elevation = centerHeight * Mathf.Lerp(1f, 0.25f, Mathf.Pow(Hex.Distance(centerHex, h)/range, 2));
        }
    }

}
