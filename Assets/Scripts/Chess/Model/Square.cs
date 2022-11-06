namespace Chess.Model
{
    public class Square
    {
        public Vector2Int Pos;
        public ChessColor Color;
        public Piece Piece;
    
        public Square(Vector2Int pos, ChessColor color, Piece piece)
        {
            Pos = pos;
            Color = color;
            Piece = piece;
        }
    }
}
