using Proiect_TW.BussinesLogic.Entities.User;
using Proiect_TW.BussinesLogic.Interfaces;

namespace Proiect_TW.BussinesLogic
{
    public class BussinessLogic
    {

        public ISession GetSessionBL()
        {
            return new SessionBL();
        }
    }

}
