using System.Collections.Generic;
namespace CG4U.Security.ClientApp.Services.Roots
{
    public class RootRegister
    {
        public bool success { get; set; }
        public List<RootIdentityError> errors { get; set; }
    }
}
