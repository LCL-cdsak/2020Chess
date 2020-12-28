using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using Assets.Script;

public class inicial : MonoBehaviour {
    public int mode;
    // Use this for initialization
    public Chess chess;
    public GameObject debug;
    public GameObject temp_piece;
    public GameObject[] Pieces = new GameObject[32];
    public GameObject[] PieceTypePrefabs = new GameObject[8];
    public Transform HintBlockTransform;
    public Transform[,] HintBlocks = new Transform[8, 8];
    public Vector3[] InitPieceLocations = new Vector3[32];
    ChessAlgorithm algorithm;
    int[] best_path = new int[4];//[0] is fromx,[1] is fromy,[2] is tox,[3] is toy

    public GameObject SelectPieceTypeUI;
    public bool is_selecting_piece_type = false; // this is the flag for waiting user to select the new pawn type.
    void Start()
    {
        algorithm = new ChessAlgorithm();
        chess = new Chess();
        chess.MovePieceEvent += MovePieceGameObject;
        chess.RemovePieceEvent += RemovePieceGameObject;
        
        //debug.transform.localPosition = new Vector3(5f, 0, 0);
        //  Piece = GameObject.FindGameObjectsWithTag("piece");

        /*
            Debug.Log(Piece[15].transform.localPosition.x);
            Debug.Log(Piece[15].transform.localPosition.z);
            //Console.WriteLine();
        */
        GameObject board = GameObject.Find("Board");
        for (int i = 0; i < 8; ++i)
        {
            for (int k = 0; k < 8; ++k)
            {
                HintBlocks[i, k] = Instantiate(HintBlockTransform);
                HintBlocks[i, k].SetParent(board.transform);
                HintBlocks[i, k].localPosition = new Vector3(i, 0, k);
                HintBlocks[i, k].GetComponent<Renderer>().material.color = new Color(0, 1, 0, 0.3f);
                HintBlocks[i, k].GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
                HintBlocks[i, k].GetComponent<Renderer>().enabled = false;
            }
        }
        for(int i=0; i<32; ++i)
        {
            InitPieceLocations[i] = Pieces[i].transform.localPosition;
        }
        ChangePieceType(1, 0, "white", Piece.PieceType.Queen);
        CloseSelectPieceUI();
    }
    public void ShowCloseSelectPieceUI()
    {
        SelectPieceTypeUI.SetActive(true);
    }
    public void CloseSelectPieceUI()
    {
        SelectPieceTypeUI.SetActive(false);
    }

    void ResetGame()
    {
        is_selecting_piece_type = false;
        for(int i=0; i<32; ++i)
        {
            Pieces[i].transform.localPosition = InitPieceLocations[i];
        }
        CloseSelectPieceUI();
    }
    // Update is called once per frame

    int Position_row;
    int Position_col;
    private bool is_deselect = true;
    public bool is_mouse_dragging = false;
    void Update() {
        if (is_selecting_piece_type)
        {
            return;
        }
        mode = Mode_Select.Mode;
        ChessAlgorithm.level = Mode_Select.Level;
        if (Input.GetMouseButtonDown(0))
        {
            is_mouse_dragging = true;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //  Debug.Log(hit.point.x);
                // Debug.Log(hit.point.z);
                Position_row = Convert.ToInt32(Math.Round(hit.point.x * 10, 0, MidpointRounding.AwayFromZero));
                Position_col = Convert.ToInt32(Math.Round(hit.point.z * 10, 0, MidpointRounding.AwayFromZero));
                Debug.Log(Position_row);
                //   Debug.Log()
                // print(Math.Round(Position_row, 0, MidpointRounding.AwayFromZero));
                if (chess.is_selected_piece)
                {
                    if(chess.IsPawnReachBottom(chess.selected_piece_location[0], chess.selected_piece_location[1],
                                               Position_row, Position_col))
                    {
                        // Start Waiting
                        is_selecting_piece_type = true;
                        ShowCloseSelectPieceUI();
                    }
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
                    Change_mode(mode);
                    //單人模式 (100-102行註解即可轉為雙人模式)
                    /*best_path = algorithm.AI(ref chess.map);
                    chess.SelectPiece(best_path[0], best_path[1]);                
                    chess.MovePiece(best_path[2], best_path[3], out is_deselect);*/
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
                else if (hit.transform.IsChildOf(GameObject.Find("Pieces").transform))
                {
                    // select the piece if player valid
                    if (chess.SelectPiece(Position_row, Position_col))
                    {
                        //Console.WriteLine("ssss");

                        temp_piece = Pieces[GetPictureBoxIndexFromLocation(Position_row, Position_col)];
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
                int Position_row_up = Convert.ToInt32(Math.Round(hit.point.x * 10, 0, MidpointRounding.AwayFromZero));
                int Position_col_up = Convert.ToInt32(Math.Round(hit.point.z * 10, 0, MidpointRounding.AwayFromZero));
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
                int Position_row_up = Convert.ToInt32(Math.Round(hit.point.x * 10, 0, MidpointRounding.AwayFromZero));
                int Position_col_up = Convert.ToInt32(Math.Round(hit.point.z * 10, 0, MidpointRounding.AwayFromZero));

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
    public void Change_mode(int m)
    {
        if (m==1)
        {
            best_path = algorithm.AI(ref chess.map);
            chess.SelectPiece(best_path[0], best_path[1]);
            chess.MovePiece(best_path[2], best_path[3], out is_deselect);
        }
    }
    public void MovePieceGameObject(int row, int col, int new_row, int new_col)
    {
        // This function can be bind to chess, then all move will be synchronize with chess.
        Debug.Log("Move " + row.ToString() + " " + col.ToString() + " to " + new_row.ToString() + " " + new_col.ToString());

        GameObject to_move_piece = Pieces[GetPictureBoxIndexFromLocation(row, col)];
        int dead_piece_index = GetPictureBoxIndexFromLocation(new_row, new_col);
        Debug.Log(GetPictureBoxIndexFromLocation(row, col));
        Debug.Log(GetPictureBoxIndexFromLocation(new_row, new_col));
        if (dead_piece_index != -1)
        {
            Pieces[dead_piece_index].transform.localPosition = new Vector3(100, 100, 100);
        }
        to_move_piece.transform.localPosition = new Vector3(new_row, 0, new_col);
        if (to_move_piece.tag == "white")
        {
            to_move_piece.transform.GetComponent<MeshRenderer>().material.color = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        }
        else if (to_move_piece.tag == "black")
        {
            to_move_piece.transform.GetComponent<MeshRenderer>().material.color = new Color(0.2f, 0.2f, 0.2f, 0.2f);
        }
    }

    public void RemovePieceGameObject(int row, int col)
    {
        int index = GetPictureBoxIndexFromLocation(row, col);
        if (index == -1)
            return;
        if (Pieces[index] != null)
        {
            Pieces[index].transform.localPosition = new Vector3(100, 100, 100);
        }
    }
    public void Display_ValidPath_HintBlocks(bool[,] bool_map)
    {
        for (int i = 0; i < 8; ++i)
        {
            for (int k = 0; k < 8; ++k)
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
    void ChangePieceType(int row, int col, string team, Piece.PieceType type)
    {
        int index = 0;
        if (team == "black")
            index += 4;
        switch (type)
        {
            case Piece.PieceType.Queen:
                break;
            case Piece.PieceType.Knight:
                index += 1;
                break;
            case Piece.PieceType.Rook:
                index += 2;
                break;
            case Piece.PieceType.Bishop:
                index += 3;
                break;
        }
        int old_ind = GetPictureBoxIndexFromLocation(row, col);
        GameObject old_obj = Pieces[old_ind];
        
        Pieces[old_ind] = Instantiate(PieceTypePrefabs[index], old_obj.transform.position, 
                                                                       old_obj.transform.rotation, GameObject.Find("Pieces").transform) as GameObject;
        Pieces[old_ind] = GameObject.Find(Pieces[old_ind].name);
        Destroy(old_obj);
        //Pieces[GetPictureBoxIndexFromLocation(row, col)].transform.lossyScale = old_obj.transform.lossyScale;
        //Pieces[GetPictureBoxIndexFromLocation(row, col)].transform.SetParent(GameObject.Find("board").transform);

    }
    public void OnSelectPieceTypeUIClicked(Piece.PieceType type)
    {
        is_selecting_piece_type = false;
        CloseSelectPieceUI();
        int row = chess.selected_piece_location[0], col = chess.selected_piece_location[1];
        ChangePieceType(row, col, chess.map[row, col].team, type);
        chess.MovePawnToBottom(row, col, Position_row, Position_col);

    }
    public int temp;
    public int GetPictureBoxIndexFromLocation(int row, int col)
    {
        for (int i = 0; i < 32; ++i)
        {
            if (Pieces[i].transform.localPosition.x == row && Pieces[i].transform.localPosition.z == col)
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
