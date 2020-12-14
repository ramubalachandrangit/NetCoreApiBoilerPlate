using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Service
{
    public interface IService
    {
        Task<object> GetStudent(string id, CancellationToken ct = new CancellationToken());

        Task<object> GetInstitution(string id, CancellationToken ct = new CancellationToken());
    }
}
