using System;

namespace Chess.Model.Pieces
{
    public class Pawn : Piece
    {
        public override PieceType GetFigureType()
        {
            return PieceType.Pawn;
        }

        public override bool AbleMoveTo(Square target)
        {
            if (MoveIsForward(target) && target.Piece == null
                || AbleEat(target))
            {
                return true;
            }
            return false;
        }

        private bool MoveIsForward(Square targetSquare)
        {
            var distance = targetSquare.Pos - OwnSquare.Pos;
            if (!wasMoved && Math.Abs(distance.Y) == 2)
            {
                Vector2Int step = OwnSquare.Pos.GetStep(targetSquare.Pos);
                if (Desk.GetFigureAt(OwnSquare.Pos + step) != null)
                {
                    return false;
                }
                distance.Y /= 2;
            }
            if (color == ChessColor.Black && distance == new Vector2Int(0, -1)
                || color == ChessColor.White && distance == new Vector2Int(0, 1))
            {
                return true;
            }

            return false;
        }

        public bool AbleEat(Square targetSquare)
        {
            Vector2Int dist = targetSquare.Pos - OwnSquare.Pos;
            if (targetSquare.Piece != null && Math.Abs(dist.X) == 1 && targetSquare.Piece.color != color)
            {
                if (color == ChessColor.Black && dist.Y == -1 || color == ChessColor.White && dist.Y == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public Pawn(Desk getDesk) : base(getDesk) { }
    }
}
