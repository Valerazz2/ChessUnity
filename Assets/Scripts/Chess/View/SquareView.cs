using Chess.Model;
using Chess.View;
using UnityEngine;

public class SquareView : AbstractView<Square>
{
    [SerializeField] private SpriteRenderer SpriteRenderer;
    [SerializeField] private Sprite blackSquare;
    [SerializeField] private Sprite whiteSquare;

    [SerializeField] private GameObject choosedSquare;
    [SerializeField] private GameObject choosedPiece;
    private GameObject createdSprite;

    protected override void OnBind()
    {
        model.MoveAbleChanged += ChangeColor;
        var sprite = model.Color == ChessColor.White ? whiteSquare : blackSquare;
        SpriteRenderer.sprite = sprite;
        transform.position = model.GetPosVector3();
    }

    private void ChangeColor()
    {
        if (createdSprite)
            Destroy(createdSprite);
        
        
        if (model.Piece != null && model.Piece.Color == model.Desk.move && model.MoveAble)
        {
            createdSprite = Instantiate(choosedPiece);
            createdSprite.transform.position = model.GetPosVector3();
            return;
        }

        if (!model.MoveAble) return;
        createdSprite = Instantiate(choosedSquare);
        createdSprite.transform.position = model.GetPosVector3();
    }

}
