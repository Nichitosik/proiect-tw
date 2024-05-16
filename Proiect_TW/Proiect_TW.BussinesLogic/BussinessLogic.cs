using Proiect_TW.BusinessLogic.Entities.User;
using Proiect_TW.BusinessLogic.Interfaces;
using Proiect_TW.BusinessLogic;
using Proiect_TW.BussinesLogic.Interfaces;
using Proiect_TW.BussinesLogic;

namespace Proiect_TW.BusinessLogic
{
    public class BussinessLogic
    {

        public ISession GetSessionBL()
        {
            return new SessionBL();
        }
        public ISessionAdmin GetSessionAdmin()
        {
            return new SessionAdmin();
        }
    }
}
