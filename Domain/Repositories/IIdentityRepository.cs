using Domain.ApiModels;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IIdentityRepository : IDisposable
    {
        Task<User> CheckLoginAsync(LoginApiModel NewLogin, CancellationToken ct = new CancellationToken());

        Task<UserWithRoleApiModel> FetchUserWithRole(Guid userID, CancellationToken ct = new CancellationToken());

        Task<RefreshToken> AddAsync(RefreshToken newRefreshToken, CancellationToken ct = new CancellationToken());
    }
}
