using System;
using System.Collections.Generic;
using Chess;

public abstract class Figure : DeskObj
{
    public bool wasMoved;
    public FigureColor color;
    public Tile ownTile;

    protected Figure(Desk getDesk) : base(getDesk) {}

    public bool TryMoveSuccess(Tile target)
    {
        Figure figure = null;
        if (target.currentFigure != null)
        {
            figure = target.currentFigure;
        }
        Tile oldTile = ownTile;
        
        MoveToWithOutChecking(target);

        Figure king = Desk.FindKing(color);
        if (king == null)
        {
            throw new Exception("No King in desk");
        }
        bool isCheck = IsCheckTo(king);
        
        MoveToWithOutChecking(oldTile);
        if (figure != null)
        {
            figure.ownTile = target;
            target.currentFigure = figure;
        }
        return !isCheck;
    }

    public void MoveToWithOutChecking(Tile target)
    {
        ownTile.currentFigure = null;
        target.currentFigure = this;
        ownTile = target;
    }
    public void MoveTo(Tile target)
    {
        if (AbleMoveTo(target) && TryMoveSuccess(target))
        {
            wasMoved = true;
            ownTile.currentFigure = null;
            target.currentFigure = this;
            ownTile = target;   
            return;
        }
        throw new Exception("Loshara");
    }
    public abstract FigureType GetFigureType();
    public abstract bool AbleMoveTo(Tile target);

    private bool IsCheckTo(Figure king)
    {
        FigureColor oppositeColor = king.color == FigureColor.White ? FigureColor.Black : FigureColor.White;
        foreach (var figure in Desk.AllFigureColor(oppositeColor))
        {
            if (figure.AbleMoveTo(king.ownTile))
            {
                return true;
            }
        }
        return false;
    }
    protected bool CheckTile(Tile tile, FigureColor figureColor)
    {
        if (tile.currentFigure == null || tile.currentFigure.color != figureColor)
        {
            return true;
        }
        return false;
    }

    public bool AbleMoveAnyWhere()
    {
        foreach (var tile in Desk.desk)
        {
            if (AbleMoveTo(tile) && TryMoveSuccess(tile))
            {
                return true;
            }
        }
        return false;
    }

    public List<Tile> AbleMoveTiles()
    {
        List<Tile> tiles = new List<Tile>();
        foreach (var tile in Desk.desk)
        {
            if (AbleMoveTo(tile) && TryMoveSuccess(tile))
            {
                tiles.Add(tile); 
            }
        }
        return tiles;
    }

    protected bool CheckTiles(Vector2Int step, Tile target)
    {
        for (var pos = ownTile.pos + step; pos != target.pos; pos += step)
        {
            if (Desk.GetFigureAt(pos) != null)
            {
                return false;
            }
        }
        return CheckTile(target, color);
    }
}
