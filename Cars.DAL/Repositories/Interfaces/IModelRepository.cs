using Cars.DAL.Models;
using System.Threading.Tasks;

namespace Cars.DAL.Repositories.Interfaces
{
    public interface IModelRepository: IGenericRepository<Model>
    {
        Task<Model> CheckPropAsync(string model, Make make);
    }
}
