using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess.Model
{
    public class ChessEx
    {
        private Desk Desk;

        public string moves = "1.Nh3 h6 2.c4 g5 3.Nc3 g4 4.Nf4 e5 5.Nh5 Qh4 6.Ng3 Nf6 7.d4 exd4 8.Nb5 Na6 9.Qxd4 Bb4+ 10.Bd2 Qg5 11.Nxc7+ Kd8 12.Nxa6 Bxd2+ 13.Qxd2 bxa6 14.h4 Qe5 15.e3 Rb8 16.O-O-O a5 17.Qd4 Re8 18.Bd3 a4 19.Rhe1 a3 20.b3 Bb7 21.Bf1 Bc6 22.Kc2 a5 23.Kc3 Rc8 24.Qxe5 Rxe5 25.Rd6 Ke7 26.Red1 Be4 27.R6d4";
        
        private string[] stringNotation = {"P", "N", "B", "R", "Q", "K"};
        private PieceType[] typeNotation =
            {PieceType.Pawn, PieceType.Knight, PieceType.Bishop, PieceType.Rook, PieceType.Queen, PieceType.King};
        
        private string[] splitMoves;
        public ChessEx(Desk desk)
        {
            Desk = desk;
        }

        public IEnumerator a()
        {
            splitMoves = RefactorMoves();
            AddPawnMoves();
          
            foreach (var stringMove in splitMoves)
            {
                yield return new WaitForSeconds(0.5f);
                Debug.Log(stringMove);
                if (stringMove == "O-O")
                {
                    var king = Desk.FindKing(Desk.move);
                    var target = Desk.GetSquareAt(king.Square.Pos + new Vector2Int(2, 0));
                    Desk.MoveTo(king, target);
                    continue;
                }

                if (stringMove == "O-O-O")
                {
                    var king = Desk.FindKing(Desk.move);
                    var target = Desk.GetSquareAt(king.Square.Pos + new Vector2Int(-2, 0));
                    Desk.MoveTo(king, target);
                    continue;
                }
                var piecesColor = Desk.FindPieceColor(Desk.move);
                var fitsPiecesForColor = FindFitsPiecesFor(stringMove[0], (List<Piece>)piecesColor);
                Square square = GetSquareAt(stringMove);
                var FitsForMove = FindPiecesAbleMoveTo(fitsPiecesForColor, square);
                
                if (FitsForMove.Count == 1)
                {
                    Desk.MoveTo(FitsForMove[0], square);
                }
                else
                {
                    MoveTruePiece(FitsForMove, square, stringMove[1]);
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
                        piece.MoveTo(square);
                        break;
                    }
                }
                else
                {
                    int posX = stringMove - 'a';
                    if (piece.Square.Pos.X == posX)
                    {
                        piece.MoveTo(square);
                        break;
                    }
                }
            }
        }

        private void AddPawnMoves()
        {
            for (int i = 0; i < splitMoves.Length; i++)
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

        private List<Piece> FindPiecesAbleMoveTo(List<Piece> _pieces, Square target)
        {
            List<Piece> pieces = new List<Piece>();
            foreach (var piece in _pieces)
            {
                if (piece.AbleMoveTo(target) && piece.TryMoveSuccess(target))
                {
                    pieces.Add(piece);   
                }
            }
            return pieces;
        }

        private List<Piece> FindFitsPiecesFor(char a, List<Piece> pieces)
        {
            var fitsPieces = new List<Piece>();
            foreach (var piece in pieces)
            {
                var type = GetTypeFor(a);
                if (piece.GetPieceType() == type)
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
                if (moves[i] == 'x' || moves[i] == '+')
                {
                    moves = moves.Remove(i, 1);
                }
            }
            
            var splitMoves = moves.Split();
            for (var i = 0; i < splitMoves.Length; i += 2) 
            {
                if (i < 18)
                {
                    splitMoves[i] = splitMoves[i].Remove(0, 2);
                }
                else
                {
                    splitMoves[i] = splitMoves[i].Remove(0, 3);
                }
            }
            return splitMoves;
        }
    }
}
