using System;

namespace Chess.Model
{
    public class Square : DeskObj
    {
        public readonly Vector2Int Pos;
        public readonly ChessColor Color;
        public Piece Piece;
        private bool _moveAble;

        public bool MoveAble
        {
            get => _moveAble;
            internal set
            {
                if (value != _moveAble)
                {
                    _moveAble = value;
                    MoveAbleChanged?.Invoke();
                }
            }
        }

        public event Action MoveAbleChanged; 

        public Square(Vector2Int pos, ChessColor color, Piece piece, Desk desk) : base(desk)
        {
            Pos = pos;
            Color = color;
            Piece = piece;
        }
        

        public void Select()
        {
            Desk.Select(this);
        }

        public bool IsPieceOfColor(ChessColor color)
        {
            return Piece != null && Piece.Color == color;
        }
    }
}
