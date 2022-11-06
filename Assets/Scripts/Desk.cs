using System;
using System.Collections.Generic;
using Chess;
using Vector2Int = Chess.Vector2Int;

public class Desk
{
    public FigureColor Move = FigureColor.White;
    public static readonly int deskSizeX = 8, deskSizeY = 8;
    
    public Tile[,] desk = new Tile[deskSizeX, deskSizeY];
    
    public Figure currentFigure;
    
    public event Action<MoveInfo> OnMove;

    public void CreateMap()
    {
        var figuresSpots = new Figure[,]
        {
            {new Ladja(this), new Peshka(this), null, null, null, null, new Peshka(this), new Ladja(this)},
            {new Knight(this), new Peshka(this), null, null, null, null, new Peshka(this), new Knight(this)},
            {new Slon(this), new Peshka(this), null, null, null, null, new Peshka(this), new Slon(this)},
            {new Ferz(this), new Peshka(this), null, null, null, null, new Peshka(this), new Ferz(this)},
            {new King(this), new Peshka(this), null, null, null, null, new Peshka(this), new King(this)},
            {new Slon(this), new Peshka(this), null, null, null, null, new Peshka(this), new Slon(this)},
            {new Knight(this), new Peshka(this), null, null, null, null, new Peshka(this), new Knight(this)},
            {new Ladja(this), new Peshka(this), null, null, null, null, new Peshka(this), new Ladja(this)}
        };
        
        for (int x = 0; x < deskSizeX; x++)
        {
            for (int y = 0; y < deskSizeY; y++)
            {
                var color = (x + y) % 2 == 0 ? FigureColor.Black : FigureColor.White;
                var fig = figuresSpots[x, y];
                var tile = desk[x, y] = new Tile(new Vector2Int(x, y), color, fig);
                if (fig != null)
                {
                    fig.ownTile = tile;
                    fig.color = y <= 2 ? FigureColor.White : FigureColor.Black;
                }
            }
        }
    }

    public void MoveTo(Figure figure, Tile target)
    {
        if (!figure.AbleMoveTo(target) || !figure.TryMoveSuccess(target))
        {
            return;
        }

        Move = Move.Invert();
        if (figure.GetFigureType() == FigureType.King && Vector2Int.Distance(figure.ownTile.pos, target.pos) == new Vector2Int(2, 0))
        {
            Vector2Int dir = target.pos - figure.ownTile.pos;
            dir.X /= 2;
            Figure rook = FindRookByStep(target, figure);
            MoveRookWhenCastling(dir, rook, figure);
        }
        var eventInfo = new MoveInfo();
        eventInfo.Figure = figure;
        eventInfo.MovedFrom = figure.ownTile;
        eventInfo.MoveType = MoveType.FigureMoved;
        figure.MoveTo(target);
        InvokeChessEvent(eventInfo);
    }

    public void InvokeChessEvent(MoveInfo eventInfo)
    {
        OnMove?.Invoke(eventInfo);
    }

    public Figure FindKing(FigureColor color)
    {
        foreach (var figure in AllFigureColor(color))
        {
            if (figure.GetFigureType() == FigureType.King)
            {
                return figure;
            }
        }
        return null;
    }
    
    public bool IsMateFor(Figure king)
    {
        foreach (var figure in AllFigureColor(king.color))
        {
            if (figure.AbleMoveAnyWhere())
            {
                return false;
            }
        }
        return true;
    }

    public List<Figure> AllFigureColor(FigureColor figureColor)
    {
        List<Figure> figures = new List<Figure>();
        foreach (var tile in desk)
        {
            var figure = tile.currentFigure;
            if (figure != null && figure.color == figureColor)
            {
                figures.Add(figure);
            }
        }
        return figures;
    }
    private void MoveRookWhenCastling(Vector2Int offset, Figure rook, Figure king)
    {
        Vector2Int rookPos = king.ownTile.pos + offset;
        MoveInfo moveInfo= new MoveInfo
        {
            Figure = rook,
            MovedFrom = rook.ownTile,
            MoveType = MoveType.FigureMoved
        };
        rook.MoveToWithOutChecking(desk[rookPos.X, rookPos.Y]);
        InvokeChessEvent(moveInfo);
            
    }
    private Figure FindRookByStep(Tile target, Figure king)
    {
        Vector2Int step = king.ownTile.pos.GetStep(target.pos);
        var pos = king.ownTile.pos + step;
        while (pos.X - 1 <= deskSizeX && pos.X >= 0)
        {
            Figure figure = GetFigureAt(pos); 
            if (figure != null)
            {
                return figure;
            }
                
            FigureColor enemyColor = king.color == FigureColor.White ? FigureColor.Black : FigureColor.White;
            foreach (var enemyFigure in AllFigureColor(enemyColor))
            {
                if (enemyFigure.AbleMoveTo(desk[pos.X, pos.Y]))
                {
                    return null;
                }
            }
            pos += step;
        }
        return null;
    }

    public Figure GetFigureAt(Vector2Int pos)
    {
        return desk[pos.X, pos.Y].currentFigure;
    }
}
