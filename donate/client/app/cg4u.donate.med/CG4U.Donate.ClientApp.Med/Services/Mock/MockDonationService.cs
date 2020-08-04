using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using CG4U.Donate.ClientApp.Med.Services.Root;
using Xamarin.Forms;
using System.Reflection;

namespace CG4U.Donate.ClientApp.Med.Services.Mock
{
    public class MockDonationService : Interfaces.IDonationService
    {
        public Task<bool> AddDesiredAsync(RootDesired desired)
        {
            return null;
        }

        public Task<bool> AddGivenAsync(RootGiven given)
        {
            return null;
        }

        public Task<List<RootDonation>> ListDonationsByLanguageAndNameAsync(string query)
        {
            if (query.Equals("anti", StringComparison.CurrentCultureIgnoreCase))
            {
                byte[] bImg;
                var assembly = this.GetType().GetTypeInfo().Assembly;
                var devicePath = Device.RuntimePlatform == Device.iOS ? "iOS" : "Droid";
                var filePath = string.Concat("CG4U.Donate.ClientApp.Med." + devicePath + ".Resources.icon-pill.png");
                using (Stream s = assembly.GetManifestResourceStream(filePath))
                {
                    long length = s.Length;
                    bImg = new byte[length];
                    s.Read(bImg, 0, (int)length);
                }

                return Task.FromResult(
                    new List<RootDonation>()
                    {
                        new RootDonation()
                        {
                            idDonations = 1, idSystems = 1,
                            img = bImg,
                            names = new List<RootDonationName>()
                            {
                                new RootDonationName() { name = "Antialérgicos", idDonationsNames = 1, idDonations = 1, idLanguages = 1 },
                            }
                        },
                        new RootDonation()
                        {
                            idDonations = 2, idDonationsDad = 1, idSystems = 1,
                            img = bImg,
                            names = new List<RootDonationName>()
                            {
                                new RootDonationName() { name = "Loratamed 10mg", idDonationsNames = 2, idDonations = 2, idLanguages = 1 },
                            }
                        },
                        new RootDonation()
                        {
                            idDonations = 3, idDonationsDad = 1, idSystems = 1,
                            img = bImg,
                            names = new List<RootDonationName>()
                            {
                                new RootDonationName() { name = "Levocetirizina 5mg", idDonationsNames = 3, idDonations = 3, idLanguages = 1 },
                            }
                        },
                        new RootDonation()
                        {
                            idDonations = 4, idSystems = 1,
                            img = bImg,
                            names = new List<RootDonationName>()
                            {
                                new RootDonationName() { name = "Antibióticos", idDonationsNames = 4, idDonations = 4, idLanguages = 1 },
                            }
                        },
                        new RootDonation()
                        {
                            idDonations = 5, idDonationsDad = 4, idSystems = 1,
                            img = bImg,
                            names = new List<RootDonationName>()
                            {
                                new RootDonationName() { name = "Amoxicilina", idDonationsNames = 5, idDonations = 5, idLanguages = 1 },
                            }
                        },
                        new RootDonation()
                        {
                            idDonations = 6, idDonationsDad = 4, idSystems = 1,
                            img = bImg,
                            names = new List<RootDonationName>()
                            {
                                new RootDonationName() { name = "Ciprofloxacino", idDonationsNames = 6, idDonations = 6, idLanguages = 1 },
                            }
                        }
                    }
                );
            }
            else
                return Task.FromResult(new List<RootDonation>());
        }
    }
}
