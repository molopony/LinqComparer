using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace System.Linq.Compare
{
    public class MemberSelector<T>
    {
        public List<MemberInfo> Members { get; } = new List<MemberInfo>();

        public MemberSelector<T> Clear()
        {
            Members.Clear();
            return this;
        }

        public MemberSelector<T> Add(Expression<Func<T, object>> memberExpression)
        {
            var memberInfo = GetMemberFromExpression(memberExpression);
            if (!Members.Contains(memberInfo))
            {
                Members.Add(memberInfo);
            }

            return this;
        }

        public MemberSelector<T> All()
        {
            Clear();

            Members.AddRange(
                typeof(T).GetProperties()
                .OfType<MemberInfo>()
                .Union(typeof(T).GetFields()));

            return this;
        }

        public MemberSelector<T> Remove(Expression<Func<T, object>> memberExpression)
        {
            var memberInfo = GetMemberFromExpression(memberExpression);
            while (Members.Contains(memberInfo))
            {
                Members.Remove(memberInfo);
            }

            return this;
        }

        private MemberInfo GetMemberFromExpression(Expression<Func<T, object>> expression)
        {
            MemberExpression Exp = null;

            if (expression.Body is UnaryExpression)
            {
                var UnExp = (UnaryExpression)expression.Body;
                if (UnExp.Operand is MemberExpression)
                {
                    Exp = (MemberExpression)UnExp.Operand;
                }
                else
                    throw new ArgumentException();
            }
            else if (expression.Body is MemberExpression)
            {
                Exp = (MemberExpression)expression.Body;
            }
            else
            {
                throw new ArgumentException();
            }

            if (!(Exp.Member is FieldInfo || Exp.Member is PropertyInfo))
            {
                throw new ArgumentException("expression should be a property or field expression");
            }

            return Exp.Member;
        }

        internal bool Equal(T sourceItem, T targetItem)
        {
            var areEqual = true;

            foreach (var member in Members)
            {
                object sourceValue;
                object targetValue;

                if (member is PropertyInfo propertyInfo)
                {
                    sourceValue = propertyInfo.GetValue(sourceItem);
                    targetValue = propertyInfo.GetValue(targetItem);
                }
                else if (member is FieldInfo fieldInfo)
                {
                    sourceValue = fieldInfo.GetValue(sourceItem);
                    targetValue = fieldInfo.GetValue(targetItem);
                }
                else
                {
                    throw new NotSupportedException($"Type type {member?.GetType().Name} not supported");
                }

                if (sourceValue == null)
                {
                    areEqual = (targetValue == null);
                }
                else if (targetValue != null)
                {
                    areEqual = sourceValue.Equals(targetValue);
                }

                if (areEqual == false)
                {
                    break;
                }
            }

            return areEqual;
        }
    }
}
