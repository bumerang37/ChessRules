using System.Collections.Generic;
using System.Runtime.Versioning;

namespace ChessRules
{
    public class Chess
    {
        public string fen
        {
            get
            {
                return board.fen;
            }
        }
        
        public bool IsCheck { get; private set; }
        public bool IsCheckmate { get; private set; }
        public bool IsStalemate { get; private set; }

        Board board;
        Moves moves;
        
        public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            board = new Board(fen);
            moves = new Moves(board);
        }

        Chess(Board board)
        {
            this.board = board;
            moves = new Moves(board);
            SetCheckFlags();
        }

        void SetCheckFlags()
        {
            IsCheck = board.IsCheck();
            IsCheckmate = false;
            IsStalemate = false;
            foreach (string moves in YieldValidMoves())
                return;
            if (IsCheck)
                IsCheckmate = true;
            else
                IsStalemate = true;
        }

        public Chess Move(string move)
        {
            FigureMoving fm = new FigureMoving(move);
            if (!moves.CanMove(fm))
                return this;

            if (board.IsCheckAfter(fm))
                return this;
            Board nextBoard = board.Move(fm);
            Chess nextChess = new Chess(nextBoard);
            return nextChess;
        }

        public char GetFigureAt(int x, int y)
        {
            Square square = new Square(x, y);
            Figure figure = board.GetFigureAt(square);
            return figure == Figure.none ? '.' : (char) figure;
        }
        
        public char GetFigureAt(string xy)
        {
            Square square = new Square(xy);
            Figure figure = board.GetFigureAt(square);
            return figure == Figure.none ? '.' : (char) figure;
        }

        public IEnumerable<string> YieldValidMoves()
        {
            foreach (FigureOnSquare fs in board.YieldMyFigureSquares())
                foreach (Square to in Square.YieldBoardSquares())
                    foreach (Figure promotion in fs.figure.YieldPromotions(to))
                    {
                        FigureMoving fm = new FigureMoving(fs, to, promotion);
                        if (moves.CanMove(fm))
                            if (!board.IsCheckAfter (fm))
                                yield return fm.ToString();
                    }
        }
    }
}
