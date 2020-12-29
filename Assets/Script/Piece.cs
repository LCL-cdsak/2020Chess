using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script
{
    public class Piece
    {
        public PieceType piece_type;
        public string team;
        public bool[,] valid_path = new bool[8, 8];
        public bool[,] protect_path = null;

        public static readonly int[,] king_offsets = { { -1,-1}, {-1,0 }, { -1,1},
                                         { 0,-1}, {0, 1},
                                         { 1,-1}, {1, 0}, {1,1} };

        public static readonly int[,] cross_vectors = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
        public static readonly int[,] diagonal_vectors = new int[,] { { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };

        public Piece(string _team, string piece_type_name)
        {
           // if (!Enum.TryParse(piece_type_name, true, out piece_type))
                // no match Enum type
                //Console.WriteLine("bug");
            team = _team;
        }
        public Piece(string _team, PieceType _piece_type)
        {
            piece_type = _piece_type;
            team = _team;
        }
        public void RoundInitialize()
        {
            protect_path = null;
            valid_path = null;
        }


        public bool[,] ValidPath(int row, int col, Piece[,] now_map)
        {
            bool[,] bool_map = new bool[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    bool_map[i, j] = false;
                }
            }

            switch (piece_type)
            {
                case PieceType.Pawn:
                    if (team == "black")
                    {
                        if (row + 1 < 8)
                        {
                            if (now_map[row + 1, col] == null)
                            {
                                bool_map[row + 1, col] = true;
                            }
                            if (col + 1 < 8)
                            {
                                if (now_map[row + 1, col + 1] != null)
                                {
                                    if (now_map[row + 1, col + 1].team == "white") bool_map[row + 1, col + 1] = true;
                                }
                            }
                            if (col - 1 >= 0)
                            {
                                if (now_map[row + 1, col - 1] != null)
                                {
                                    if (now_map[row + 1, col - 1].team == "white") bool_map[row + 1, col - 1] = true;
                                }
                            }
                        }
                        if (row == 1 && now_map[row + 2, col] == null && now_map[row + 1, col]==null)
                        {
                            bool_map[row + 2, col] = true;
                        }
                        break;
                    }
                    else if (team == "white")
                    {
                        if (row - 1 >= 0)
                        {
                            if (now_map[row - 1, col] == null)
                            {
                                bool_map[row - 1, col] = true;
                            }
                            if (col + 1 < 8)
                            {
                                if (now_map[row - 1, col + 1] != null)
                                {
                                    if (now_map[row - 1, col + 1].team == "black") bool_map[row - 1, col + 1] = true;
                                }
                            }
                            if (col - 1 >= 0)
                            {
                                if (now_map[row - 1, col - 1] != null)
                                {
                                    if (now_map[row - 1, col - 1].team == "black") bool_map[row - 1, col - 1] = true;
                                }
                            }
                        }
                        if (row == 6 && now_map[row - 2, col] == null && now_map[row - 1, col] == null)
                        {
                            bool_map[row - 2, col] = true;
                        }
                        break;
                    }
                    break;

                case PieceType.King:


                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            if (row + i >= 0 && row + i < 8 && col + j >= 0 && col + j < 8)
                            {
                                if (now_map[row + i, col + j] == null)
                                {
                                    bool_map[row + i, col + j] = true;
                                }
                                else
                                {
                                    if (now_map[row + i, col + j].team != team) { bool_map[row + i, col + j] = true; }

                                }
                            }
                        }
                    }
                    break;

                case PieceType.Queen:
                    Diagonal_path(row, col, now_map, bool_map);
                    Cross_path(row, col, now_map, bool_map);
                    break;

                case PieceType.Bishop:
                    Diagonal_path(row, col, now_map, bool_map);
                    break;

                case PieceType.Rook:
                    Cross_path(row, col, now_map, bool_map);
                    break;

                case PieceType.Knight:

                    if (row - 2 >= 0 && col - 1 >= 0)
                    {
                        if (now_map[row - 2, col - 1] == null) bool_map[row - 2, col - 1] = true;
                        else
                        {
                            if (now_map[row - 2, col - 1].team != team) bool_map[row - 2, col - 1] = true; ;
                        }
                    }
                    if (row - 1 >= 0 && col - 2 >= 0)
                    {
                        if (now_map[row - 1, col - 2] == null) bool_map[row - 1, col - 2] = true;
                        else
                        {
                            if (now_map[row - 1, col - 2].team != team) bool_map[row - 1, col - 2] = true; ;
                        }
                    }
                    if (row + 1 < 8 && col - 2 >= 0)
                    {
                        if (now_map[row + 1, col - 2] == null) bool_map[row + 1, col - 2] = true;
                        else
                        {
                            if (now_map[row + 1, col - 2].team != team) bool_map[row + 1, col - 2] = true;
                        }
                    }
                    if (row + 2 < 8 && col - 1 >= 0)
                    {
                        if (now_map[row + 2, col - 1] == null) bool_map[row + 2, col - 1] = true;
                        else
                        {
                            if (now_map[row + 2, col - 1].team != team) bool_map[row + 2, col - 1] = true;
                        }
                    }
                    if (row + 2 < 8 && col + 1 < 8)
                    {
                        if (now_map[row + 2, col + 1] == null) bool_map[row + 2, col + 1] = true;
                        else
                        {
                            if (now_map[row + 2, col + 1].team != team) bool_map[row + 2, col + 1] = true;
                        }
                    }
                    if (row + 1 < 8 && col + 2 < 8)
                    {
                        if (now_map[row + 1, col + 2] == null) bool_map[row + 1, col + 2] = true;
                        else
                        {
                            if (now_map[row + 1, col + 2].team != team) bool_map[row + 1, col + 2] = true;
                        }
                    }
                    if (row - 1 >= 0 && col + 2 < 8)
                    {
                        if (now_map[row - 1, col + 2] == null) bool_map[row - 1, col + 2] = true;
                        else
                        {
                            if (now_map[row - 1, col + 2].team != team) bool_map[row - 1, col + 2] = true;
                        }
                    }
                    if (row - 2 >= 0 && col + 1 < 8)
                    {
                        if (now_map[row - 2, col + 1] == null) bool_map[row - 2, col + 1] = true;
                        else
                        {
                            if (now_map[row - 2, col + 1].team != team) bool_map[row - 2, col + 1] = true;
                        }
                    }


                    break;
            }
            return bool_map;
        }
        private void Cross_path(int row, int col, Piece[,] now_map, bool[,] bool_map)//判斷十字路徑
        {
            // only consider the valid path for this piece (no king detection)
            for (int i = 1; i < 8; i++)
            {
                if (col + i < 8)
                {
                    if (now_map[row, col + i] == null)
                    {
                        bool_map[row, col + i] = true;
                    }
                    else
                    {
                        if (now_map[row, col + i].team != team)
                        {
                            bool_map[row, col + i] = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (col - i >= 0)
                {
                    if (now_map[row, col - i] == null)
                    {
                        bool_map[row, col - i] = true;
                    }
                    else
                    {
                        if (now_map[row, col - i].team != team)
                        {
                            bool_map[row, col - i] = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (row - i >= 0)
                {
                    if (now_map[row - i, col] == null)
                    {
                        bool_map[row - i, col] = true;
                    }
                    else
                    {
                        if (now_map[row - i, col].team != team)
                        {
                            bool_map[row - i, col] = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (row + i < 8)
                {
                    if (now_map[row + i, col] == null)
                    {
                        bool_map[row + i, col] = true;
                    }
                    else
                    {
                        if (now_map[row + i, col].team != team)
                        {
                            bool_map[row + i, col] = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }


            return;
        }
        public void Diagonal_path(int row, int col, Piece[,] now_map, bool[,] bool_map)//判斷對角路徑
        {
            for (int i = 1; i < 8; i++)
            {
                if (row + i < 8 && col + i < 8)
                {
                    if (now_map[row + i, col + i] == null)
                    {
                        bool_map[row + i, col + i] = true;
                    }
                    else
                    {
                        if (now_map[row + i, col + i].team != team)
                        {
                            bool_map[row + i, col + i] = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (row - i >= 0 && col + i < 8)
                {
                    if (now_map[row - i, col + i] == null)
                    {
                        bool_map[row - i, col + i] = true;
                    }
                    else
                    {
                        if (now_map[row - i, col + i].team != team)
                        {
                            bool_map[row - i, col + i] = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (row + i < 8 && col - i >= 0)
                {
                    if (now_map[row + i, col - i] == null)
                    {
                        bool_map[row + i, col - i] = true;
                    }
                    else
                    {
                        if (now_map[row + i, col - i].team != team)
                        {
                            bool_map[row + i, col - i] = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            for (int i = 1; i < 8; i++)
            {
                if (row - i >= 0 && col - i >= 0)
                {
                    if (now_map[row - i, col - i] == null)
                    {
                        bool_map[row - i, col - i] = true;
                    }
                    else
                    {
                        if (now_map[row - i, col - i].team != team)
                        {
                            bool_map[row - i, col - i] = true;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        /*
         * 白方:
         * p:士兵 
         * r:城堡
         * h:騎士 
         * b:主教 
         * q:皇后 
         * k:國王 
         * 黑方:
         * P:士兵
         * R:城堡
         * H:騎士
         * B:主教
         * Q:皇后
         * K:國王 
         * 
         * N:空格
         */
        public Piece Thread_path(int row, int col, Piece[,] now_map, ref bool is_check, ref bool[,] check_path)
        {
            // This function will return the protect_piece,
            // ref bool[,] is set when the king is checked.
            Piece protect_piece = null;
            int[,] offsets = null;
            bool[,] thread_path = null;
            bool temp_is_check;
            switch (piece_type)
            {
                case PieceType.Pawn:
                    if (team == "white")
                    {
                        if (row - 1 >= 0)
                        {
                            if (col - 1 >= 0)
                            {
                                if (now_map[row - 1, col - 1] != null)
                                    if (now_map[row - 1, col - 1].team != team && now_map[row - 1, col - 1].piece_type == PieceType.King)
                                    {
                                        thread_path = new bool[8, 8];
                                        thread_path[row, col] = true; // only self location is true
                                        is_check = true;
                                        check_path = thread_path;
                                        return null;
                                    }
                            }
                            if (col + 1 < 8)
                            {
                                if (now_map[row - 1, col + 1] != null)
                                    if (now_map[row - 1, col + 1].team != team && now_map[row - 1, col + 1].piece_type == PieceType.King)
                                    {
                                        thread_path = new bool[8, 8];
                                        thread_path[row, col] = true;
                                        is_check = true;
                                        check_path = thread_path;
                                        return null;
                                    }
                            }
                        }
                    }
                    else
                    {
                        if (row + 1 < 8)
                        {
                            if (col - 1 >= 0)
                            {
                                if (now_map[row + 1, col - 1] != null)
                                    if (now_map[row + 1, col - 1].team != team && now_map[row + 1, col - 1].piece_type == PieceType.King)
                                    {
                                        thread_path = new bool[8, 8];
                                        thread_path[row, col] = true;
                                        is_check = true;
                                        check_path = thread_path;
                                        return null;
                                    }
                            }
                            if (col + 1 < 8)
                            {
                                if (now_map[row + 1, col + 1] != null)
                                    if (now_map[row + 1, col + 1].team != team && now_map[row + 1, col + 1].piece_type == PieceType.King)
                                    {
                                        thread_path = new bool[8, 8];
                                        thread_path[row, col] = true;
                                        is_check = true;
                                        check_path = thread_path;
                                        return null;
                                    }
                            }
                        }
                    }
                    break;
                case PieceType.King:
                    // This condition should not happend, no King will close to the other king.
                    /*offsets = new int[,] { { -1,-1}, {-1,0 }, { -1,1},
                                       { 0,-1}, {0, 1},
                                       { 1,-1}, {1, 0}, {1,1} };*/
                    return null;
                //break;
                case PieceType.Queen:
                    thread_path = Thread_Cross_Diagonal_path(false, row, col, now_map, out temp_is_check, out protect_piece);
                    if (thread_path != null)
                    {
                        if (temp_is_check)
                        {
                            is_check = true;
                            check_path = thread_path;
                            return null;
                        }
                        else
                        {
                            protect_piece.protect_path = thread_path;
                            return protect_piece;
                        }
                        
                    }
                    

                    thread_path = Thread_Cross_Diagonal_path(true, row, col, now_map, out temp_is_check, out protect_piece);
                    if (thread_path != null)
                    {
                        if (temp_is_check)
                        {
                            is_check = true;
                            check_path = thread_path;
                            return null;
                        }
                        else
                        {
                            protect_piece.protect_path = thread_path;
                            return protect_piece;
                        }
                    }
                    break;
                case PieceType.Bishop:
                    thread_path = Thread_Cross_Diagonal_path(true, row, col, now_map, out temp_is_check, out protect_piece);
                    if (thread_path != null)
                    {
                        if (temp_is_check)
                        {
                            is_check = true;
                            check_path = thread_path;
                        }
                        else
                            protect_piece.protect_path = thread_path;
                    }
                    break;
                case PieceType.Rook:
                    thread_path = Thread_Cross_Diagonal_path(false, row, col, now_map, out temp_is_check, out protect_piece);
                    if (thread_path != null)
                    {
                        if (temp_is_check)
                        {
                            is_check = true;
                            check_path = thread_path;
                        }
                        else
                            protect_piece.protect_path = thread_path;
                    }
                    break;
                case PieceType.Knight:
                    offsets = new int[,]{ {-2, -1}, {-2, 1}, {-1, -2}, {-1, 2},
                                          {1, -2}, {1, 2}, {2, -1}, {2, 1}};
                    int irow, icol;
                    for (int i = 0; i < 8; ++i)
                    {
                        irow = row + offsets[i, 0];
                        icol = col + offsets[i, 1];
                        if (irow >= 0 && irow < 8 && icol >= 0 && icol < 8)
                        {
                            if (now_map[irow, icol] != null)
                            {
                                if (now_map[irow, icol].team != team)
                                {
                                    if (now_map[irow, icol].piece_type == PieceType.King)
                                    {
                                        thread_path = new bool[8, 8];
                                        thread_path[row, col] = true;
                                        is_check = true;
                                        check_path = thread_path;
                                        return null;
                                    }
                                }

                            }

                        }

                    }
                    break;
            }

            return protect_piece;
        }
        public bool[,] Thread_Cross_Diagonal_path(bool is_diagonal, int row, int col, Piece[,] now_map, out bool is_check, out Piece protect_piece)
        {
            // Calculate and return "thread path"
            // the is_check is temp for Chess, not the game's is_check, just this single piece.
            // Check or Protect will have return value.
            // if is_diagonal is false, do cross_path
            is_check = false; // if the king get "direct" check.
            protect_piece = null;

            bool is_crossed;
            int[,] vectors;
            if (is_diagonal)
            {
                vectors = diagonal_vectors;
            }
            else
            {
                vectors = cross_vectors;
            }

            int irow, icol;
            for (int i = 0; i < 4; ++i)
            {
                is_crossed = false;
                irow = row;
                icol = col;
                for (int k = 0; k < 7; ++k)
                {
                    irow += vectors[i, 0];
                    icol += vectors[i, 1];
                    if (0 <= irow && irow < 8 && 0 <= icol && icol < 8)
                    {
                        if (now_map[irow, icol] != null)
                        {
                            if (now_map[irow, icol].team == team)
                            {
                                break;
                            }

                            if (is_crossed)
                            {
                                if (now_map[irow, icol].piece_type == PieceType.King)
                                {
                                    // is_check remain false, just assign thread_map

                                    return BackTrackThreadMap(k, vectors[i, 0], vectors[i, 1], irow, icol);
                                }
                                else
                                {
                                    // this path is not threading king
                                    break;
                                }
                            }
                            else
                            {
                                if (now_map[irow, icol].piece_type == PieceType.King)
                                {
                                    // direct check king.
                                    is_check = true;
                                    // backtrack to form thread path
                                    return BackTrackThreadMap(k, vectors[i, 0], vectors[i, 1], irow, icol); ;
                                }
                                else
                                {
                                    is_crossed = true;
                                    protect_piece = now_map[irow, icol];
                                }
                            }
                        }
                    }
                }
            }
            protect_piece = null;
            return null;
        }
        public bool[,] BackTrackThreadMap(int k, int r_vec, int c_vec, int irow, int icol)
        {
            bool[,] thread_map = new bool[8, 8];
            for (; k >= 0; --k)
            {
                irow -= r_vec;
                icol -= c_vec;
                thread_map[irow, icol] = true;
            }
            return thread_map;
        }
        public void Team_path(int row, int col, Piece[,] now_map, bool[,] all_team_path)
        {
            int[,] offsets = null;
            int irow, icol;
            switch (piece_type)
            {
                case PieceType.Pawn:
                    if (team == "white")
                    {
                        if (row - 1 >= 0)
                        {
                            if (col - 1 >= 0)
                            {
                                all_team_path[row - 1, col - 1] = true;
                            }
                            if (col + 1 < 8)
                            {
                                all_team_path[row - 1, col + 1] = true;
                            }
                        }
                    }
                    else
                    {
                        if (row + 1 < 8)
                        {
                            if (col - 1 >= 0)
                            {
                                all_team_path[row + 1, col - 1] = true;
                            }
                            if (col + 1 < 8)
                            {
                                all_team_path[row + 1, col + 1] = true;
                            }
                        }
                    }
                    break;
                case PieceType.King:
                    offsets = new int[,] { { -1,-1}, {-1,0 }, { -1,1},
                                       { 0,-1}, {0, 1},
                                       { 1,-1}, {1, 0}, {1,1} };

                    for (int i = 0; i < 8; ++i)
                    {
                        irow = row + offsets[i, 0];
                        icol = col + offsets[i, 1];
                        if (0 <= irow && irow < 8 && 0 <= icol && icol < 8)
                            all_team_path[irow, icol] = true;
                    }
                    break;
                case PieceType.Queen:
                    Team_Cross_Diagonal_path(false, row, col, now_map, all_team_path);
                    Team_Cross_Diagonal_path(true, row, col, now_map, all_team_path);
                    break;
                case PieceType.Bishop:
                    Team_Cross_Diagonal_path(true, row, col, now_map, all_team_path);
                    break;
                case PieceType.Rook:
                    Team_Cross_Diagonal_path(false, row, col, now_map, all_team_path);
                    break;
                case PieceType.Knight:
                    offsets = new int[,]{ {-2, -1}, {-2, 1}, {-1, -2}, {-1, 2},
                                          {1, -2}, {1, 2}, {2, -1}, {2, 1}};
                    for (int i = 0; i < 8; ++i)
                    {
                        irow = row + offsets[i, 0];
                        icol = col + offsets[i, 1];
                        if (0 <= irow && irow < 8 && 0 <= icol && icol < 8)
                            all_team_path[irow, icol] = true;
                    }
                    break;
            }
        }
        public void Team_Cross_Diagonal_path(bool is_diagonal, int row, int col, Piece[,] now_map, bool[,] all_team_path)
        {
            int[,] vectors;
            if (is_diagonal)
            {
                vectors = diagonal_vectors;
            }
            else
            {
                vectors = cross_vectors;
            }

            int irow, icol;
            for (int i = 0; i < 4; ++i)
            {
                irow = row;
                icol = col;
                for (int k = 0; k < 7; ++k)
                {
                    irow += vectors[i, 0];
                    icol += vectors[i, 1];
                    if (0 <= irow && irow < 8 && 0 <= icol && icol < 8)
                    {
                        if (now_map[irow, icol] == null)
                            all_team_path[irow, icol] = true;
                        else
                        {
                            all_team_path[irow, icol] = true;
                            break;
                        }
                    }
                }
            }
        }
        public static PieceType PieceTypeFromString(string str)
        {
            return (PieceType)Enum.Parse(typeof(PieceType), str, true);
        }
        public static string PieceToImagePath(Piece piece)
        {
            switch (piece.team)
            {
                case "white":
                    return "w" + piece.piece_type.ToString() + ".png";
                case "black":
                    return "b" + piece.piece_type.ToString() + ".png";
                default:
                    return "NULL";
            }

        }
        public static Piece PieceFromChar(char c)
        {
            if (char.IsUpper(c))
            {
                return new Piece("white", (PieceType)System.Enum.ToObject(typeof(PieceType), char.ToUpper(c)));
            }
            else
            {
                return new Piece("black", (PieceType)System.Enum.ToObject(typeof(PieceType), char.ToUpper(c)));
            }

        }

        public enum PieceType
        {
            Pawn = 'P',
            Rook = 'R',
            Knight = 'H',
            Bishop = 'B',
            Queen = 'Q',
            King = 'K'
        }
    }
}

