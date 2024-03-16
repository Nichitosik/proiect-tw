using Proiect_TW.BussinesLogic.Core;
using Proiect_TW.BussinesLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proiect_TW.Domain.Entities.User;
using Proiect_TW.BussinesLogic.Entities.User;

namespace Proiect_TW.BussinesLogic
{
    public class SessionBL : UserAPI, ISession
    {
        public ULoginResp UserLogin(ULoginData data)
        {
            throw new NotImplementedException();
        }
    }
}