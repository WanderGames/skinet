using System.Linq.Expressions;

namespace Core.Interfaces;

//list of all methods we want our specification psttern to support
public interface ISpecification<T> 
{
    //where query
    //takes a type of T and returns a bool
    Expression<Func<T, bool>>? Criteria { get; }

    //order by
    Expression<Func<T, object>>? OrderBy { get; }
    //order by descending
    Expression<Func<T, object>>? OrderByDescending { get; }
    bool IsDistinct { get; }
    int Take {get;}
    int Skip {get;}
    bool isPagingEnabled {get;}
    IQueryable<T> ApplyCriteria(IQueryable<T> query);

}

//this still takes a type of T but returns a TResult
public interface ISpecification<T, TResult> : ISpecification<T>
{
    //our select
    Expression<Func<T, TResult>>? Select { get; }
}
