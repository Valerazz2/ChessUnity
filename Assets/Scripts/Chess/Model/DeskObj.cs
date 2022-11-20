namespace Chess.Model
{
    public class DeskObj
    {
        public Desk Desk { get;}

        protected DeskObj(Desk getDesk)
        {
            Desk = getDesk;
        }
    }
}