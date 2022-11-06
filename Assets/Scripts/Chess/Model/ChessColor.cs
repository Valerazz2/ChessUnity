namespace Chess.Model
{
    public enum ChessColor
    {
        White, 
        Black
    }

    public static class ChessColorExt
    {
        public static ChessColor Invert(this ChessColor e)
        {
            return e == ChessColor.White ? ChessColor.Black : ChessColor.White;
        }
    }
}
