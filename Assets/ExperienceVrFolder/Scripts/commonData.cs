using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class commonData : MonoBehaviour {

    public GameObject MainRotationGo;
    public GameObject[] massPoints;

    public int type = 0;

    [HideInInspector]
    public GameObject selectObj;
    [HideInInspector]
    public int rotCount = 0;

    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
	}

    void Update()
    {

    }

    public int countScore()
    {
        int score = 0;

        for (int i = 0; i < massPoints.Length; i++)
        {
            Debug.Log(i);
            if(massPoints[i].tag == "interactive")
            {
                if(type == 1)
                {
                    foreach (int s in massPoints[i].GetComponent<greenField_scheme>().pointToScore)
                    {
                        if (massPoints[i].GetComponent<greenField_scheme>().chooseNum + 1 == s)
                        {
                            score++;
                        }
                    }
                }
                else if(type == 0)
                {
                    foreach (int s in massPoints[i].GetComponent<greenField>().pointToScore)
                    {
                        if (massPoints[i].GetComponent<greenField>().chooseNum + 1 == s)
                        {
                            score++;
                        }
                    }
                }
                else
                {
                    foreach (int s in massPoints[i].GetComponent<greenField_nasos>().pointToScore)
                    {
                        if (massPoints[i].GetComponent<greenField_nasos>().chooseNum + 1 == s)
                        {
                            score++;
                        }
                    }
                }
                
            }
                
        }

        return score;
    }
}
