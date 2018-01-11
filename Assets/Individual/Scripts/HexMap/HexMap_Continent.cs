using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap_Continent : HexMap {
    [Header("Continent Generator")]
    //define the amount of continents generated
    public int numContinents = 2;

    // Use this for initialization
    public override void GenerateMap()
    {
        //Call the base class to make all hexes
        base.GenerateMap();

        //Generate the map
        GenerateContinents();
        GenerateMoisture();
        GenerateTemperature();

        //Update the visuals of the map
        UpdateHexVisuals();
    }

    private void GenerateContinents()
    {

        //divide column count with number of continents to get "equal" distrobution of continents
        int continentSpacing = columnCount / numContinents;

        //Generate the random landmasses
        //TODO: Make the amount of landmasses and size of landmasses based on map size and/or player settings
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

        //add lumpiness to the map
        float noiseResolution = 0.1f;

        //add some randomization for the Perlin noise to make the map generation more "natural"
        Vector2 noiseOffset = new Vector2(Random.Range(-0.1f, 0.1f ), Random.Range(-0.1f, 0.1f));

        float noiseScale = 2f; //larger value = more islands and lakes

        //the loop to actually assign the added lumpiness to the map
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
        //code for randomness of moisture
        float noiseResolution = 0.1f;

        //add some randomization for the Perlin noise to make the map generation more "natural"
        Vector2 noiseOffset = new Vector2(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f));

        float noiseScale = 0.5f; //larger value = more forrests, lower value = more dessert

        //actual loop for adding the moisture to the area
        for (int column = 0; column < columnCount; column++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                Hex h = GetHexAt(column, row);

                //TODO: Improve the algorithm for generating dessert, grassland and forrest as it is highly based on elevation right now

                //create basic strip of dessert, grasslands and forrests to form our base.
                float lerpScale = (Mathf.Abs(row - (rowCount / 2f)))/(rowCount/2); //lerpscale = 1 means dessert, =0 means forrest
                print(lerpScale + " " + row);
                
                //Generate the initial moisture
                h.moisture = Mathf.Lerp(0f, 1f, lerpScale);
         
                //add some randomness to the moisture
                float moisture = Mathf.PerlinNoise(((float)column / Mathf.Max(columnCount, rowCount) / noiseResolution) + noiseOffset.x, ((float)row / Mathf.Max(columnCount, rowCount) / noiseResolution)) + noiseOffset.y - 0.5f;
                h.moisture += moisture * noiseScale;
                
            }
        }
    }

    //Note: temperature at the moment does not do anything
    private void GenerateTemperature()
    {

        //actual loop for adding the temperature to the area
        for (int column = 0; column < columnCount; column++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                Hex h = GetHexAt(column, row);

                //create basic strips of temperatures to form our base.
                float lerpScale = (Mathf.Abs(row - rowCount / 2f)) / (rowCount / 2); //lerpscale.
                lerpScale = Mathf.Abs(lerpScale);

                //Generate the initial temperature
                h.temperature = Mathf.Lerp(0f, 1f, lerpScale);
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
