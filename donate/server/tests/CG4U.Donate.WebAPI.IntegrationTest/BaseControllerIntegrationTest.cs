using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using CG4U.Core.Services.ViewModels;
using CG4U.Donate.WebAPI.IntegrationTest.DTO;
using CG4U.Donate.WebAPI.ViewModels;
using Microsoft.Extensions.Configuration;

namespace CG4U.Donate.WebAPI.IntegrationTest
{
    public abstract class BaseControllerIntegrationTest
    {
        protected List<UserViewModel> listUsers;
        protected LocationViewModel locationBorbaGato;
        protected LocationViewModel locationOmarCardoso;
        protected LocationViewModel locationBoaVistaShopping;
        protected LocationViewModel locationIpiranga;
        protected List<RootDonation> listRootDonations;
        protected List<DesiredViewModel> listDesireds;
        protected List<GivenViewModel> listGivens;

        public BaseControllerIntegrationTest()
        {
            Environment.SetupEnvironment();

            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();

            listUsers = new List<UserViewModel>();
            root.GetSection("Logins").Bind(listUsers);

            locationBorbaGato = new LocationViewModel
            {
                Id = 1,
                IdParent = 1,
                Address = "Rua Borba Gato 331 Ap 32 Torre J",
                City = "São Paulo",
                State = "SP",
                ZipCode = "04747030",
                Latitude = (decimal)-23.6565581,
                Longitude = (decimal)-46.7001006,
                Active = 1
            };

            locationBoaVistaShopping = new LocationViewModel
            {
                Id = 2,
                IdParent = 2,
                Address = "Rua Borba Gato 59",
                City = "São Paulo",
                State = "SP",
                ZipCode = "04747030",
                Latitude = (decimal)-23.6547989,
                Longitude = (decimal)-46.7010411,
                Active = 1
            };

            locationOmarCardoso = new LocationViewModel
            {
                Id = 3,
                Address = "Rua Omar Cardoso 10",
                City = "São Paulo",
                State = "SP",
                ZipCode = "04747030",
                Latitude = (decimal)-23.6565648,
                Longitude = (decimal)-46.7001215,
                Active = 1
            };

            locationIpiranga = new LocationViewModel
            {
                Id = 4,
                Address = "Rua Agostinho Gomes 2084 Casa 4 Ipiranga",
                City = "São Paulo",
                State = "SP",
                ZipCode = "04206900",
                Latitude = (decimal)-23.5860643,
                Longitude = (decimal)-46.6042267,
                Active = 1
            };

            listRootDonations = new List<RootDonation>()
            {
                new RootDonation
                {
                    Id = 1,
                    idDonations = 1,
                    idSystems = 1,
                    idDonationsDad = null,
                    img = null,
                    Active = 1,
                    names = new List<RootDonationName>()
                    {
                        new RootDonationName()
                        {
                            Id = 1,
                            idDonationsNames = 1,
                            idDonations = 1,
                            idLanguages = 1,
                            name = "Analgésicos e Antitérmicos",
                            Active = 1
                        }
                    }
                },
                new RootDonation
                {
                    Id = 2,
                    idDonations = 2,
                    idSystems = 1,
                    idDonationsDad = null,
                    img = null,
                    Active = 1,
                    names = new List<RootDonationName>()
                    {
                        new RootDonationName()
                        {
                            Id = 4,
                            idDonationsNames = 4,
                            idDonations = 2,
                            idLanguages = 1,
                            name = "Antibióticos",
                            Active = 1
                        }
                    }
                },
                new RootDonation
                {
                    Id = 3,
                    idDonations = 3,
                    idSystems = 1,
                    idDonationsDad = null,
                    img = null,
                    Active = 1,
                    names = new List<RootDonationName>()
                    {
                        new RootDonationName()
                        {
                            Id = 7,
                            idDonationsNames = 7,
                            idDonations = 3,
                            idLanguages = 1,
                            name = "Tylenol Sinus",
                            Active = 1
                        }
                    }
                }
            };

            listDesireds = new List<DesiredViewModel>()
            {
                new DesiredViewModel
                {
                    Id = 1,
                    IdDonationsDesired = 1,
                    Donation = Mapper.Map<RootDonation, DonationViewModel>(listRootDonations[0]),
                    User = listUsers[1],
                    DtUpdate = DateTime.Now,
                    Location = locationBorbaGato,
                    MaxGetinMeters = 5000,
                    Active = 1
                },
                new DesiredViewModel
                {
                    Id = 2,
                    IdDonationsDesired = 2,
                    Donation = Mapper.Map<RootDonation, DonationViewModel>(listRootDonations[1]),
                    User = listUsers[1],
                    DtUpdate = DateTime.Now,
                    Location = locationBorbaGato,
                    MaxGetinMeters = 12600,
                    Active = 1
                },
                new DesiredViewModel
                {
                    Id = 3,
                    IdDonationsDesired = 3,
                    Donation = Mapper.Map<RootDonation, DonationViewModel>(listRootDonations[2]),
                    User = listUsers[1],
                    DtUpdate = DateTime.Now,
                    Location = locationBorbaGato,
                    MaxGetinMeters = 11000,
                    Active = 1
                }
            };
            listDesireds[0].Location.IdParent = listDesireds[0].Id;
            listDesireds[1].Location.IdParent = listDesireds[1].Id;
            listDesireds[2].Location.IdParent = listDesireds[2].Id;

            listGivens = new List<GivenViewModel>()
            {
                new GivenViewModel
                {
                    Id = 1,
                    IdDonationsGivens = 1,
                    Donation = Mapper.Map<RootDonation, DonationViewModel>(listRootDonations[0]),
                    User = listUsers[2],
                    DtUpdate = DateTime.Now,
                    Location = locationIpiranga,
                    MaxLetinMeters = 1000,
                    Active = 1
                },
                new GivenViewModel
                {
                    Id = 2,
                    IdDonationsGivens = 2,
                    Donation = Mapper.Map<RootDonation, DonationViewModel>(listRootDonations[1]),
                    User = listUsers[2],
                    DtUpdate = DateTime.Now,
                    Location = locationIpiranga,
                    MaxLetinMeters = 12600,
                    Active = 1
                },
                new GivenViewModel
                {
                    Id = 3,
                    IdDonationsGivens = 3,
                    Donation = Mapper.Map<RootDonation, DonationViewModel>(listRootDonations[1]),
                    User = listUsers[0],
                    DtUpdate = DateTime.Now,
                    Location = locationBoaVistaShopping,
                    MaxLetinMeters = 5000,
                    Active = 1
                }
            };
            listGivens[0].Location.IdParent = listGivens[0].Id;
            listGivens[1].Location.IdParent = listGivens[1].Id;
            listGivens[2].Location.IdParent = listGivens[2].Id;
        }

        protected async Task<RootLogin> GetLoginUserAsync(int id)
        {
            return await UserUtils.DoLogin(Environment.ClientApiAuth, listUsers[id]);
        }
    }
}
