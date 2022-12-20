namespace Chess.Server
{
    public interface IChessService
    {
        JoinResult Join();

        void SelectSquare(string sid, string squareRef);
    }
}
