using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Domain.LocalizationExtensions
{
    public static class AttributesUtil
    {
        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            return GetAttributes<T>(type).FirstOrDefault();
        }

        public static IEnumerable<T> GetAttributes<T>(this Type type) where T : Attribute
        {
            foreach (T a in type.GetCustomAttributes(typeof(T), false))
            {
                yield return a;
            }
        }

        public static T GetAttribute<T>(this MemberInfo member) where T : Attribute
        {
            return GetAttributes<T>(member).FirstOrDefault();
        }

        public static IEnumerable<T> GetAttributes<T>(this MemberInfo member) where T : Attribute
        {
            foreach (T a in member.GetCustomAttributes(typeof(T), false))
            {
                yield return a;
            }
        }

        public static T GetTypeAttribute<T>(this Type type) where T : Attribute
        {
            var attribute = GetAttribute<T>(type);

            if (attribute == null)
            {
                foreach (var baseInterface in type.GetInterfaces())
                {
                    attribute = GetTypeAttribute<T>(baseInterface);
                    if (attribute != null)
                    {
                        break;
                    }
                }
            }

            return attribute;
        }

        public static T[] GetTypeAttributes<T>(Type type) where T : Attribute
        {
            var attributes = GetAttributes<T>(type).ToArray();

            if (attributes.Length == 0)
            {
                foreach (var baseInterface in type.GetInterfaces())
                {
                    attributes = GetTypeAttributes<T>(baseInterface);
                    if (attributes.Length > 0)
                    {
                        break;
                    }
                }
            }

            return attributes;
        }

        public static AttributeUsageAttribute GetAttributeUsage(this Type attributeType)
        {
            var attributes = attributeType.GetCustomAttributes<AttributeUsageAttribute>(true).ToArray();
            return attributes.Length != 0 ? attributes[0] : DefaultAttributeUsage;
        }

        private static readonly AttributeUsageAttribute DefaultAttributeUsage = new AttributeUsageAttribute(AttributeTargets.All);

        public static Type GetTypeConverter(MemberInfo member)
        {
            var attrib = GetAttribute<TypeConverterAttribute>(member);

            if (attrib != null)
            {
                return Type.GetType(attrib.ConverterTypeName);
            }

            return null;
        }
    }
}