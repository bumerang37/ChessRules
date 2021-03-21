using System.Text;

namespace ChessRules
{
    public class Board
    {
        // ReSharper disable once InconsistentNaming
        public string fen { get; private set; }
        Figure[,] figures;
        // ReSharper disable once InconsistentNaming
        public Color moveColor { get; private set; }
        public bool canCastleA1 { get; private set; } // Q
        public bool canCastleH1 { get; private set; } // K
        public bool canCastleA8 { get; private set; } // q
        public bool canCastleH8 { get; private set; } // k
        public Square enpassant { get; private set; }
        public int drawNumber { get; private set; }
        public int moveNumber { get; private set; }

        public Board(string fen)
        {
            this.fen = fen;
            figures = new Figure[8, 8];
            Init();
        }

        public Board Move(FigureMoving fm)
        {
            Board next = new Board(fen);
            next.SetFiguresAt(fm.from, Figure.none);
            next.SetFiguresAt(fm.to, fm.figure);
            if (moveColor == Color.black)
                next.moveNumber++;
            next.moveColor = moveColor.FlipColor();
            next.GenerateFEN();
            return next;
        }

        void GenerateFEN()
        {
            this.fen = FenFigures() + " " +
                       FenMoveColor() + " " +
                       FenCastleFlags() + " " +
                       FenEnpassant() + " " +
                       FenDrawNumber() + " " +
                       FenMoveNumber();
        }
        private string FenFigures()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                    sb.Append(figures[x, y] == Figure.none
                        ? '1'
                        : (char)figures[x, y]);
                if (y > 0)
                    sb.Append("/");
            }
            string eight = "11111111";
            for (int j = 8; j >= 2; j--)
                sb = sb.Replace(eight.Substring(0, j), j.ToString());
            return sb.ToString();
            
            return "";
        }
        private object FenMoveColor()
        {
            return moveColor == Color.white ? "w" : "b";
        }
        private object FenCastleFlags()
        {
            string flags =
                (canCastleA1 ? "Q" : "") +
                (canCastleH1 ? "K" : "") +
                (canCastleA8 ? "q" : "") +
                (canCastleH8 ? "K" : "");
            return flags == "" ? "-" : flags;
        }
        private object FenEnpassant()
        {
            return enpassant.Name;
        }
        private object FenDrawNumber()
        {
            return drawNumber.ToString();
        }
        private object FenMoveNumber()
        {
            return moveNumber.ToString();
        }

        public Figure GetFigureAt(Square square)
        {
            if (square.OnBoard())
                return figures[square.x, square.y];
            return Figure.none;
        }
        void SetFiguresAt(Square square, Figure figure)
        {
            if (square.OnBoard())
                figures[square.x, square.y] = figure;
        }
        void Init()
        {
            // rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
            // 0                                           1 2    3 4 5
            string[] parts = fen.Split();
            if (parts.Length != 6)
            {
                return;
            }
            InitFigures(parts[0]);
            InitMoveColor(parts[1]);
            InitCastleFlags(parts[2]);
            InitEnpassant(parts[3]);
            InitDrawNumber(parts[4]);
            InitMoveNumber(parts[5]);
        }
        private void InitFigures(string v)
        {
            // 8 -> 71 -> 611 -> 61111 -> ...
            for (int j = 8; j >= 2; j--)
                v = v.Replace(j.ToString(), (j - 1).ToString() + "1");
            v = v.Replace('1', (char)Figure.none);
            string[] lines = v.Split('/');
            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                    figures[x, y] = (Figure)lines[7 - y] [x];
        }
        private void InitMoveColor(string v)
        {
            moveColor = (v == "b") ? Color.black : Color.white;
        }
        private void InitCastleFlags(string v)
        {
            canCastleA1 = v.Contains("Q");
            canCastleH1 = v.Contains("K");
            canCastleA8 = v.Contains("q");
            canCastleH8 = v.Contains("k");
        }
        private void InitEnpassant(string v)
        {
            enpassant = new Square(v);
        }
        private void InitDrawNumber(string v)
        {
            drawNumber = int.Parse(v);
        }
        private void InitMoveNumber(string v)
        {
            moveNumber = int.Parse(v);
        }
    }
}
