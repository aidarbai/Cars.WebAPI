using Cars.DAL.Models;

namespace Cars.DAL.Repositories.Interfaces
{
    public interface IMakeRepository : IGenericRepository<Make>, IPropCheck<Make>
    {
    }
}
