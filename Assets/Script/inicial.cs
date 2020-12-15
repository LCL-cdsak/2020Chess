using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using Assets.Script;

public class inicial : MonoBehaviour {

    // Use this for initialization
    ChessAlgorithm algorithm = new ChessAlgorithm();
    public Chess chess;
    public GameObject temp_piece;
    public GameObject[] Piece = new GameObject[32];
    public Transform HintBlockTransform;
    public Transform[,] HintBlocks = new Transform[8, 8];
    int[] best_path = new int[4];//[0] is fromx,[1] is fromy,[2] is tox,[3] is toy

    void Start() {
        chess = new Chess();
        chess.MovePieceEvent += MovePieceGameObject;
        //  Piece = GameObject.FindGameObjectsWithTag("piece");

        /*
            Debug.Log(Piece[15].transform.localPosition.x);
            Debug.Log(Piece[15].transform.localPosition.z);
            //Console.WriteLine();
        */
        GameObject board = GameObject.Find("Board");
        for(int i=0; i<8; ++i)
        {
            for(int k=0; k<8; ++k)
            {
                HintBlocks[i, k] = Instantiate(HintBlockTransform);
                HintBlocks[i, k].SetParent(board.transform);
                HintBlocks[i, k].position = new Vector3(i, 0, k);
                HintBlocks[i, k].GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.3f);
                HintBlocks[i, k].GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
                HintBlocks[i, k].GetComponent<Renderer>().enabled = false;
            }
        }
    }
    // Update is called once per frame

    int Position_row;
    int Position_col;
    private bool is_deselect=true;
    public bool is_mouse_dragging = false;
    void Update() {

        if (Input.GetMouseButtonDown(0))
        {
            is_mouse_dragging = true;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Position_row =Convert.ToInt32(Math.Round(hit.point.x, 0, MidpointRounding.AwayFromZero)) ;
                Position_col = Convert.ToInt32(Math.Round(hit.point.z, 0, MidpointRounding.AwayFromZero));
                // print(Math.Round(Position_row, 0, MidpointRounding.AwayFromZero));
                if (chess.is_selected_piece)
                {
                    // move the selected piece if path is valid
                    if (!chess.MovePiece(Position_row, Position_col, out is_deselect))
                    {
                        if (is_deselect)
                        {
                            Clean_ValidPath_HintBlocks();
                            // not only no move, set the piece to Transparent
                            //Debug.Log("Deselect");
                            if (hit.collider.gameObject.tag == "white")
                            {

                                Debug.Log("Deselect white");
                                hit.collider.gameObject.transform.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1);
                            }
                            else if (hit.collider.gameObject.tag == "black")
                            {
                                Debug.Log("Deselect black");
                                hit.collider.gameObject.transform.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0);
                            }
                            
                            //piece.BackColor = Color.Transparent;
                            return;
                        }
                        return;
                    }
                    else
                    {
                        // Piece Moved
                        Clean_ValidPath_HintBlocks();
                    }
                    //單人模式 (94-96行註解即可轉為雙人模式)
                    best_path = algorithm.AI(ref chess.map);
                    chess.SelectPiece(best_path[0], best_path[1]);                
                    chess.MovePiece(best_path[2], best_path[3], out is_deselect);
                    /////** This work now done by MovePieceGameObject() **/////
                    /*temp = GetPictureBoxIndexFromLocation(Position_row, Position_col);
                    // Remove the dead piece
                    if (chess.map_NotNull)
                    {
                        Piece[temp].transform.position = new Vector3(100, 100, 100);
                    }*/

                    /*Vector3 temp1 = temp_piece.transform.position;
                    temp1.x = Position_row;
                    temp1.z = Position_col;
                    temp_piece.transform.position = temp1;
                    if (temp_piece.tag == "white")
                    {
                        temp_piece.transform.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1);
                    }
                    else if (temp_piece.tag == "black")
                    {
                        temp_piece.transform.GetComponent<MeshRenderer>().material.color = new Color(0, 0, 0, 0);
                    }*/

                    /***********************************************************************************************
                    MovePieceGameObject((int)temp_piece.transform.position.x, (int)temp_piece.transform.position.z, 
                                        Position_row, Position_col);
                    ***********************************************************************************************/

                    //piece.BackColor = Color.Transparent;
                    //ai move

                    /*best_path = algorithm.AI(ref chess.map);
                    if (chess.SelectPiece(best_path[0], best_path[1]))
                    {
                        temp_piece = Piece[GetPictureBoxIndexFromLocation(best_path[0], best_path[1])];
                    }
                    chess.MovePiece(best_path[2], best_path[3], out is_deselect);
                    chess.RoundInitialize();
                    //change picturebox
                    temp = GetPictureBoxIndexFromLocation(best_path[2], best_path[3]);
                    if (chess.map_NotNull)
                    {
                        Piece[temp].transform.position = new Vector3(100, 100, 100);
                    }
                    Vector3 temp2 = temp_piece.transform.position;
                    temp2.x =best_path[2] ;
                    temp2.z =best_path[3] ;
                    temp_piece.transform.position = temp2;*/

                }
                else if(hit.transform.IsChildOf(GameObject.Find("Pieces").transform))
                {
                    // select the piece if player valid
                    if (chess.SelectPiece(Position_row, Position_col))
                    {
                        //Console.WriteLine("ssss");

                        temp_piece = Piece[GetPictureBoxIndexFromLocation(Position_row, Position_col)];
                        hit.collider.gameObject.transform.GetComponent<MeshRenderer>().material.color = new Color(0, 1, 0.085f, 1);
                        Display_ValidPath_HintBlocks(chess.map[Position_row, Position_col].valid_path);
                        // piece.BackColor = Color.LightBlue;
                    }

                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            is_mouse_dragging = false;
            // determine if need drag back
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                int Position_row_up = Convert.ToInt32(Math.Round(hit.point.x, 0, MidpointRounding.AwayFromZero));
                int Position_col_up = Convert.ToInt32(Math.Round(hit.point.z, 0, MidpointRounding.AwayFromZero));
                if (Position_row == Position_row_up && Position_col == Position_col_up)
                {
                    // Player click the piece, no drag
                    return;
                }
                else
                {
                    // Player drag the piece to a new location
                    bool temp_deselect;// just for the argument
                    if (chess.MovePiece(Position_row_up, Position_col_up, out temp_deselect))
                    {
                        // chess Success move, move the gameobject

                       /*********************************************************************************
                        MovePieceGameObject(Position_row, Position_col, Position_row_up, Position_col_up);
                       /*********************************************************************************/
                    }
                }
            }
        }

        // The mouse moving color change effect (dragging effect)
        if (is_mouse_dragging)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                int Position_row_up = Convert.ToInt32(Math.Round(hit.point.x, 0, MidpointRounding.AwayFromZero));
                int Position_col_up = Convert.ToInt32(Math.Round(hit.point.z, 0, MidpointRounding.AwayFromZero));

                if (chess.is_selected_piece)
                {
                    // player is draging the piece, light up the location/enemy_piece/teammate_rook if valid_path

                }
                else
                {
                    // player select failed
                    return;
                }
            }
        }
        else
        {
            // mouse is not dragging, light up the teammate piece

        }

    }
    public void MovePieceGameObject(int row, int col, int new_row, int new_col)
    {
        // This function can be bind to chess, then all move will be synchronize with chess.
        Debug.Log("Move " + row.ToString() + " " + col.ToString() + " to " + new_row.ToString() + " " + new_col.ToString());

        GameObject to_move_piece = Piece[GetPictureBoxIndexFromLocation(row, col)];
        int dead_piece_index = GetPictureBoxIndexFromLocation(new_row, new_col);
        Debug.Log(GetPictureBoxIndexFromLocation(row, col));
        Debug.Log(GetPictureBoxIndexFromLocation(new_row, new_col));
        if (dead_piece_index != -1)
        {
            Piece[dead_piece_index].transform.position = new Vector3(100, 100, 100);
        }
        to_move_piece.transform.position = new Vector3(new_row, 0, new_col);
        if (to_move_piece.tag == "white")
        {
            to_move_piece.transform.GetComponent<MeshRenderer>().material.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        }
        else if (to_move_piece.tag == "black")
        {
            to_move_piece.transform.GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.2f, 0.2f, 0.2f);
        }
    }
    public void Display_ValidPath_HintBlocks(bool[,] bool_map)
    {
        for(int i=0; i<8; ++i)
        {
            for(int k=0; k<8; ++k)
            {
                HintBlocks[i, k].GetComponent<Renderer>().enabled = bool_map[i, k];
            }
        }
    }
    public void Clean_ValidPath_HintBlocks()
    {
        {
            for (int i = 0; i < 8; ++i)
            {
                for (int k = 0; k < 8; ++k)
                {
                    HintBlocks[i, k].GetComponent<Renderer>().enabled = false;
                }
            }
        }
    }
    public int temp;
    public int GetPictureBoxIndexFromLocation(int row, int col)
    {
        for (int i = 0; i < 32; ++i)
        {
            if (Piece[i].transform.localPosition.x == row && Piece[i].transform.localPosition.z == col)
            {
                //Debug.Log(i);
                //Console.WriteLine(i);
                return i;
            }
        }
        //Debug.Log("error: GetPBIndexFromChar return -1");
        return -1;
    }
}
