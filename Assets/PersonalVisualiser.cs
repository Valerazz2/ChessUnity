using Chess;
using UnityEngine;
using Vector2Int = Chess.Vector2Int;

public class PersonalVisualiser : MonoBehaviour
{
    public Figure ownFigure;
    public Desk Desk;
    private bool wasDestroyed;
    void Start()
    {
        Desk.OnMove += OnFigureMoved;
    }

    public void OnFigureMoved(MoveInfo moveInfo)
    {
        if (moveInfo.Figure == ownFigure)
        {
            transform.position = new Vector3(moveInfo.Figure.ownTile.pos.X, moveInfo.Figure.ownTile.pos.Y);
        }

        Vector2Int pos = ownFigure.ownTile.pos;
        if (Desk.desk[pos.X, pos.Y].currentFigure != ownFigure && !wasDestroyed)
        {
            wasDestroyed = true;
            DestroyImmediate(gameObject);
        }
    }
}
