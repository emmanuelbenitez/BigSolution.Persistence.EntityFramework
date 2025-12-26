#region Copyright & License

// Copyright © 2020 - 2022 Emmanuel Benitez
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

// ReSharper disable once CheckNamespace
namespace System.Linq.Expressions;

/// <summary>
/// Provides extension methods for working with <see cref="Expression"/> objects in the <see cref="System.Linq.Expressions"/> namespace.
/// </summary>
/// <remarks>
/// This static class includes utility methods that simplify or enhance the manipulation of expression trees,
/// such as converting expressions to a generalized return type or other common expression-related tasks.
/// </remarks>
internal static class ExpressionExtensions
{
    /// <summary>
    /// Converts an expression with a specific return type to an expression with a return type of <see cref="object"/>.
    /// </summary>
    /// <typeparam name="TInput">The type of the input parameter of the expression.</typeparam>
    /// <typeparam name="TOutput">The original return type of the expression.</typeparam>
    /// <param name="expression">The expression to convert.</param>
    /// <returns>
    /// A new expression that has the same input parameter as the original expression but returns a value of type <see cref="object"/>.
    /// </returns>
    /// <remarks>
    /// This method is useful when working with expressions that need to be generalized to return an <see cref="object"/> type,
    /// such as when configuring entity keys or other scenarios requiring type conversion.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="expression"/> is <c>null</c>.
    /// </exception>
    public static Expression<Func<TInput, object>> ToObjectExpression<TInput, TOutput>(this Expression<Func<TInput, TOutput>> expression)
    {
        Expression converted = Expression.Convert(expression.Body, typeof(object));
        return Expression.Lambda<Func<TInput, object>>(converted, expression.Parameters);
    }
}