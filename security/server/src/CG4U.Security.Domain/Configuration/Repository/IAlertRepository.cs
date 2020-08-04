using System;
using System.Threading.Tasks;
using CG4U.Core.Common.Domain.Interfaces;

namespace CG4U.Security.Domain.Configuration.Repository
{
    public interface IAlertRepository : IRepository<Alert>
    {
        Task AddConnectionPersonGroupAsync(int idPersonGroups, int idAlerts);
    }
}
