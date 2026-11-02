using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities1.Exceptions1
{
    public abstract class BadRequest : Exception
    {
        protected BadRequest(string message) :base(message) 
        {
            
        }
    }
}
