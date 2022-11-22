using System;
using System.Collections.Generic;
using System.Linq;
using Chess.Model.Pieces;
using Chess.View;
using UnityEngine;

namespace Chess.Model
{
    public class Desk
    {
        
        public static readonly int DeskSizeX = 8, DeskSizeY = 8;
        public IEnumerable<Square> ISquares => Squares.Cast<Square>(); 
        
        private ChessColor move = ChessColor.White;

        private readonly Square[,] Squares = new Square[DeskSizeX, DeskSizeY];
        
        private Piece CurrentPiece;

        private ChessState ChessState = ChessState.FigureNull;

        public event Action<MoveInfo> OnMove;

        public event Action<Piece> OnPieceAdd;

        public event Action<Piece> OnPieceRemove;

        private Player whitePlayer, blackPlayer;

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
                    var tile = Squares[x, y] = new Square(new Vector2Int(x, y), color, fig, this);
                    if (fig != null)
                    {
                        fig.Square = tile;
                        fig.Color = y <= 2 ? ChessColor.White : ChessColor.Black;
                    }
                }
            }

            whitePlayer = new Player(ChessColor.White, this);
            blackPlayer = new Player(ChessColor.Black, this);
        }

        public IEnumerable<Piece> GetAllPiece()
        {
            return (from Square square in Squares where square.Piece != null select square.Piece).ToList();
        }

        private void MoveTo(Piece piece, Square target)
        {
            if (!piece.AbleMoveTo(target))
            {
                return;
            }
            move = move.Invert();
            if (piece.GetPieceType() == PieceType.King && 
                Vector2Int.Distance(piece.Square.Pos, target.Pos) == new Vector2Int(2, 0))
            {
                var rook = FindFirstFigureByStep(target, piece);
                MoveRookWhenCastling(rook, piece);
            }

            var eventInfo = new MoveInfo
            {
                Piece = piece,
                MovedFrom = piece.Square,
            };
            piece.MoveTo(target);
            OnMove?.Invoke(eventInfo);

            if (piece.GetPieceType() == PieceType.Pawn && piece.ReachedLastSquare())
            {
                var queen = new Queen(this)
                {
                    Color = piece.Color,
                    Square = piece.Square
                };
                SetPieceAt(target, queen);
                
                OnPieceAdd?.Invoke(queen);
                OnPieceRemove?.Invoke(piece);
            }
        }

        public Piece FindKing(ChessColor color)
        {
            return FindPieceColor(color).FirstOrDefault(figure => figure.GetPieceType() == PieceType.King);
        }

        public bool MateFor(Piece king)
        {
            return !FindPieceColor(king.Color).Any(figure => figure.AbleMoveAnyWhere()) && IsCheckTo(king);
        }

        private IEnumerable<Piece> FindPieceColor(ChessColor chessColor)
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
        
        public Piece FindFirstFigureByStep(Square target, Piece king)
        {
            var step = king.Square.Pos.GetStep(target.Pos);
            var pos = king.Square.Pos + step;
            while (pos.X < DeskSizeX && pos.X >= 0)
            {
                var piece = GetPieceAt(pos);
                var enemyColor = king.Color.Invert();
                if (piece != null || FindPieceColor(enemyColor).Any(e => e.AbleMoveTo(Squares[pos.X, pos.Y])))
                {
                    return piece;
                }

                pos += step;
            }
            return null;
        }


        private void MoveRookWhenCastling(Piece rook, Piece king)
        {
            var offset = king.Square.Pos.GetStep(rook.Square.Pos);
            var rookPos = king.Square.Pos + offset;
            var moveInfo = new MoveInfo
            {
                Piece = rook,
                MovedFrom = rook.Square,
            };
            rook.MoveToWithOutChecking(Squares[rookPos.X, rookPos.Y]);
            OnMove?.Invoke(moveInfo);
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

        private void SetPieceAt(Square square, Piece piece)
        {
            square.Piece = piece;
            piece.Square = square;
        }

        public void Select(Square square)
        {
            switch (ChessState)
            {
                case ChessState.FigureNull:
                    if (square.IsPieceOfColor(move))
                    {
                        CurrentPiece = square.Piece;
                        SetMoveAbleSquaresFor(CurrentPiece);
                        ChessState = ChessState.FigureChoosed;
                    }
                    break;
                
                case ChessState.FigureChoosed:
                    if (square.IsPieceOfColor(move))
                    {
                        CurrentPiece = square.Piece;
                        SetMoveAbleSquaresFor(CurrentPiece);
                    }
                    else if (CurrentPiece.Color == move)
                    {
                        MoveTo(CurrentPiece, square);
                        SetAbleMoveTiles(false);
                    }
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void SetMoveAbleSquaresFor(Piece piece)
        {
            foreach (var square in Squares)
            {
                square.MoveAble = piece.AbleMoveTo(square);
            }
            piece.Square.MoveAble = true;
        }
        private void SetAbleMoveTiles(bool moveAble)
        {
            foreach (var square in Squares)
            {
                square.MoveAble = moveAble;
            }
        }

        public void OnPieceRemoveInvoke(Piece obj)
        {
            OnPieceRemove?.Invoke(obj);
        }
    }
}
