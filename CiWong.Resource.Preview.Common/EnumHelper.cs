using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CiWong.Resource.Preview.Common
{
    public static class EnumHelper
    {
        /// <summary>
        /// 获取Description特性的说明
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static object GetDescription(this ICustomAttributeProvider provider)
        {
            var attributes = (DescriptionAttribute[])provider.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes == null || attributes.Length == 0) return String.Empty;
            return attributes.FirstOrDefault().Description;
        }

        /// <summary>
        /// 获取指定枚举的描述内容
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            return value.GetType().GetField(value.ToString()).GetDescription().ToString();
        }

        /// <summary>
        /// 将一个枚举常数的名称转换成等效的枚举对象
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(this string value)
        {
            if (string.IsNullOrEmpty(value)) throw new Exception("空字符不能转换成枚举");
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        /// <summary>
        /// 将指定的32位整数转换为枚举成员
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TEnum ToEnum<TEnum>(this object value)
        {
            if (!typeof(TEnum).IsEnum) throw new Exception("非枚举类型不能转换");
            return (TEnum)Enum.ToObject(typeof(TEnum), Convert.ToInt32(value));
        }
    }
}
