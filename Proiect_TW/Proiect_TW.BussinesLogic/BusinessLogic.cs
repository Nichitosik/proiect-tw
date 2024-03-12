using Proiect_TW.BussinesLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proiect_TW.BussinesLogic
{
    public class BussinessLogic
    {
        public ISession GetSession()
        {
            throw new NotImplementedException();
        }

        public ISession GetSessionBL()
        {
            return new SessionBL();
        }
    }
}
