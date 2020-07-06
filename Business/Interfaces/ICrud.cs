using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICrud<TModel> where TModel : class
    {
        IEnumerable<TModel> GetAll();

        TModel GetById(int id);

        Task Add(TModel model);

        void Update(int modelId, TModel model);

        void Delete(int modelId);
    }
}