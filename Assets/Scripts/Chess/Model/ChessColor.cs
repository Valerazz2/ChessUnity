namespace Chess.Model
{
    public enum ChessColor
    {
        White, 
        Black
    }

    public static class ChessColorExt
    {
        public static ChessColor Invert(this ChessColor color)
        {
            return color == ChessColor.White ? ChessColor.Black : ChessColor.White;
        }

        public static Vector2Int GetNaturalDirection(this ChessColor color)
        {
            return color == ChessColor.White ? Vector2Int.Up : Vector2Int.Down;
        }
    }
}
