using System;
namespace CG4U.Security.WebAPI.IntegrationTest.DTO
{
    public class RootLogin
    {
        public string access_token { get; set; }
        public DateTime expires_in { get; set; }
        public RootUser user { get; set; }
    }
}
