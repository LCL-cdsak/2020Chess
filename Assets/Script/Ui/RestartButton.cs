using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
            Scene cur_scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(0);
            //inic.ResetGame();
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
