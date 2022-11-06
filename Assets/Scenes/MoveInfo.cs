namespace Chess
{
    public class MoveInfo
    {
        public MoveType MoveType;
        public Tile MovedFrom;
        public Figure Figure;
    }

    public enum MoveType
    {
        FigureMoved,
        FigureRemoved
    }
}