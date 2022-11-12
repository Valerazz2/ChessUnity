using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Model.Pieces;
using UnityEngine;

namespace Chess.Model
{
    public class Desk
    {
        public ChessColor Move = ChessColor.White;
        
        public static readonly int DeskSizeX = 8, DeskSizeY = 8;
    
        public readonly Square[,] Squares = new Square[DeskSizeX, DeskSizeY];
    
        public Piece CurrentPiece;
    
        public event Action<MoveInfo> OnMove;

        public void CreateMap()
        {
            var figuresSpots = new Piece[,]
            {
                {new Rook(this), new Pawn(this), null, null, null, null, new Pawn(this), new Rook(this)},
                {new Knight(this), new Pawn(this), null, null, null, null, new Pawn(this), new Knight(this)},
                {new Bishop(this), new Pawn(this), null, null, null, null, new Pawn(this), new Bishop(this)},
                {new Queen(this), new Pawn(this), null, null, null, null, new Pawn(this), new Queen(this)},
                {new King(this), new Pawn(this), null, null, null, null, new Pawn(this), new King(this)},
                {new Bishop(this), new Pawn(this), null, null, null, null, new Pawn(this), new Bishop(this)},
                {new Knight(this), new Pawn(this), null, null, null, null, new Pawn(this), new Knight(this)},
                {new Rook(this), new Pawn(this), null, null, null, null, new Pawn(this), new Rook(this)}
            };
        
            for (var x = 0; x < DeskSizeX; x++)
            {
                for (var y = 0; y < DeskSizeY; y++)
                {
                    var color = (x + y) % 2 == 0 ? ChessColor.Black : ChessColor.White;
                    var fig = figuresSpots[x, y];
                    var tile = Squares[x, y] = new Square(new Vector2Int(x, y), color, fig);
                    if (fig != null)
                    {
                        fig.Square = tile;
                        fig.Color = y <= 2 ? ChessColor.White : ChessColor.Black;
                    }
                }
            }
        }

        public void MoveTo(Piece piece, Square target)
        {
            if (!piece.AbleMoveTo(target) || !piece.TryMoveSuccess(target))
            {
                return;
            }

            Move = Move.Invert();
            if (piece.GetPieceType() == PieceType.King && Vector2Int.Distance(piece.Square.Pos, target.Pos) == new Vector2Int(2, 0))
            {
                var rook = FindRookByStep(target, piece);
                MoveRookWhenCastling(rook, piece);
            }
            
            var eventInfo = new MoveInfo
            {
                Piece = piece,
                MovedFrom = piece.Square,
                MoveType = MoveType.PieceMoved
            };
            piece.MoveTo(target);
            InvokeChessEvent(eventInfo);
        }

        private void InvokeChessEvent(MoveInfo eventInfo)
        {
            OnMove?.Invoke(eventInfo);
        }

        public Piece FindKing(ChessColor color)
        {
            return FindPieceColor(color).FirstOrDefault(figure => figure.GetPieceType() == PieceType.King);
        }
    
        public bool MateFor(Piece king)
        {
            return !FindPieceColor(king.Color).Any(figure => figure.AbleMoveAnyWhere()) && IsCheckTo(king);
        }

        public List<Piece> FindPieceColor(ChessColor chessColor)
        {
            var figures = new List<Piece>();
            foreach (var tile in Squares)
            {
                var piece = tile.Piece;
                if (piece != null && piece.Color == chessColor)
                {
                    figures.Add(piece);
                }
            }
            return figures;
        }
        private void MoveRookWhenCastling(Piece rook, Piece king)
        {
            var offset = king.Square.Pos.GetStep(rook.Square.Pos);
            var rookPos = king.Square.Pos + offset;
            MoveInfo moveInfo= new MoveInfo
            {
                Piece = rook,
                MovedFrom = rook.Square,
                MoveType = MoveType.PieceMoved
            };
            rook.MoveToWithOutChecking(Squares[rookPos.X, rookPos.Y]);
            InvokeChessEvent(moveInfo);
            
        }
        public Piece FindRookByStep(Square target, Piece king)
        {
            var step = king.Square.Pos.GetStep(target.Pos);
            var pos = king.Square.Pos + step;
            while (pos.X < DeskSizeX && pos.X >= 0)
            {
                var piece = GetPieceAt(pos); 
                if (piece != null)
                {
                    return piece;
                }
                
                var enemyColor = king.Color.Invert();
                if (FindPieceColor(enemyColor).Any(e => e.AbleMoveTo(Squares[pos.X, pos.Y])))
                {
                    return null;
                }
                
                pos += step;
            }
            return null;
        }

        public Piece GetPieceAt(Vector2Int pos)
        {
            return Squares[pos.X, pos.Y].Piece;
        }

        public bool StaleMateFor(ChessColor color)
        {
            return FindPieceColor(color).All(piece => !piece.AbleMoveAnyWhere());
        }
        public bool IsCheckTo(Piece king)
        {
            var oppositeColor = king.Color.Invert();
            return FindPieceColor(oppositeColor).Any(figure => figure.AbleMoveTo(king.Square));
        }

        public void SetPieceAt(Square square, Piece piece)
        {
            square.Piece = piece;
            piece.Square = square;
        }
    }
}
