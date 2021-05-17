using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Extension
{
    public static class LambdaToSqlHelper
    {
        /// <summary>
        /// 从表达式获取sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string GetSqlFromExpression<T>(this Expression<Func<T, bool>> func)
        {
            if (func != null && func.Body is BinaryExpression be)
            {
                return BinarExpressionProvider(be.Left, be.Right, be.NodeType);
            }
            else
            {
                return " ( 1 = 1 ) ";
            }
        }

        /// <summary>
        /// 拆分、拼接sql
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        static string BinarExpressionProvider(Expression left, Expression right, ExpressionType type)
        {
            string sb = "(";
            sb += ExpressionRouter(left);
            sb += ExpressionTypeCast(type);
            string tmpStr = ExpressionRouter(right);
            if (tmpStr == "null")
            {
                if (sb.EndsWith(" =")) sb = sb.Substring(0, sb.Length - 2) + " is null";
                else if (sb.EndsWith("<>")) sb = sb.Substring(0, sb.Length - 2) + " is not null";
            }
            else sb +=tmpStr;
            return sb += ")";
        }

        /// <summary>
        /// 拆分、拼接sql
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        static string ExpressionRouter(Expression exp)
        {
            string sb = string.Empty;
            if (exp is BinaryExpression be)
            {
                return BinarExpressionProvider(be.Left, be.Right, be.NodeType);
            }
            else if (exp is NewArrayExpression ae)
            {
                StringBuilder tmpstr = new StringBuilder();
                foreach (Expression ex in ae.Expressions)
                {
                    tmpstr.Append(ExpressionRouter(ex));
                    tmpstr.Append(",");
                }
                return tmpstr.ToString(0, tmpstr.Length - 1);
            }
            else if (exp is MethodCallExpression mce)
            {
                var attributeData = mce.Method.GetCustomAttributes(typeof(ToSqlFormat), false).First();
                return string.Format(((ToSqlFormat)attributeData).Format, ExpressionRouter(mce.Arguments[0]), ExpressionRouter(mce.Arguments[1]));
            }
            else if (exp is ConstantExpression ce)
            {
                if (ce.Value == null)
                    return "null";
                else if (ce.Value is ValueType)
                    return ce.Value.ToString();
                else if (ce.Value is string || ce.Value is DateTime || ce.Value is char)
                    return string.Format("'{0}'", ce.Value.ToString());
            }
            else if (exp is UnaryExpression)
            {
                UnaryExpression ue = ((UnaryExpression)exp);
                return ExpressionRouter(ue.Operand);
            }
            else if (exp is MemberExpression me)
            {
                return MemberExpressionProvider(exp);
            }
            return null;
        }
        private static string MemberExpressionProvider(Expression exp)
        {
            if (!exp.ToString().StartsWith("value"))
            {
                var em = exp as MemberExpression;
                return em.Member.Name;
            }
            else
            {
                var result = Expression.Lambda(exp).Compile().DynamicInvoke();
                return $"'{result.ToString()}'";
            }
        }
        /// <summary>
        /// 介绍表达式树节点的节点类型 转换为 sql关键字
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static string ExpressionTypeCast(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.And:
                    return " & ";
                case ExpressionType.AndAlso:
                    return " AND ";
                case ExpressionType.Equal:
                    return " =";
                case ExpressionType.GreaterThan:
                    return " >";
                case ExpressionType.GreaterThanOrEqual:
                    return ">=";
                case ExpressionType.LessThan:
                    return "<";
                case ExpressionType.LessThanOrEqual:
                    return "<=";
                case ExpressionType.NotEqual:
                    return "<>";
                case ExpressionType.Or:
                    return " | ";
                case ExpressionType.OrElse:
                    return " Or ";
                case ExpressionType.Add:
                case ExpressionType.AddChecked:
                    return "+";
                case ExpressionType.Subtract:
                case ExpressionType.SubtractChecked:
                    return "-";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Multiply:
                case ExpressionType.MultiplyChecked:
                    return "*";
                default:
                    return null;
            }
        }

        [ToSqlFormat("{0} IN ({1})")]
        public static bool In<T>(this T obj, T[] array)
        {
            return true;
        }
        [ToSqlFormat("{0} NOT IN ({1})")]
        public static bool NotIn<T>(this T obj, T[] array)
        {
            return true;
        }
        [ToSqlFormat("{0} LIKE {1}")]
        public static bool Like(this string str, string likeStr)
        {
            return true;
        }
        [ToSqlFormat("{0} NOT LIKE {1}")]
        public static bool NotLike(this string str, string likeStr)
        {
            return true;
        }
    }
    public class ToSqlFormat : Attribute
    {
        public string Format { get; set; }
        public ToSqlFormat(string str)
        {
            Format = str;
        }

    }
}
