using Cars.DAL.Models;
using System.Threading.Tasks;

namespace Cars.BLL.Services.Interfaces
{
    public interface IModelService
    {
        Task<Model> CheckModelAsync(string model, Make make);
    }
}
