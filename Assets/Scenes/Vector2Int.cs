using System;
using System.Security.Cryptography.X509Certificates;

namespace Chess
{
    public struct Vector2Int
    {
        public int X, Y;
        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        static public Vector2Int Distance(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(Math.Abs(a.X - b.X), Math.Abs(a.Y - b.Y));
        }
   
        public Vector2Int GetStep(Vector2Int target)
        {
            var dist = target - this;
            if (!dist.IsVertical() && !dist.IsHorizontal() && !dist.IsDiagonal())
            {
                return ZERO;
            }
            dist.X = Math.Sign(dist.X);
            dist.Y = Math.Sign(dist.Y);
         
            return dist;
        }

        public bool IsDiagonal()
        {
            return !IsZero() && Math.Abs(X) == Math.Abs(Y);
        }

        public bool IsHorizontal()
        {
            return X != 0 && Y == 0;
        }

        public bool IsVertical()
        {
            return X == 0 && Y != 0;
        }

        public static readonly Vector2Int ZERO = new Vector2Int(0, 0);
        
        public bool IsZero()
        {
            return X == 0 && Y == 0;
        }
        
        public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new Vector2Int(a.X + b.X, a.Y + b.Y);
        public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new Vector2Int(a.X - b.X, a.Y - b.Y);
        public static bool operator ==(Vector2Int a, Vector2Int b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(Vector2Int a, Vector2Int b) => !(a == b);
    }
}