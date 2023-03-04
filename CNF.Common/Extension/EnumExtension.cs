using System.ComponentModel;

namespace CNF.Common.Extension;

public static class EnumExtension
{
    /// <summary>
    /// 获得枚举字段的名称
    /// </summary>
    /// <returns></returns>
    public static string? GetName(this Enum thisValue)
    {
        return Enum.GetName(thisValue.GetType(), thisValue);
    }

    /// <summary>
    /// 获得枚举字段的值
    /// </summary>
    /// <returns></returns>
    public static T GetValue<T>(this Enum thisValue)
    {
        return (T)Enum.Parse(thisValue.GetType(), thisValue.ToString());
    }

    /// <summary>
    /// 获取枚举描述
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string GetEnumDescription(this Enum value)
    {
        if (value == null)
        {
            throw new ArgumentException("value");
        }

        var text = value.ToString();
        var fi = value.GetType().GetField(text);
        if (fi != null)
        {
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            text = attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
        else
        {
            text = string.Empty;
        }

        return text;
    }
}