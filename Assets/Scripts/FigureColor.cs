namespace Chess
{
    public enum FigureColor
    {
        White, 
        Black
    }

    public static class FigureColorExt
    {
        public static FigureColor Invert(this FigureColor e)
        {
            return e == FigureColor.White ? FigureColor.Black : FigureColor.White;
        }
    }
}
