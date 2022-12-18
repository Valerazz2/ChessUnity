using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chess.Model
{
    public class ChessEx
    {
        private Desk Desk;

        public string moves = "1. g4 d5 2. Bg2 c6 3. b4 Bxg4 4. a4 e6 5. b5 Nf6 6. Nf3 Bd6 7. Ba3 c5 8. d4 cxd4 9. Qxd4 Bxf3 10. Bxf3 e5 11. Qd2 e4 12. Bg2 Bxa3 13. Rxa3 Nbd7 14. h4 Nb6 15. Qg5 Qe7 16. O-O Nc4 17. Rg3 g6 18. Nc3 h6 19. Qc1 d4 20. Nb1 Nd6 21. c4 Rc8 22. c5 Nf5 23. c6 bxc6 24. Rh3 c5 25. Nd2 e3 26. fxe3 Nxe3 27. Rxf6 Qxf6 28. Ne4 Qe5 29. Rxe3 dxe3 30. Qxe3 Qd4 31. Nd6+ Kd7 32. Qxd4 cxd4 33. Nxf7 Rc1+ 34. Kh2 Re8 35. Bf3 h5 36. Kg3 Rc3 37. Kf2 Ke6 38. Ng5+ Ke7 39. Be4 Kf6 40. a5 Rb8 41. b6 axb6 42. a6 Ra3 43. Bb7 Ke7 44. Ne4 Kd7 45. Nd2 Rxb7 46. axb7 Kc7 47. Nc4 Rc3 48. Nxb6 Kxb7 49. Nd5 Rc6 50. Kf3 Rd6 51. Nf4 Kc6 52. Ke4 Kc5 53. Ke5 Ra6 54. Ne6+ Kc4 55. Nxd4 g5 56. hxg5 Ra5+ 57. Ke4 Rxg5 58. e3 h4 59. Kf4 Rg8 60. Nf3 h3 61. e4 Rg2 62. e5 h2 63. Kf5 h1=Q 64. e6 Rf2 65. e7 Qxf3+ 66. Ke6 Qd5# ";
        
        private string[] stringNotation = {"P", "N", "B", "R", "Q", "K"};
        private PieceType[] typeNotation = {PieceType.Pawn, PieceType.Knight, PieceType.Bishop, PieceType.Rook, PieceType.Queen, PieceType.King};
        
        private string[] splitMoves;
        public ChessEx(Desk desk) {Desk = desk;}

        public IEnumerator a()
        {
            splitMoves = RefactorMoves();
            AddPawnMoves();
            
            foreach (var stringMove in splitMoves)
            {
                if (char.IsDigit(stringMove[0]))
                    continue;
                
                yield return new WaitForSeconds(0.5f);
                Debug.Log(stringMove);
                
                switch (stringMove)
                {
                    case "O-O": Castle(2);
                        continue;
                    case "O-O-O": Castle(-2); 
                        continue;
                }

                var square = GetSquareAt(stringMove);
                var fitsForMove = FindFitsPiecesFor(stringMove[0], square);

                if (fitsForMove.Count == 1)
                {
                    Desk.MoveTo(fitsForMove[0], square);
                }
                else
                {
                    MoveTruePiece(fitsForMove, square, stringMove[1]);
                }
            }
        }

        private void MoveTruePiece(List<Piece> fitsForMove, Square square, char stringMove)
        {
            foreach (var piece in fitsForMove)
            {
                if (char.IsDigit(stringMove))
                {
                    int posY = stringMove - '1';
                    if (piece.Square.Pos.Y == posY)
                    {
                        Desk.MoveTo(piece, square);
                        break;
                    }
                }
                else
                {
                    int posX = stringMove - 'a';
                    if (piece.Square.Pos.X == posX)
                    {
                        Desk.MoveTo(piece, square);
                        break;
                    }
                }
            }
        }

        private void Castle(int x)
        {
            var king = Desk.FindKing(Desk.move);
            var target = Desk.GetSquareAt(king.Square.Pos + new Vector2Int(x, 0));
            Desk.MoveTo(king, target);
        }
        
        private void AddPawnMoves()
        {
            for (var i = 0; i < splitMoves.Length; i++)
            {
                if (!char.IsUpper(splitMoves[i][0]))
                {
                    splitMoves[i] = splitMoves[i].Insert(0, "P");
                }
            }
        }

        private Square GetSquareAt(string pos)
        {
            int x = pos[pos.Length - 2] - 'a';
            int y = pos[pos.Length - 1] - '1';
            return Desk.GetSquareAt(new Vector2Int(x, y));
        }

        private List<Piece> FindFitsPiecesFor(char a, Square target)
        {
            var fitsPieces = new List<Piece>();
            foreach (var piece in Desk.GetAllPiece())
            {
                var type = GetTypeFor(a);
                if (piece.GetPieceType() == type && piece.Color == Desk.move && piece.AbleMoveTo(target) && piece.TryMoveSuccess(target))
                {
                    fitsPieces.Add(piece);
                }
            }
            return fitsPieces;
        }

        private PieceType? GetTypeFor(char pieceName)
        {
            for (var i = 0; i < stringNotation.Length; i++)
            {
                if (stringNotation[i][0] == pieceName)
                {
                    return typeNotation[i];
                }
            }
            return null;
        }

        private string[] RefactorMoves()
        {
            for (int i = 0; i < moves.Length; i++)
            {
                if (moves[i] == 'x' || moves[i] == '+' || moves[i] == '#')
                {
                    moves = moves.Remove(i, 1);
                }

                if (moves[i] == '=')
                {
                    moves = moves.Remove(i, 2);
                }
            }
            return moves.Split().Where((t, j) => j % 3f != 0).ToArray();
        }
    }
}
