using efcoreApi.Models;

namespace efcoreApi.Services
{
    public class GoodsRepository<T> where T : class
    {
        IRepositoryBase<T> repositoryBase;
        public GoodsRepository(IRepositoryBase<T> _repositoryBase) {
        repositoryBase = _repositoryBase;
        }
        //public PagedList<Goods> GetOwners(GoodsParameters ownerParameters)
        //{
        //    return PagedList<Goods>.ToPagedList(repositoryBase.FindAll().OrderBy(on => on.Name),
        //        ownerParameters.PageNumber,
        //        ownerParameters.PageSize);
        //}
    }
}
