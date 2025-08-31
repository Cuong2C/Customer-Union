using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAuthentication
{
    public class ClientSourceAuthenticationHandler : IClientSourceAuthenticationHandler
    {
        public bool Validate(string clientSource)
        {

            return true;
        }
    }

}
