using System;

namespace Chess.Model
{
    public class ServerPlayer
    {
        public readonly ChessColor Color;
        
        public readonly string ID = Guid.NewGuid().ToString();

        public readonly ChessGame game;

        public ServerPlayer(ChessGame game, ChessColor color)
        {
            this.game = game;
            Color = color;
        }
    }
}