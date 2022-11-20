using Chess.Model;
using Chess.View;
using UnityEngine;

public class SquareView : AbstractView<Square>
{
    [SerializeField] private SpriteRenderer SpriteRenderer;

    protected override void OnBind()
    {
        model.MoveAbleChanged += ChangeColor;
        ChangeColor();
        transform.position = model.GetPosVector3();
    }

    private void ChangeColor()
    {
       var newColor = model.MoveAble ? Color.yellow : model.Color.ToUnityColor();
       SpriteRenderer.color = newColor;
    }

}
