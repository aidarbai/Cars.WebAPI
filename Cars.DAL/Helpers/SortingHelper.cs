using Cars.COMMON.ViewModels.Cars;
using Cars.DAL.DbContext;
using Cars.DAL.Models;
using System.Linq;
using static Cars.COMMON.Constants.AppConstants;

namespace Cars.DAL.Helpers
{
    public static class SortingHelper
    {
        public static IQueryable<Car> GetSortedQuery(CarDbContext _db, SortAndPageCarModel model)
        {
            var carsQuery = _db.CarsV2.AsQueryable();

            //TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
            //var sortBy = ti.ToTitleCase(model.SortBy);
            //PropertyInfo prop = typeof(Car).GetProperty(sortBy);
            //var type = prop.PropertyType;

            //carsQuery = model.Order == Order.ASC
            //    ? carsQuery.OrderBy(c => EF.Property<Car>(c, GetSortProperty(prop)))
            //    : carsQuery.OrderByDescending(c => EF.Property<Car>(c, GetSortProperty(prop)));

            carsQuery = model.Order == Order.ASC
                ? carsQuery.OrderBy(model.SortBy)
                : carsQuery.OrderByDescending(model.SortBy);

            return carsQuery;

            //return carsQuery.OrderByDescending(c => EF.Property<int>(c, "Price"));
        }

        //private static string GetSortProperty(PropertyInfo prop) =>
        //    prop is null ? Attributes.PRICE : prop.Name;
    }

    //public static class IQueryableExtensions
    //{
    //    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
    //    {
    //        return source.OrderBy(ToLambda<T>(propertyName));
    //    }

    //    public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
    //    {
    //        return source.OrderByDescending(ToLambda<T>(propertyName));
    //    }

    //    private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
    //    {
    //        var parameter = Expression.Parameter(typeof(T));
    //        var property = Expression.Property(parameter, propertyName);
    //        var propAsObject = Expression.Convert(property, typeof(object));

    //        return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
    //    }
    //}
}