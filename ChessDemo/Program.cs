using System;
using System.Text;
using ChessRules;

namespace ChessDemo
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Chess chess = new Chess();
            chess.GetFigureAt(3, 5);

            while (true)
            {
                Console.WriteLine(chess.fen);
                Print(ChessToAscii(chess));
                string move = Console.ReadLine();
                if (move == "") break;
                chess = chess.Move(move);
            }
        }

        static string ChessToAscii (Chess chess)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("  +----------------+");
            for (int y = 7; y >= 0; y--)
            {
                sb.Append(y + 1);
                sb.Append(" |");
                for (int x = 0; x < 8; x++)
                    sb.Append(chess.GetFigureAt(x, y) + " ");
                sb.AppendLine("|");
            }
            sb.AppendLine("  +----------------+");
            sb.AppendLine("   a b c d e f g h  ");

            // if (chess.isCheck) sb.AppendLine("IS CHECK");
            // if (chess.isCheckmate) sb.AppendLine("IS CHECKMATE");
            // if (chess.isStalemate) sb.AppendLine("IS STALEMATE");
            return sb.ToString();
        }
        
        static string ChessToAscii2 (Chess chess)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("    a b c d e f g h  ");
            sb.AppendLine("  +-----------------+");
            for (int y = 7; y >= 0; y--)
            {
                sb.Append(y + 1);
                sb.Append(" | ");
                for (int x = 0; x < 8; x++) sb.Append(chess.GetFigureAt(x, y) + " ");
                sb.AppendLine("| " + (y + 1));
            }
            sb.AppendLine("  +-----------------+");
            sb.AppendLine("    A B C D E F G H  ");

            // if (chess.isCheck) sb.AppendLine("IS CHECK");
            // if (chess.isCheckmate) sb.AppendLine("IS CHECKMATE");
            // if (chess.isStalemate) sb.AppendLine("IS STALEMATE");
            return sb.ToString();
        }

        static void Print(string text)
        {
            ConsoleColor old = Console.ForegroundColor;
            foreach (char x in text)
            {
                if (x >= 'a' && x <= 'z')
                    Console.ForegroundColor = ConsoleColor.Red;
                else if (x >= 'A' && x <= 'Z')
                    Console.ForegroundColor = ConsoleColor.White;
                else
                    Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(x);
            }
            Console.ForegroundColor = old;
        }
    }
}
