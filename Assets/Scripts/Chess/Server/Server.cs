using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Chess.Server;

namespace Chess.Model
{
    public class Server : IChessService
    {
        private Dictionary<string, ServerPlayer> dictionary = new Dictionary<string, ServerPlayer>();
        
        private ServerPlayer waitingPlayer;
        
        readonly object joinLock = new object();

        public int GameCount => dictionary.Count / 2;

        public JoinResult Join()
        {
            lock (joinLock)
            {
                var game = waitingPlayer == null ? new ChessGame() : waitingPlayer.game;
                var color = waitingPlayer == null ? ChessColor.Black : ChessColor.White;
                var player = new ServerPlayer(game, color);
                dictionary.Add(player.ID, player);
                if (waitingPlayer == null)
                {
                    waitingPlayer = player;
                    game.PlayerWhite = player;
                }
                else
                {
                    waitingPlayer = null;
                    game.PlayerBlack = player;
                }

                return new JoinResult
                {
                    Sid = player.ID,
                    Color = player.Color
                };
            }
        }

        public void SelectSquare(string sid, string squareRef)
        {
            ServerPlayer player;
            lock (dictionary)
            {
                if (!dictionary.TryGetValue(sid, out player))
                {
                    throw new Exception("Player not found");
                }
            }

            var desk = player.game.Desk;
            lock (desk)
            {
                var square = desk.GetSquareAt(new Vector2Int(squareRef[0], squareRef[1]));
              
                if (desk.move == player.Color)
                {
                    desk.Select(square);
                }
            }
        }
    }
}
