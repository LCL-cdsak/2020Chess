using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script;

public class SelectPieceButton : MonoBehaviour {
    public delegate void PieceButtonClickedDelegate();
    public PieceButtonClickedDelegate PieceButtonClickedEvent;
    public Piece.PieceType type;
    public Button btn;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        if (btn == null)
        {
            Debug.Log("No btn in components");
        }
        btn.onClick.AddListener(ClickTest);
	}

    public void ClickTest()
    {
        Debug.Log("btn test");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

}
