#region Copyright & License

// Copyright © 2020 - 2021 Emmanuel Benitez
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
namespace System.Linq.Expressions
{
    internal static class ExpressionExtensions
    {
        public static Expression<Func<TInput, object>> ToObjectExpression<TInput, TOutput>(this Expression<Func<TInput, TOutput>> expression)
        {
            Expression converted = Expression.Convert(expression.Body, typeof(object));
            return Expression.Lambda<Func<TInput, object>>(converted, expression.Parameters);
        }
    }
}
