using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CG4U.Core.Services.ViewModels;
using CG4U.Security.WebAPI.IntegrationTest.DTO;
using CG4U.Security.WebAPI.ViewModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CG4U.Security.WebAPI.IntegrationTest.Configs
{
    public class PersonsLoadable : BaseConfigurationLoadable, IConfigurationLoadable<List<PersonViewModel>>
    {
        public PersonsLoadable(RootLogin login)
            : base(login)
        {
        }

        public async Task<List<PersonViewModel>> GetResultLoaded(IConfigurationRoot config)
        {
            var persons = new List<PersonViewModel>();
            config.GetSection("Persons").Bind(persons);
            foreach (var person in persons)
            {
                using (var postContent = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json"))
                {
                    var requestUriGet = $"Person/GetByIdAsync/{person.Id}";
                    var postUri = "Person/AddPersonAsync";

                    var isNewItem = await IsNewItemAddedAsync(requestUriGet, postUri, postContent);
                    if (isNewItem)
                    {
                        await AddPersonFace(person.Id);
                    }
                }
            }
            return persons;
        }

        private async Task AddPersonFace(int idPerson)
        {
            foreach (string personFaceFilePath in Directory.EnumerateFiles(
                $"Img/Faces/{idPerson}", "*", SearchOption.AllDirectories)
            )
            {
                using (var sc = new StreamContent(File.OpenRead(personFaceFilePath)))
                using (var ms = new MemoryStream())
                {
                    await sc.CopyToAsync(ms);
                    var faceViewModel = new FaceViewModel()
                    {
                        IdPersons = idPerson,
                        Image = ms.ToArray()
                    };

                    using (var postContent = new StringContent(JsonConvert.SerializeObject(faceViewModel), Encoding.UTF8, "application/json"))
                    using (var response = await Environment.ServerApiSecurity
                        .CreateRequest("Person/AddPersonFaceAsync")
                        .AddHeader("Authorization", "Bearer " + _login.access_token)
                        .And(request => request.Content = postContent)
                        .And(request => request.Method = HttpMethod.Post)
                        .PostAsync())
                    {
                        response.EnsureSuccessStatusCode();
                    }
                }
            }            
        }
    }
}
