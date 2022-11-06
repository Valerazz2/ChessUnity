namespace Chess.Model
{
    public class MoveInfo
    {
        public MoveType MoveType;
        public Square MovedFrom;
        public Piece Piece;
    }

    public enum MoveType
    {
        FigureMoved,
        FigureRemoved
    }
}