using Dx.Core.API.Interfaces;

namespace Dx.AdminUtilities.Features.Admin.Interfaces;

public interface IAdminRepository : IRepository<AdminProfile>
{
    void Load();

    void Save();
}