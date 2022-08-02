using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cars.COMMON.ViewModels.Cars;

namespace Cars.DAL.Helpers
{
    public static class PaginationHelper
    {
        public static async Task<List<T>> GetPaginatedList<T>(
            IQueryable<T> query,
            SortAndPageCarModel model)
                where T : class
        {
            model.PageNumber = model.PageNumber < 1 ? 1 : model.PageNumber;

            int count = query.Count();
            int totalPages = (int)Math.Ceiling(decimal.Divide(count, model.PageSize));
            totalPages = totalPages > 0 ? totalPages : 1;

            if (model.PageNumber > totalPages)
            {
                model.PageNumber = totalPages;
            }

            var result = await query.AsSingleQuery()
                                  .Skip((model.PageNumber - 1) * model.PageSize)
                                  .Take(model.PageSize)
                                  .ToListAsync();


            return result;
        }

        //public static async Task<List<T>> GetPaginatedList<T, D>(
        //    IQueryable<D> query,
        //    SortAndPageFilter filter)
        //        where T : class where D : class
        //{
        //    filter.PageNumber = filter.PageNumber < 1 ? 1 : filter.PageNumber;

        //    int count = query.Count();
        //    int totalPages = (int)Math.Ceiling(decimal.Divide(count, filter.PageSize));
        //    totalPages = totalPages > 0 ? totalPages : 1;

        //    if (filter.PageNumber > totalPages)
        //    {
        //        filter.PageNumber = totalPages;
        //    }

        //    var result = await query.AsSingleQuery()
        //                          .Skip((filter.PageNumber - 1) * filter.PageSize)
        //                          .Take(filter.PageSize)
        //                          .ProjectTo<T>()
        //                          .ToListAsync();


        //    return result;
        //}
    }
}
