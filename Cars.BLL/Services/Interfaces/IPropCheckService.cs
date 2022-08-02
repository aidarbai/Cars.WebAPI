using System.Threading.Tasks;

namespace Cars.BLL.Services.Interfaces
{
    public interface IPropCheckService<T> where T : class
    {
        Task<T> CheckPropAsync(string prop);
    }
}
