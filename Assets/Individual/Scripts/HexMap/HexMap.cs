using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour {

    public GameObject HexPrefab;

    //Meshes for our different tiles
    public Mesh meshWater;
    public Mesh meshFlat;
    public Mesh meshHill;
    public Mesh meshMountain;

    //Materials for our different tiles
    public Material matOcean;
    public Material matGrasslands;
    public Material matPlains;
    public Material matMountain;

    //Tiles with height above this will be a mountain
    public float mountainHeight = 0.85f;
    public float hillHeight = 0.6f;
    public float flatHeight = 0.0f;

    //skipping this value for now
    //public float waterHeight = -0.5f; 

    //The size of our map
    public int rowCount = 30;
    public int columnCount = 60;

    //TODO: link up with hex class
    bool allowWrapEastWest = true;
    bool allowWrapNorthSouth = false;

    //Create a 2d array for our hexes
    private Hex[,] hexes;

    private Dictionary<Hex, GameObject> hexToGameObjectMap;

    //Getter for the hex in position passed into function
    public Hex GetHexAt(int x, int y)
    {
        if(hexes == null)
        {
            Debug.LogError("Hexes array not yet instantiated!");
            return null;
        }
        
        //Required for wrapping to keep coordinates and not mess things up
        if (allowWrapEastWest)
        {
            x = x % columnCount;
            if (x < 0)
            {
                x += columnCount;
            }
        }
        if (allowWrapNorthSouth)
        {
            y = y % rowCount;
            if (y < 0)
            {
                y += rowCount;
            }

        }


        return hexes[x, y];
    }

	// Use this for initialization
	void Start ()
    {
        GenerateMap();
	}


    //Generate the hex map
    virtual public void GenerateMap()
    {
        //Initialize our array for our hexes
        hexes = new Hex[columnCount, rowCount];
        hexToGameObjectMap = new Dictionary<Hex, GameObject>();

        //Generate a map filled with ocean
        for (int column = 0; column < columnCount; column++)
        {
            for (int row = 0; row < rowCount; row++)
            {                
                //Create the hex
                Hex h = new Hex(column, row);
                h.elevation = -0.5f;

                //place the hex in the 2d array
                hexes[column, row] = h;

                //Get the position from the camera
                Vector3 pos = h.PositionFromCamera(Camera.main.transform.position, rowCount, columnCount);

                //Instantiate the hex
                GameObject hexGo = Instantiate(HexPrefab, pos, Quaternion.identity, this.transform);

                //map hex to dictionary
                hexToGameObjectMap[h] = hexGo;

                //Assign the proper data from the HexBehaviour
                hexGo.GetComponent<HexBehaviour>().Hex = h;
                hexGo.GetComponent<HexBehaviour>().HexMap = this;

                //Assign the coordinates of the hex
                //hexGo.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", column, row);

                //Destroy(for now at least) the textmesh if the coordinates is not desirable
                TextMesh textMesh = hexGo.GetComponentInChildren<TextMesh>();
                Destroy(textMesh);
            }
        }

        UpdateHexVisuals();
        //make the tiles non movable(left out for now)
        //StaticBatchingUtility.Combine(this.gameObject);
    }


    public void UpdateHexVisuals()
    {

        for (int column = 0; column < columnCount; column++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                //get the hex
                Hex h = hexes[column, row];
                GameObject hexGo = hexToGameObjectMap[h];

                //get the material for the ocean
                MeshRenderer mr = hexGo.GetComponentInChildren<MeshRenderer>();


                if (h.elevation >= mountainHeight)
                {
                    mr.material = matMountain;
                }


                //Commented out since we dont have any hills, yet.
                /*
                else if (h.elevation >= hillHeight)
                {
                    //dont have a hill yet so uses plains for now
                    mr.material = matPlains;
                }
                */

                else if (h.elevation >= flatHeight)
                {
                    mr.material = matGrasslands;
                }


                else
                {
                    mr.material = matOcean;
                }


                //get the mesh for the ocean(water)
                MeshFilter mf = hexGo.GetComponentInChildren<MeshFilter>();
                mf.mesh = meshWater;

            }
        }
    }

    public Hex[] GetHexesWithinRangeOff(Hex centerHex, int range)
    {
        List<Hex> results = new List<Hex>();
        for(int dx = -range; dx < range - 1; dx++)
        {
            for (int dy = Mathf.Max(-range + 1, -dx- range); dy < Mathf.Min(range, -dx+ range - 1); dy++)
            {
                results.Add(GetHexAt(centerHex.Q + dx, centerHex.R + dy));
            }
        }
        return results.ToArray();
    }
}
