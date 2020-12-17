using Domain.ApiModels;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataCore.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly DataContext _context;

        public IdentityRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<User> CheckLoginAsync(LoginApiModel NewLogin, CancellationToken ct = new CancellationToken())
        {
            return await _context.Users.Where(a => a.Username == NewLogin.UserName).FirstOrDefaultAsync();
        }

        public async Task<UserWithRoleApiModel> FetchUserWithRole(Guid userID, CancellationToken ct = new CancellationToken())
        {
            UserWithRoleApiModel llsit = await (from t in _context.Users
                                                join acc in _context.UserRoles on t.Id equals acc.UserID
                                                join r in _context.Roles on acc.RoleID equals r.Id
                                                where t.Identifier == userID
                                                select new UserWithRoleApiModel()
                                                {
                                                    Id = t.Id,
                                                    Email = t.Email,
                                                    Identifier = t.Identifier,
                                                    Role = r.Type
                                                }).FirstOrDefaultAsync();
            return llsit;
        }

        public async Task<RefreshToken> AddAsync(RefreshToken newRefreshToken, CancellationToken ct = new CancellationToken())
        {
            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync(ct);
            return newRefreshToken;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
