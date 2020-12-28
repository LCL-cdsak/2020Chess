using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script;

public class SelectPieceButton : MonoBehaviour {
    public delegate void PieceButtonClickedDelegate(Piece.PieceType type);
    public PieceButtonClickedDelegate PieceButtonClickedEvent;
    public Piece.PieceType type;
    public Button btn;
    public GameObject board;
    public inicial inic;
	// Use this for initialization
	void Start () {
        btn = GetComponent<Button>();
        if (btn == null)
        {
            Debug.Log("No btn in components");
        }
        btn.onClick.AddListener(ClickTest);
        board = GameObject.Find("Board");
        inic = board.GetComponent<inicial>();
	}

    public void ClickTest()
    {
        Debug.Log(type.ToString() + " btn clicked");
        PieceButtonClickedEvent(type);
    }
	
	// Update is called once per frame
	void Update () {
        if (inic.is_selecting_piece_type)
        {
            if (Input.GetMouseButtonDown(0))
            {
                inic.is_selecting_piece_type = false;
                inic.CloseSelectPieceUI();
            }
        }
	}

}
