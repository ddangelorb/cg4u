using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CG4U.Security.WebAPI.IntegrationTest.Configs
{
    public interface IConfigurationLoadable<T>
    {
        Task<T> GetResultLoaded(IConfigurationRoot config);
    }
}
