using System.Collections;
using System.Collections.Generic;
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
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
