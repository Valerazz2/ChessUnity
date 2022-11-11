using Chess.Model;
using UnityEngine;
using Vector2Int = Chess.Model.Vector2Int;

namespace Chess.View
{
    public class PersonalVisualiser : MonoBehaviour
    {
        public Piece OwnPiece;
        public Desk Desk;
        private bool wasDestroyed;
        void Start()
        {
            Desk.OnMove += OnFigureMoved;
        }

        private void OnFigureMoved(MoveInfo moveInfo)
        {
            if (moveInfo.Piece == OwnPiece)
            {
                transform.position = new Vector3(moveInfo.Piece.OwnSquare.Pos.X, moveInfo.Piece.OwnSquare.Pos.Y);
            }

            Vector2Int pos = OwnPiece.OwnSquare.Pos;
            if (Desk.GetPieceAt(pos) != OwnPiece && !wasDestroyed)
            {
                wasDestroyed = true;
                DestroyImmediate(gameObject);
            }
        }
    }
}
