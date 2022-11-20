using Chess.Model;
using UnityEngine;

public static class ViewEx
{
    public static Vector3 GetPosVector3(this Square square)
    {
        return new Vector3(square.Pos.X, square.Pos.Y);
    }

    public static Color ToUnityColor(this ChessColor chessColor)
    {
        return chessColor == ChessColor.White ? Color.white : Color.green;
    }
}
