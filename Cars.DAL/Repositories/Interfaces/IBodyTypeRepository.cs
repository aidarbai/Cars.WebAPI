using Cars.DAL.Models;

namespace Cars.DAL.Repositories.Interfaces
{
    public interface IBodyTypeRepository : IGenericRepository<BodyType>, IPropCheck<BodyType>
    {
    }
}
