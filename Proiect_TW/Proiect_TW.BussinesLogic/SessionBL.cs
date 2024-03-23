using Proiect_TW.BusinessLogic.Core;
using Proiect_TW.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proiect_TW.Domain.Entities.User;
using Proiect_TW.BusinessLogic.Entities.User;

namespace Proiect_TW.BusinessLogic
{
    public class SessionBL : UserAPI, ISession
    {
        public ULoginResp UserLogin(ULoginData data)
        {
            throw new NotImplementedException();
        }
    }
}