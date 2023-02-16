using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotMat : MonoBehaviour {

    float rot = 1;
    public Material matScheme;
    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        
        if(Input.GetKeyDown(KeyCode.R))
        {
            rotate();
            rot += 1;
            this.gameObject.GetComponent<MeshRenderer>().material = matScheme;
            Color newCol = this.GetComponent<MeshRenderer>().materials[1].color;
            this.gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color(newCol.r, newCol.g, newCol.b, 0.3f);
        }
		
	}

    void rotate()
    {
        Vector3 offset = Vector3.zero;
        Vector3 tiling = Vector3.zero;
        var matrix = Matrix4x4.TRS(offset, Quaternion.Euler(0, 0, rot), tiling);
        this.gameObject.GetComponent<MeshRenderer>().material.SetFloat("_Angle", rot);
    }
}
