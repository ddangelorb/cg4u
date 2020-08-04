using System.Collections.Generic;

namespace CG4U.Donate.ClientApp.Med.Services.Root
{
    public class RootRegister
    {
        public bool success { get; set; }
        public List<RootIdentityError> errors { get; set; }
    }
}
