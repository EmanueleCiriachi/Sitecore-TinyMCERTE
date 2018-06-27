using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TinyMCERTE {
    public static class ExtensionMethods {
        public static String nameof<T, TT>(this Expression<Func<T, TT>> accessor) {
            return nameof(accessor.Body);
        }

        public static String nameof<T>(this Expression<Func<T>> accessor) {
            return nameof(accessor.Body);
        }

        public static String nameof<T, TT>(this T obj, Expression<Func<T, TT>> propertyAccessor) {
            return nameof(propertyAccessor.Body);
        }

        public static string Left(this string value, int maxLength) {
            if (string.IsNullOrEmpty(value)) return value;
            maxLength = Math.Abs(maxLength);

            return (value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength)
                   );
        }

        public static string FormatWith(this string value, params object[] args) {
            return String.Format(value, args);
        }

        private static String nameof(Expression expression) {
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                var memberExpression = expression as MemberExpression;
                if (memberExpression == null)
                    return null;
                return memberExpression.Member.Name;
            }
            return null;
        }
    }
}
