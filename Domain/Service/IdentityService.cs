using Domain.ApiModels;
using Domain.Constants;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utility;

namespace Domain.Service
{
    public partial class Service
    {
        public async Task<AuthenticationResultApiModel> ValidateUser(LoginApiModel credentials, string secret,TimeSpan tokenLifeTime, CancellationToken ct = new CancellationToken())
        {

            Cryptography crpt = new Cryptography();
            User userRes = await _identityRepository.CheckLoginAsync(credentials, ct);
            if (userRes != null)
            {

                string Dpassword = crpt.DecryptText(userRes.Password, secret, userRes.Salt);
                if (Dpassword.Equals(credentials.Password))
                {
                    string randNumber = this.RandomString(6);// generate 6 digit alphanumeric string like W03UJD   
                    return await GenerateAuthenticationResult(userRes.Identifier.ToString(), secret, tokenLifeTime, ct);

                }
                else
                {
                    return new AuthenticationResultApiModel { ErrorMessage = new List<string> { ValidationMessage.InvalidUser }, Success = false };
                }
            }
            else
            {
                return new AuthenticationResultApiModel { Success = false, ErrorMessage = new List<string> { ValidationMessage.InvalidUser } };
            }
        }


        private async Task<AuthenticationResultApiModel> GenerateAuthenticationResult(string userId, string secret, TimeSpan tokenLifeTime, CancellationToken ct)
        {
            UserWithRoleApiModel userRes = await _identityRepository.FetchUserWithRole(Guid.Parse(userId), ct);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,userRes.Email),
                    new Claim(JwtRegisteredClaimNames.Email,userRes.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,  Guid.NewGuid().ToString()),
                    new Claim("Id", userRes.Identifier.ToString()),
                    new Claim("role",userRes.Role)
                }),
                Expires = DateTime.UtcNow.Add(tokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = this.GenerateBitRandomNumber(32);
            var newRefreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = userId,
                ExpiredDate = DateTime.UtcNow.AddDays(1),
                Token = refreshToken
            };
            await _identityRepository.AddAsync(newRefreshToken, ct);
            return new AuthenticationResultApiModel
            {

                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken,
                Success = true
            };
        }

        public string RandomString(int size)
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", ""); 
            return path.Substring(0, size).ToUpper();
        }

        public string GenerateBitRandomNumber(int size)
        {
            var randomNumber = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }


}
