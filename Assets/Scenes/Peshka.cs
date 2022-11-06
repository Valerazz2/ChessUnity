using System;
using System.Collections;
using System.Collections.Generic;
using Chess;

public class Peshka : Figure
{
    public override FigureType GetFigureType()
    {
        return FigureType.Peshka;
    }

    public override bool AbleMoveTo(Tile target)
    {
        if (MoveIsForward(target) && target.currentFigure == null
            || AbleEat(target))
        {
            return true;
        }
        return false;
    }

    private bool MoveIsForward(Tile targetTile)
    {
        var distance = targetTile.pos - ownTile.pos;
        if (!wasMoved && Math.Abs(distance.Y) == 2)
        {
            Vector2Int step = ownTile.pos.GetStep(targetTile.pos);
            if (Desk.desk[ownTile.pos.X + step.X,ownTile.pos.Y + step.Y].currentFigure != null)
            {
                return false;
            }
            distance.Y /= 2;
        }
        if (color == FigureColor.Black && distance == new Vector2Int(0, -1)
            || color == FigureColor.White && distance == new Vector2Int(0, 1))
        {
            return true;
        }

        return false;
    }

    public bool AbleEat(Tile targetTile)
    {
        Vector2Int dist = targetTile.pos - ownTile.pos;
        if (targetTile.currentFigure != null && Math.Abs(dist.X) == 1 && targetTile.currentFigure.color != color)
        {
            if (color == FigureColor.Black && dist.Y == -1 || color == FigureColor.White && dist.Y == 1)
            {
                return true;
            }
        }
        return false;
    }

    public Peshka(Desk getDesk) : base(getDesk) { }
}
