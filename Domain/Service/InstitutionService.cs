using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Service
{
    public partial class Service 
    {
        public Task<object> GetInstitution(string id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
