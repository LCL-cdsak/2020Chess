using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script
{
    public class Step {
        public int row, col, nrow, ncol;
        public string team;
        public Piece.PieceType type;
        public Piece.PieceType? killed;
        public bool white_castling, black_castling;
        public Step(int _row, int _col, int _nrow, int _ncol, Piece[,] map)
        {
            row = _row;
            col = _col;
            nrow = _nrow;
            ncol = _ncol;
            type = map[row, col].piece_type;
            if (map[nrow, ncol] == null)
                killed = null;
            else 
                killed = map[nrow, ncol].piece_type;
            team = map[row, col].team;
        }
    }
    
}