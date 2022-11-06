using System;
using System.Collections.Generic;
using Chess.Model.Pieces;

namespace Chess.Model
{
    public class Desk
    {
        public ChessColor Move = ChessColor.White;
        public static readonly int deskSizeX = 8, deskSizeY = 8;
    
        public Square[,] desk = new Square[deskSizeX, deskSizeY];
    
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
        
            for (int x = 0; x < deskSizeX; x++)
            {
                for (int y = 0; y < deskSizeY; y++)
                {
                    var color = (x + y) % 2 == 0 ? ChessColor.Black : ChessColor.White;
                    var fig = figuresSpots[x, y];
                    var tile = desk[x, y] = new Square(new Vector2Int(x, y), color, fig);
                    if (fig != null)
                    {
                        fig.OwnSquare = tile;
                        fig.color = y <= 2 ? ChessColor.White : ChessColor.Black;
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
            if (piece.GetFigureType() == PieceType.King && Vector2Int.Distance(piece.OwnSquare.Pos, target.Pos) == new Vector2Int(2, 0))
            {
                Vector2Int dir = target.Pos - piece.OwnSquare.Pos;
                dir.X /= 2;
                Piece rook = FindRookByStep(target, piece);
                MoveRookWhenCastling(dir, rook, piece);
            }
            var eventInfo = new MoveInfo();
            eventInfo.Piece = piece;
            eventInfo.MovedFrom = piece.OwnSquare;
            eventInfo.MoveType = MoveType.FigureMoved;
            piece.MoveTo(target);
            InvokeChessEvent(eventInfo);
        }

        public void InvokeChessEvent(MoveInfo eventInfo)
        {
            OnMove?.Invoke(eventInfo);
        }

        public Piece FindKing(ChessColor color)
        {
            foreach (var figure in AllFigureColor(color))
            {
                if (figure.GetFigureType() == PieceType.King)
                {
                    return figure;
                }
            }
            return null;
        }
    
        public bool IsMateFor(Piece king)
        {
            foreach (var figure in AllFigureColor(king.color))
            {
                if (figure.AbleMoveAnyWhere())
                {
                    return false;
                }
            }
            return true;
        }

        public List<Piece> AllFigureColor(ChessColor chessColor)
        {
            List<Piece> figures = new List<Piece>();
            foreach (var tile in desk)
            {
                var figure = tile.Piece;
                if (figure != null && figure.color == chessColor)
                {
                    figures.Add(figure);
                }
            }
            return figures;
        }
        private void MoveRookWhenCastling(Vector2Int offset, Piece rook, Piece king)
        {
            Vector2Int rookPos = king.OwnSquare.Pos + offset;
            MoveInfo moveInfo= new MoveInfo
            {
                Piece = rook,
                MovedFrom = rook.OwnSquare,
                MoveType = MoveType.FigureMoved
            };
            rook.MoveToWithOutChecking(desk[rookPos.X, rookPos.Y]);
            InvokeChessEvent(moveInfo);
            
        }
        private Piece FindRookByStep(Square target, Piece king)
        {
            Vector2Int step = king.OwnSquare.Pos.GetStep(target.Pos);
            var pos = king.OwnSquare.Pos + step;
            while (pos.X - 1 <= deskSizeX && pos.X >= 0)
            {
                Piece piece = GetFigureAt(pos); 
                if (piece != null)
                {
                    return piece;
                }
                
                ChessColor enemyColor = king.color == ChessColor.White ? ChessColor.Black : ChessColor.White;
                foreach (var enemyFigure in AllFigureColor(enemyColor))
                {
                    if (enemyFigure.AbleMoveTo(desk[pos.X, pos.Y]))
                    {
                        return null;
                    }
                }
                pos += step;
            }
            return null;
        }

        public Piece GetFigureAt(Vector2Int pos)
        {
            return desk[pos.X, pos.Y].Piece;
        }
    }
}
