using UnityEngine;
using Pathfinding.Serialization.JsonFx; //make sure you include this using
using System;

public class Sketch : MonoBehaviour {
    public GameObject myPrefab;
    public GameObject the_cube;

    public string jsonResponse;

    public string _WebsiteURL = "http://xxu629.azurewebsites.net/tables/TreeSurvey?zumo-api-version=2.0.0"; //http://{Your Site Name}.azurewebsites.net/tables/{Your Table Name}?zumo-api-version=2.0.0
    //CHANGE

    void Start() {

        jsonResponse = Request.GET(_WebsiteURL);


        if (string.IsNullOrEmpty(jsonResponse))
        {
            return;
        }


        TheTreeSurvey[] tree = JsonReader.Deserialize<TheTreeSurvey[]>(jsonResponse);//CHANGE

        int i = 0;
        int totalLocate = tree.Length;

        for (i = 0; i < totalLocate; i++)
        {
            int x = tree[i].PositionX + i;//CHANGE water variable
            int y = tree[i].PositionY;//CHANGE
            int z = tree[i].PositionZ + i;//CHANGE
            string the_text = tree[i].TreeID;//CHANGE
            

            GameObject newLocation = (GameObject)Instantiate(myPrefab, new Vector3(x, y, z), Quaternion.identity);
            newLocation.GetComponentInChildren<TextMesh>().text = tree[i].Location;
            newLocation.GetComponent<LocationScript>().Category = the_text;


            if (tree[i].EcologicalValue == "Very High")//CHANGE
            {
                newLocation.GetComponent<Renderer>().material.color = Color.red;
            }

            else if(tree[i].EcologicalValue == "High")
            {
                newLocation.GetComponent<Renderer>().material.color = Color.yellow;
            }
            else if (tree[i].EcologicalValue == "Medium")
            {
                newLocation.GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                newLocation.GetComponent<Renderer>().material.color = Color.white;
            }

        }

    }

    void Update()
    {
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.tag == "location")
                {
                    hitInfo.collider.gameObject.GetComponent<Renderer>().material.color = Color.grey;
                    hitInfo.collider.gameObject.transform.localScale = new Vector3(transform.localScale.x * 0.5f, transform.localScale.y * 0.5f, transform.localScale.z * 0.5f);
                    int GIndex = Int32.Parse(hitInfo.collider.gameObject.GetComponent<LocationScript>().Category);

                    TheTreeSurvey[] tree = JsonReader.Deserialize<TheTreeSurvey[]>(jsonResponse);//CHANGE

                    GameObject newBoard = (GameObject)Instantiate(the_cube, new Vector3(hitInfo.point.x, hitInfo.point.y + 1.2f, hitInfo.point.z), Quaternion.identity);
                    newBoard.GetComponentInChildren<TextMesh>().text = "Ecologica lValue: " + "\n" + tree[GIndex - 1].EcologicalValue + "\n" + "Historical" + "\n" + "Significance: " + "\n" + tree[GIndex - 1].HistoricalSignificance + "\n" + "Coordinate: " + "\n" + "(" + tree[GIndex - 1].PositionX + "," + tree[GIndex - 1].PositionY + "," + tree[GIndex - 1].PositionZ + ")";
                    //CHANGE
                }

                if (hitInfo.collider.tag == "board")
                {
                    Destroy(hitInfo.collider.gameObject);
                }
            }
        }
    }
}
