using System.Collections.Generic;
using System.Linq;

namespace Chess.Model
{
    public class Player
    {
        private readonly List<Piece> capturedPieces = new List<Piece>();
        private ChessColor color;
        public int CapturedPiecesPrice => capturedPieces.Sum(pieceType => pieceType.price);

        public Player(ChessColor chessColor, Desk desk)
        {
            color = chessColor;
            desk.OnPieceCaptured += AddPiece;
        }

        private void AddPiece(Piece piece)
        {
            if (piece.Color != color)
            {
                capturedPieces.Add(piece);
            }
        }
    }
}
