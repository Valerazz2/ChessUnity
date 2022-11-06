using System.Reflection;
using UnityEngine;

namespace Chess
{
    public class King : Figure
    {
        public override FigureType GetFigureType()
        {
            return FigureType.King;
        }

        public override bool AbleMoveTo(Tile target)
        {
            var dist = Vector2Int.Distance(target.pos, ownTile.pos);
            if (dist.X < 2 && dist.Y < 2 && dist != Vector2Int.ZERO)
            {
                return CheckTile(target, color);
            }
            
            if (dist == new Vector2Int(2, 0) && AbleCastling(target))
            {
                return true;
            }
            return false;
        }
        private bool AbleCastling(Tile target)
        {
            Figure rook = FindRookByStep(target);
            
            if (!wasMoved && rook != null && rook.GetFigureType() == FigureType.Ladja && !rook.wasMoved)
            {
                return true;
            }

            return false;
        }

        private Figure FindRookByStep(Tile target)
        {
            Vector2Int step = ownTile.pos.GetStep(target.pos);
            var pos = ownTile.pos + step;
            while (pos.X - 1 <= Desk.deskSizeX && pos.X >= 0)
            {
                Figure figure = Desk.desk[pos.X, pos.Y].currentFigure; 
                if (figure != null)
                {
                    return figure;
                }
                
                FigureColor enemyColor = color == FigureColor.White ? FigureColor.Black : FigureColor.White;
                foreach (var enemyFigure in Desk.AllFigureColor(enemyColor))
                {
                    if (enemyFigure.AbleMoveTo(Desk.desk[pos.X, pos.Y]))
                    {
                        return null;
                    }
                }
                pos += step;
            }
            return null;
        }
        public King(Desk getDesk) : base(getDesk) {}
    }
}