using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CG4U.Core.Services.Authorization
{
    public class SigningCredentialsConfiguration
    {
        private const string SecretKey = "CG4U.Auth.Infra.CrossCutting.Authorization.SigningCredentialsConfiguration.9b851c61-6777-4259-b750-7ef26ab5c764.HeyDeVencer";
        public static readonly SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        public SigningCredentials SigningCredentials { get; }

        public SigningCredentialsConfiguration()
        {
            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}
