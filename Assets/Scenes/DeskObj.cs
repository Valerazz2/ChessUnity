using UnityEngine;

namespace Chess
{
    public class DeskObj
    {
        protected Desk Desk;

        protected DeskObj(Desk getDesk)
        {
            Desk = getDesk;
        }
    }
}