using Proiect_TW.BusinessLogic.Entities.User;
using Proiect_TW.BusinessLogic.Interfaces;
using Proiect_TW.BusinessLogic;

namespace Proiect_TW.BusinessLogic
{
    public class BussinessLogic
    {

        public ISession GetSessionBL()
        {
            return new SessionBL();
        }
    }
}
