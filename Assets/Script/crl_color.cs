using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class crl_color : MonoBehaviour {

    public bool change_color=false;
    public inicial inicial_object = null;
    // Use this for initialization
    void OnMouseOver()//滑鼠懸浮於按鈕之上，顏色變化
    {
        /*if (!change_color)
        {
            this.transform.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0.085f, 1);
        }*/
        if (inicial_object == null)
        {
            inicial_object = (inicial)GameObject.Find("Board").GetComponent<inicial>();
            return;
        }
        else
        {
            if (inicial_object.chess.is_selected_piece)
            {
                // same team, check if at valid_path == true
                if (inicial_object.chess.selected_piece.valid_path[(int)transform.position.x, (int)transform.position.z])
                {
                    PieceLightUp();
                }
                else
                {
                    // not valid path
                }
            }
            else
            {
                if (inicial_object.chess.current_team == this.gameObject.tag)
                {
                    PieceLightUp();
                }
            }
        }

    }

    private void OnMouseExit()//滑鼠離開按鈕上方，顏色復位
    {
        /*if (!change_color)
        {
            if (transform.tag != "black")
            {
                this.transform.GetComponent<MeshRenderer>().material.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
            }
            else if (transform.tag != "white")
            {
                this.transform.GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.2f, 0.2f, 0.2f);
            }
        }*/
        if (inicial_object == null)
            inicial_object = GameObject.Find("Board").GetComponent<inicial>();
        else
        {
            if (inicial_object.chess.is_selected_piece)
            {
                //Debug.Log("color: io.c.is_selected_piece = true");
                if (inicial_object.temp_piece == this.gameObject)
                {
                    // this piece is selected, don't LightOff
                    return;
                }
                else
                {
                    // this piece is not the selected one
                    PieceLightOff();
                }
            }
            else
            {
                // no piece is selected
                //Debug.Log("color: io.c.is_selected_piece = false");
                PieceLightOff();
            }
        }
    }
    void PieceLightUp()
    {
        this.transform.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0.085f, 1);
    }
    void PieceLightOff()
    {
        if (this.transform.tag == "black")
        {
            this.transform.GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.2f, 0.2f, 0.2f);
        }
        else if (this.transform.tag == "white")
        {
            this.transform.GetComponent<MeshRenderer>().material.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        }
    }
    
    void Start () {
        inicial_object = GameObject.Find("Board").GetComponent<inicial>();
        if (this.transform.tag == "black")
        {
            this.transform.GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.2f, 0.2f, 0.2f);
        }
        else if (this.transform.tag == "white")
        {
            this.transform.GetComponent<MeshRenderer>().material.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        }
	}

    private void Update()
    {
        inicial_object = GameObject.Find("Board").GetComponent<inicial>();
    }
    /*
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //if (hit.collider.gameObject.name != "Board")
                if(hit.collider.gameObject.transform.IsChildOf(GameObject.Find("Pieces").transform))
                {
                    if (change_color)
                    {
                        change_color = false;
                    }
                    else
                    {
                        change_color = true;
                    }
                }
            }
        }
    }*/
}
