using System.Collections.Generic;
using System.Linq;

namespace Chess.Model
{
    public class Player
    {
        private readonly List<PieceType> capturedPieces = new List<PieceType>();
        private ChessColor color;
        public int CapturedPiecesPrice => capturedPieces.Sum(pieceType => pieceType.GetPrice());

        public Player(ChessColor chessColor, Desk desk)
        {
            color = chessColor;
            desk.OnPieceCaptured += AddPiece;
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
