using Cars.DAL.Models;
using System.Threading.Tasks;

namespace Cars.DAL.Repositories.Interfaces
{
    public interface IPropCheck<T> where T : class
    {
        Task<T> CheckPropAsync(string prop);
    }
}
