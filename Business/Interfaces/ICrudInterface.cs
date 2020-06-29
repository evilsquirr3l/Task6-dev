using System.Collections.Generic;

namespace Business.Interfaces
{
    public interface ICrudInterface<TModel> where TModel : class
    {
        IEnumerable<TModel> GetAll();

        TModel GetById(int id);

        void Add(TModel model);

        void Update(int modelId, TModel model);

        void Delete(int modelId);
    }
}