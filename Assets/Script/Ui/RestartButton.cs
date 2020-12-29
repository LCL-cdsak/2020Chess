using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class RestartButton : MonoBehaviour {

    // Use this for initialization
    inicial inic;
    GameObject board;
    //public UnityEngine.UI.Button btn;

    void Start () {
        transform.parent.gameObject.SetActive(false);
        
    }
    public void OnMouseUpAsButton()
    {
        Debug.Log("Restart Button Clicked");
        if (inic != null)
        {
            inic.ResetGame();
            transform.parent.gameObject.SetActive(false);

        }
    }

    // Update is called once per frame
    void Update () {
		if(inic == null)
        {
            board = GameObject.Find("Board");
            if(board != null)
            {
                inic = board.GetComponent<inicial>();
                if(inic != null)
                {

                }
            }
            return;
        }

	}
}
