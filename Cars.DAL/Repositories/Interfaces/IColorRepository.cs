using Cars.DAL.Models;

namespace Cars.DAL.Repositories.Interfaces
{
    public interface IColorRepository : IGenericRepository<Color>, IPropCheck<Color>
    {
    }
}
