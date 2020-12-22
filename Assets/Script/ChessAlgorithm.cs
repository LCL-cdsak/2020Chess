using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using Assets.Script;
namespace Assets.Script
{
    class ChessAlgorithm
    {
        public int fromx, fromy, tox, toy;
        public static int level;
        public Chess chess;
        /*
         * score:
         * pawn = 10
         * knight = 40
         * bishop = 50(if two bishops are alive,both plus 0.5)
         * rook = 60
         * queen = 110
         * king = 10000
         */
        public int[,] piece_score_table_king = new int[8, 8] { { +20, +30, +10, +00, +00, +10, +30, +20 },
                                                               { +20, +20, +00, +00, +00, +00, +20, +20 },
                                                               { -10, -20, -20, -20, -20, -20, -20, -10 },
                                                               { -20, -30, -30, -40, -40, -30, -30, -20 },
                                                               { -30, -40, -40, -50, -50, -40, -40, -30 },
                                                               { -30, -40, -40, -50, -50, -40, -40, -30 },
                                                               { -30, -40, -40, -50, -50, -40, -40, -30 },
                                                               { -30, -40, -40, -50, -50, -40, -40, -30 }};
        public int[,] piece_score_table_queen = new int[8, 8] {{ -20, -10, -10, -5, -5, -10, -10, -20 },
                                                               { -10,   0,   5,   0, 0,   0,   0, -10 },
                                                               { -10,   5,   5,   5, 5,   5,   0, -10 },
                                                               {   0,   0,   5,   5, 5,   5,   0, -5  },
                                                               {  -5,   0,   5,   5, 5,   5,   0, -5  },
                                                               { -10,   0,   5,   5, 5,   5,   0, -10 },
                                                               { -10,   0,   0,   0, 0,   0,   0, -10 },
                                                               { -20, -10, -10,  -5,-5, -10, -10, -20 }};
        public int[,] piece_score_table_rook = new int[8, 8] { {  0, 0, 0, 5, 5, 0, 0, 0 },
                                                               { -5, 0, 0, 0, 0, 0, 0,-5 },
                                                               { -5, 0, 0, 0, 0, 0, 0,-5 },
                                                               { -5, 0, 0, 0, 0, 0, 0,-5 },
                                                               { -5, 0, 0, 0, 0, 0, 0,-5 },
                                                               { -5, 0, 0, 0, 0, 0, 0,-5 },
                                                               {  5, 1, 1, 1, 1, 1, 1, 5 },
                                                               {  0, 0, 0, 0, 0, 0, 0, 0} };
        public int[,] piece_score_table_bishop = new int[8, 8] {{ -20, -10, -10, -10, -10, -10, -10, -20 },
                                                                { -10,   5,   0,   0,   0,   0,   5, -10 },
                                                                { -10,  10,  10,  10,  10,  10,  10, -10 },
                                                                { -10,   0,  10,  10,  10,  10,   0, -10 },
                                                                { -10,   5,   5,  10,  10,   5,   5, -10 },
                                                                { -10,   0,   5,  10,  10,   5,   0, -10 },
                                                                { -10,   0,   0,   0,   0,   0,   0, -10 },
                                                                { -20, -10, -10, -10, -10, -10, -10, -20 }};

        public int[,] piece_score_table_knight = new int[8, 8] {{ -50, -20, -30, -30, -30, -30, -20, -50 },
                                                               {  -40, -20, +00,  +5,  +5, +00, -20, -40 },
                                                               {  -30,  +5, +10, +15, +15, +10,  +5, -30 },
                                                               {  -30, +00, +15, +20, +20, +15, +00, -30 },
                                                               {  -30, +5, +15, +20, +20,  +15,  +5, -30 },
                                                               {  -30, +00, +10, +15, +15, +10, +00, -30 },
                                                               {  -40, -20, +00, +00, +00, +00, -20, -40 },
                                                               {  -50, -40, -30, -30, -30, -40, -40, -50 }};

        public int[,] piece_score_table_pawn = new int[8, 8] { { 0,  0,   0,   0,   0,   0,  0, 0 },
                                                               { 5, 10, -10, -20, -20, -10, 10, 5 },
                                                               { 5,  5,   5,   0,   0,   5,  5, 5 },
                                                               { 0,  0,   0,  20,  20,   0,  0, 0 },
                                                               { 5,  5,  10,  25,  25,  10,  5, 5 },
                                                               {10, 10,  20,  30,  30,  20, 10, 10},
                                                               {50, 50,  50,  50,  50,  50, 50, 50},
                                                               { 0,  0,   0,   0,   0,   0,  0, 0 }};

        public int minmax_first(ref Piece[,] current, int layer, int max_layer, ref int fromx, ref int fromy, ref int tox, ref int toy)//current is map status，return point of a match
        {
            int highest_score_of_a_match = int.MinValue;
            int x = int.MinValue;
            if (layer < 0)
                return 0;
            Piece temp;
            Piece temp2;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    //for loop all valid path，calculate the points(max and min)
                    if (current[i, j] == null)
                        continue;
                    else if (current[i, j].team == "black")//set com as black temporary
                    {
                        bool[,] paths = current[i, j].ValidPath(i, j, current);//get all valid paths of piece[i,j]                       
                        for (int m = 0; m < 8; m++)
                            for (int n = 0; n < 8; n++)
                                if (paths[m, n] == true)
                                {
                                    //calculate points and recur to next level
                                    int temp_score = calculate_point(ref current, m, n);//calculate how many score can get if go to [m,n]
                                    int temp_score_2 = int.MinValue;
                                    //Console.WriteLine(temp_score);
                                    //*******moving piece from [i,j] to [m,n]*******
                                    temp = current[m, n];
                                    current[m, n] = current[i, j];
                                    current[i, j] = null;
                                    //**********************************************
                                    for (int a = 0; a < 8; a++)
                                        for (int b = 0; b < 8; b++)
                                        {
                                            if (current[a, b] == null)
                                                continue;
                                            else if (current[a, b].team == "white")
                                            {
                                                bool[,] enemy_paths = current[a, b].ValidPath(a, b, current);
                                                for (int c = 0; c < 8; c++)
                                                    for (int d = 0; d < 8; d++)
                                                        if (enemy_paths[c, d])
                                                        {
                                                            if (temp_score_2 > calculate_point(ref current, c, d))
                                                                continue;
                                                            else
                                                            {
                                                                temp_score_2 = calculate_point(ref current, c, d);
                                                                int z = temp_score - temp_score_2;//此局分數
                                                                //*******moving piece from [a,b] to [c,d]*******
                                                                temp2 = current[c, d];
                                                                current[c, d] = current[a, b];
                                                                current[a, b] = null;
                                                                //**********************************************
                                                                int y = minmax_first(ref current, layer - 1, max_layer, ref fromx, ref fromy, ref tox, ref toy);
                                                                x = x > y ? x : y;//選出子棋局最大的分數，紀錄在x裡
                                                                if (highest_score_of_a_match < (x + z))
                                                                {
                                                                    highest_score_of_a_match = x + z;
                                                                    if (layer == max_layer)
                                                                    {
                                                                        fromx = i;
                                                                        fromy = j;
                                                                        tox = m;
                                                                        toy = n;
                                                                    }
                                                                }
                                                                //*******recover map to initial status**********
                                                                current[a, b] = current[c, d];
                                                                current[c, d] = temp2;
                                                                //**********************************************
                                                            }
                                                        }
                                            }
                                        }
                                    //*******recover map to initial status**********
                                    current[i, j] = current[m, n];
                                    current[m, n] = temp;
                                    //**********************************************
                                }
                    }
                    else
                        continue;
                }
            return highest_score_of_a_match;
        }
        public int calculate_point(ref Piece[,] current, int i, int j)
        {
            if (current[i, j] == null)
            {
                return 0;
            }
            else
            {
                if (current[i, j].team == "white")
                {
                    switch (current[i, j].piece_type)
                    {
                        case Piece.PieceType.Queen:
                            return 900;
                        case Piece.PieceType.Rook:
                            return 500;
                        case Piece.PieceType.Pawn:
                            return 100;
                        case Piece.PieceType.Knight:
                            return 300;
                        case Piece.PieceType.Bishop:
                            return 300;
                        case Piece.PieceType.King:
                            return 9000;
                        default:
                            return 0;
                    }
                }
                else
                {
                    switch (current[i, j].piece_type)
                    {
                        case Piece.PieceType.Queen:
                            return 900;
                        case Piece.PieceType.Rook:
                            return 500;
                        case Piece.PieceType.Pawn:
                            return 100;
                        case Piece.PieceType.Knight:
                            return 300;
                        case Piece.PieceType.Bishop:
                            return 300;
                        case Piece.PieceType.King:
                            return 9000;
                        default:
                            return 0;
                    }
                }
            }
        }
        public int table_score(ref Piece[,] current, int i, int j, int m, int n)//把棋子從i,j移到m,n能得到多少地圖分數
        {
            switch (current[i, j].piece_type)
            {
                case Piece.PieceType.Queen:
                    return piece_score_table_queen[m, n] - piece_score_table_queen[i, j];
                case Piece.PieceType.Rook:
                    return piece_score_table_queen[m, n] - piece_score_table_rook[i, j];
                case Piece.PieceType.Pawn:
                    return piece_score_table_queen[m, n] - piece_score_table_pawn[i, j];
                case Piece.PieceType.Knight:
                    return piece_score_table_queen[m, n] - piece_score_table_knight[i, j];
                case Piece.PieceType.Bishop:
                    return piece_score_table_queen[m, n] - piece_score_table_bishop[i, j];
                case Piece.PieceType.King:
                    return piece_score_table_queen[m, n] - piece_score_table_king[i, j];
                default:
                    return 0;
            }
        }
        //alpha、beta值用來記錄當前棋局最高分，不必要的就不需要遞迴下去
        public int minmax(int point, string status, ref Piece[,] current, int layer, int max_layer, int alpha, int beta)//current is map status，return point of a match
        {

            int value = 0;
            if (layer == 0)
                return point;
            if (status == "black")
            {
                value = int.MinValue;
                Piece temp;
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (current[i, j] == null || current[i, j].team == "white")
                            continue;
                        else if (current[i, j].team == "black")
                        {
                            bool[,] path = current[i, j].ValidPath(i, j, current);
                            for (int m = 0; m < 8; m++)
                                for (int n = 0; n < 8; n++)
                                    if (path[m, n])
                                    {
                                        int k = calculate_point(ref current, m, n) + table_score(ref current, i, j, m, n);
                                        //move piece from i,j to m,n
                                        temp = current[m, n];
                                        current[m, n] = current[i, j];
                                        current[i, j] = null;

                                        int c = minmax(point + k, "white", ref current, layer, max_layer, value, beta);//遞迴得到的值與beta比較，若得到比beta還大的值，則不需繼續遞迴下去了
                                        if (c >= value)
                                        {
                                            if (max_layer == layer)
                                            {
                                                fromx = i;
                                                fromy = j;
                                                tox = m;
                                                toy = n;
                                            }
                                            value = c;
                                        }
                                        if (c > beta)
                                        {
                                            current[i, j] = current[m, n];
                                            current[m, n] = temp;
                                            return value;
                                        }
                                        //回復原盤面
                                        current[i, j] = current[m, n];
                                        current[m, n] = temp;
                                    }
                        }
            }
            else if (status == "white")
            {
                value = int.MaxValue;
                Piece temp;
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (current[i, j] == null || current[i, j].team == "black")
                            continue;
                        else
                        {
                            bool[,] path = current[i, j].ValidPath(i, j, current);
                            for (int m = 0; m < 8; m++)
                                for (int n = 0; n < 8; n++)
                                    if (path[m, n])
                                    {
                                        int k = calculate_point(ref current, m, n);
                                        //move piece from i,j to m,n
                                        temp = current[m, n];
                                        current[m, n] = current[i, j];
                                        current[i, j] = null;
                                        int c = minmax(point - k, "black", ref current, layer - 1, max_layer, alpha, value);//value值要做為beta傳給下一層做判斷
                                                                                                                            //遞迴得到的值與alpha比較，若得到比alpha還小的值，則不需繼續遞迴下去了
                                                                                                                            //(因為min層必定取遞迴最小的回傳值，而此層的上一層是max層，一定會取最大的值，也就不會取到此層的值了))        

                                        if (c < value)
                                            value = c;//選分數最低的
                                        if (c < alpha)
                                        {
                                            current[i, j] = current[m, n];
                                            current[m, n] = temp;
                                            return value;
                                        }
                                        //回復原盤面
                                        current[i, j] = current[m, n];
                                        current[m, n] = temp;
                                    }
                        }
            }
            return value;
        }
        public int[] AI(ref Piece[,] map)
        {
            int[] best_path = new int[4];
            fromx = 0;
            fromy = 0;
            tox = 0;
            toy = 0;
            minmax(0, "black", ref map, level, level, int.MinValue, int.MaxValue);
            //bool is_deselect;
            //chess.is_selected_piece = false;
            //chess.SelectPiece(fromx, fromy);
            //chess.MovePiece(tox, toy, out is_deselect);  
            //map[tox, toy] = map[fromx, fromy];
            //map[fromx, fromy] = null;
            best_path[0] = fromx;
            best_path[1] = fromy;
            best_path[2] = tox;
            best_path[3] = toy;
            return best_path;
        }
    }
}