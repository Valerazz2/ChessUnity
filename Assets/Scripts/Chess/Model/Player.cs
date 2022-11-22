using System.Collections.Generic;

namespace Chess.Model
{
    public class Player
    {
        private readonly List<PieceType> capturedPieces = new List<PieceType>();
        private ChessColor color;

        public Player(ChessColor chessColor, Desk desk)
        {
            color = chessColor;
            desk.OnPieceRemove += AddPiece;
        }

        private void AddPiece(Piece piece)
        {
            if (piece.Color != color)
            {
                capturedPieces.Add(piece.GetPieceType());
            }
        }
    }
}
