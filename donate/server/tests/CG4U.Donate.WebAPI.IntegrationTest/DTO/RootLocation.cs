using System;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.WebAPI.IntegrationTest.DTO
{
    public class RootLocation : EntityModel<RootLocation>
    {
        public int? idParent { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zipCode { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            var objComp = obj as RootLocation;

            return address.ToLower().Equals(objComp.address.ToLower())
                && city.ToLower().Equals(objComp.city.ToLower())
                && state.ToLower().Equals(objComp.state.ToLower())
                && zipCode.ToLower().Equals(objComp.zipCode.ToLower())
                && Math.Abs(latitude - objComp.latitude) < 0.0000001
                && Math.Abs(longitude - objComp.longitude) < 0.0000001;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (19 * 397) ^ DateTime.Now.GetHashCode();
            }
        }
    }
}
