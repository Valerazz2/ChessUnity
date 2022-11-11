namespace Chess.Model
{
    public class Square
    {
        public readonly Vector2Int Pos;
        public readonly ChessColor Color;
        public Piece Piece;
    
        public Square(Vector2Int pos, ChessColor color, Piece piece)
        {
            Pos = pos;
            Color = color;
            Piece = piece;
        }
    }
}
