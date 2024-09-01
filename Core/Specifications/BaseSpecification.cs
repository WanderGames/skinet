using System.Linq.Expressions;
using Core.Interfaces;

namespace Core.Specifications;

//this primary constructor means that when we create a new BaseSpecification 
//we can pass in a expression
public class BaseSpecification<T>(Expression<Func<T, bool>>? criteria) : ISpecification<T>
{
    //empty constructor in case we dont want to use a criteria (where clause)
    protected   BaseSpecification() : this(null) {}

    public Expression<Func<T, bool>>? Criteria => criteria;

    public Expression<Func<T, object>>? OrderBy {get; private set;}

    public Expression<Func<T, object>>? OrderByDescending {get; private set;}

    public bool IsDistinct {get; private set;}

    public int Take {get; private set;}

    public int Skip {get; private set;}

    public bool isPagingEnabled {get; private set;}

    public IQueryable<T> ApplyCriteria(IQueryable<T> query)
    {
        //check if we have a criteria so we can get our total count before we apply pagination
        if(Criteria != null)
        {
            query = query.Where(Criteria);
        }

        return query;
    }

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    protected void ApplyDistinct()
    {
        IsDistinct = true;
    }

    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        isPagingEnabled = true;
    }
}

//this still takes a type of T but returns a TResult so we can use our select
public class BaseSpecification<T, TResult>(Expression<Func<T, bool>>? criteria) : BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
    //empty constructor in case we dont want to use a criteria (where clause)
    protected   BaseSpecification() : this(null) {}

    public Expression<Func<T, TResult>>? Select {get; private set;}

    protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
    {
        Select = selectExpression;
    }
}
