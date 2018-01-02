using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour {

    public GameObject HexPrefab;
    public int columnCount = 10;
    public int rowCount = 10;

    public Material[] HexMaterials;

	// Use this for initialization
	void Start ()
    {
        GenerateMap();
	}


    //Generate the hex map
    public void GenerateMap()
    {
        for (int column = 0; column < columnCount; column++)
        {
            for (int row = 0; row < rowCount; row++)
            {
                Hex h = new Hex(column, row);

                Vector3 pos = h.PositionFromCamera(Camera.main.transform.position, rowCount, columnCount);

                //instantiate the hex
                GameObject hexGo = Instantiate(HexPrefab, pos, Quaternion.identity, this.transform);

                hexGo.GetComponent<HexBehaviour>().Hex = h;

                MeshRenderer mr = hexGo.GetComponentInChildren<MeshRenderer>();
                mr.material = HexMaterials[Random.Range(0, HexMaterials.Length)];
            }
        }
        //make the tiles non movable(left out for now)
        //StaticBatchingUtility.Combine(this.gameObject);
    }
}
