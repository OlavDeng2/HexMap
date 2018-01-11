using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Defines grid position, size and neighbors and other potential information of a hex tile
public class Hex {
    //constructor
    public Hex(HexMap hexMap, int q, int r)
    {
        this.hexMap = hexMap; 
        // Q + R + S = 0
        //Therefore S = -(Q + R)
        this.Q = q;
        this.R = r;
        this.S = -(q + r);
    }


    public readonly int Q; //q for collumn
    public readonly int R; //r for row
    public readonly int S;


    //extra data for potential future use in map generation
    public float elevation;
    public float moisture;
    public float temperature;

    private HexMap hexMap;

    float radius = 1f;
   

    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;


    //Returns the world space position of the hex
    public Vector3 Position()
    {
        //x is column+half of row to counter offset of the hex tile. y is 0 to keep everything on ground level. Z is just the row.
        return new Vector3(HexHorizontalSpacing() * (this.Q + this.R/2f), 0, HexVerticalSpacing() * this.R);
    }

    //get the height of the hex
    float HexHeight()
    {
        return radius * 2;
    }

    //get the width of the hex
    float HexWidth()
    {
        return WIDTH_MULTIPLIER * HexHeight();
    }

    //Helper to get the vertical spacing
    float HexVerticalSpacing()
    {
        return HexHeight() * 0.75f;
    }

    //helper to get the horizontal spacing
    float HexHorizontalSpacing()
    {
        return HexWidth();
    }

    //Get the position from camera to allow for the "scrolling" of the map
    public Vector3 PositionFromCamera(Vector3 cameraPosition, float rowCount, float columnCount)
    {

        float mapHeight = rowCount * HexVerticalSpacing();
        float mapWidth = columnCount * HexHorizontalSpacing();

        Vector3 position = Position();

        
        if (hexMap.allowWrapEastWest)
        {
            float howManyWidthsFromCamera = (position.x - cameraPosition.x) / mapWidth;


            //we want how many widths from camera to be from -0.5 to 0.5 ideally
            if (howManyWidthsFromCamera > 0)
                howManyWidthsFromCamera += 0.5f;
            else
                howManyWidthsFromCamera -= 0.5f;


            //find out how far away we are from where we should be
            int howManyWidthsOff = (int)howManyWidthsFromCamera;

            position.x -= howManyWidthsOff * mapWidth;
        }

        if (hexMap.allowWrapNorthSouth)
        {
            float howManyHeightsFromCamera = (position.z - cameraPosition.z) / mapHeight;


            //we want how many widths from camera to be from -0.5 to 0.5 ideally
            if (howManyHeightsFromCamera > 0)
                howManyHeightsFromCamera += 0.5f;
            else
                howManyHeightsFromCamera -= 0.5f;


            //find out how far away we are from where we should be
            int howManyHeightsOff = (int)howManyHeightsFromCamera;

            position.z -= howManyHeightsOff * mapHeight;
        }
        return position;
    }

    public static float Distance(Hex a, Hex b)
    {

        //Probably wrong for wrapping
        int dQ = Mathf.Abs(a.Q - b.Q);
        if (a.hexMap.allowWrapEastWest)
        {
            if (dQ > a.hexMap.columnCount / 2)
                dQ = a.hexMap.columnCount - dQ;
        }

        int dR = Mathf.Abs(a.R - b.R);
        if (a.hexMap.allowWrapNorthSouth)
        {
            if (dR > a.hexMap.rowCount / 2)
                dR = a.hexMap.rowCount - dR;
        }

        //FIXME: Wrapping
        //return Mathf.Max(Mathf.Abs(a.Q - b.Q), Mathf.Abs(a.R - b.R), Mathf.Abs(a.S - b.S));
        return Mathf.Max(dQ, dR, Mathf.Abs(a.S - b.S));
    }
}
