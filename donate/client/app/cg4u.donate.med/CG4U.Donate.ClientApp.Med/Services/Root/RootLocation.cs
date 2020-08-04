using System;
namespace CG4U.Donate.ClientApp.Med.Services.Root
{
    public class RootLocation : RootEntity<RootLocation>
    {
        public int? idParent { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
