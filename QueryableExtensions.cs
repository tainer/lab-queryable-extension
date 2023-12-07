namespace QueryableExtensions;
using System;
using System.Linq;
using System.Linq.Expressions;

public static class QueryableExtensions
{
    public static IQueryable<TResult> Select<TResult>(this IQueryable query)
    {
        if (query == null)
            throw new ArgumentNullException(nameof(query));

        var sourceType = query.ElementType;
        var resultType = typeof(TResult);

        // Criar um parâmetro de expressão
        var parameter = Expression.Parameter(sourceType, "x");

        // Criar uma expressão de mapeamento para cada propriedade em TResult
        var propertyBindings = resultType.GetProperties()
            .Select(property => Expression.Bind(property, Expression.PropertyOrField(parameter, property.Name)))
            .OfType<MemberBinding>();

        // Criar uma expressão de inicialização usando o construtor padrão de TResult
        var memberInit = Expression.MemberInit(Expression.New(resultType), propertyBindings);

        // Criar uma expressão lambda
        var lambda = Expression.Lambda(memberInit, parameter);

        // Chamar o método de seleção na fonte
        var selectCall = Expression.Call(
            typeof(Queryable),
            "Select",
            new[] { sourceType, resultType },
            query.Expression,
            lambda
        );

        // Criar e retornar a consulta resultante
        return query.Provider.CreateQuery<TResult>(selectCall);
    }
}
