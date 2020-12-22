using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Assets.Script;
public class one_player_start : MonoBehaviour ,IPointerClickHandler{
    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.name == "level 1") Mode_Select.Level = 1;
        else if (this.name == "level 2") Mode_Select.Level = 2;
        else if (this.name == "level 3") Mode_Select.Level = 3;
        Mode_Select.Mode = 1;
        Scene cur_scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(1);
    }
}
