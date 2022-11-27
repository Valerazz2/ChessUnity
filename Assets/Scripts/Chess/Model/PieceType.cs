namespace Chess.Model
{
    public enum PieceType
    {
        Pawn, 
        Knight, 
        Bishop,
        Rook, 
        Queen, 
        King
    }

    public static class PieceTypeEx
    {
        private static readonly int[] PRICES = {1, 3, 3, 5, 8, 100000};

        public static int GetPrice(this PieceType pieceType)
        {
            return PRICES[(int)pieceType];
        }
    }
}