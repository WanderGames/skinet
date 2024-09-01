using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class SpecificationEvaluator<T> where T : BaseEntity
{
    //build up our query and return it
    public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
    {
        if(spec.Criteria != null)
        {
            //this is effectively (x => x.Brand == brand) where brand is the criteria passed in
            query = query.Where(spec.Criteria);
        }

        if(spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if(spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        if(spec.IsDistinct)
        {
            query = query.Distinct();
        }

        if (spec.isPagingEnabled)
        {
            //tell our query to skip the first x items and take the next y
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        return query;
    }

    //this allows us to return a tresult wich is our select so we can return something other than a type of T
    public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query, ISpecification<T, TResult> spec)
    {
        if(spec.Criteria != null)
        {
            //this is effectively (x => x.Brand == brand) where brand is the criteria passed in
            query = query.Where(spec.Criteria);
        }

        if(spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        if(spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        
        var selectQuery = query as IQueryable<TResult>;

        if(spec.Select != null)
        {
            selectQuery = query.Select(spec.Select);
        }

        if(spec.IsDistinct)
        {
            selectQuery = selectQuery?.Distinct();
        }

        if (spec.isPagingEnabled)
        {
            //tell our query to skip the first x items and take the next y
            selectQuery = selectQuery?.Skip(spec.Skip).Take(spec.Take);
        }

        //return selectquery or if null then return query cast to TResult
        return selectQuery ?? query.Cast<TResult>();
    }
}
