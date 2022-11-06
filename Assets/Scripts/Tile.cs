using System.Collections;
using System.Collections.Generic;
using Chess;
using UnityEngine;
using Vector2Int = Chess.Vector2Int;

public class Tile
{
    public Vector2Int pos;
    public FigureColor color;
    public Figure currentFigure;
    
    public Tile(Vector2Int getPos, FigureColor getColor, Figure figure)
    {
        pos = getPos;
        color = getColor;
        if (figure != null)
        {
            currentFigure = figure;
        }
    }
}
