namespace Chess.Model.Pieces
{
    public class King : Piece
    {
        public override PieceType GetFigureType()
        {
            return PieceType.King;
        }

        public override bool AbleMoveTo(Square target)
        {
            var dist = Vector2Int.Distance(target.Pos, OwnSquare.Pos);
            if (dist.X < 2 && dist.Y < 2 && dist != Vector2Int.ZERO)
            {
                return CheckTile(target, color);
            }
            
            if (dist == new Vector2Int(2, 0) && AbleCastling(target))
            {
                return true;
            }
            return false;
        }
        private bool AbleCastling(Square target)
        {
            Piece rook = FindRookByStep(target);
            
            if (!wasMoved && rook != null && rook.GetFigureType() == PieceType.Rook && !rook.wasMoved)
            {
                return true;
            }

            return false;
        }

        private Piece FindRookByStep(Square target)
        {
            Vector2Int step = OwnSquare.Pos.GetStep(target.Pos);
            var pos = OwnSquare.Pos + step;
            while (pos.X - 1 <= Desk.deskSizeX && pos.X >= 0)
            {
                Piece piece = Desk.desk[pos.X, pos.Y].Piece; 
                if (piece != null)
                {
                    return piece;
                }
                
                ChessColor enemyColor = color == ChessColor.White ? ChessColor.Black : ChessColor.White;
                foreach (var enemyFigure in Desk.AllFigureColor(enemyColor))
                {
                    if (enemyFigure.AbleMoveTo(Desk.desk[pos.X, pos.Y]))
                    {
                        return null;
                    }
                }
                pos += step;
            }
            return null;
        }
        public King(Desk getDesk) : base(getDesk) {}
    }
}