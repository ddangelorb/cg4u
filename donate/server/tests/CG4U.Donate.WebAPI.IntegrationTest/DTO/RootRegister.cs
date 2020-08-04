using System.Collections.Generic;

namespace CG4U.Donate.WebAPI.IntegrationTest.DTO
{
    public class RootRegister
    {
        public bool success { get; set; }
        public List<RootIdentityError> errors { get; set; }
    }
}
