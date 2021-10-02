using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TFI.PrimerParcial.Source.Repository.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }

        List<Expression<Func<T, object>>> Includes { get; }

        Expression<Func<T, bool>> OrderBy { get; }

        Expression<Func<T, bool>> OrderByDescending { get; }

        int Take { get; }

        int Skip { get; }

        bool IsPagingEnabled { get; }
    }
}
