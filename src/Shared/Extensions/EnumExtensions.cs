namespace Shared.Extensions;

public static class EnumExtensions
{
    public static string DisplayShortName<TEnum>(this TEnum enumeration) where TEnum : Enum
    {
        var enumType = typeof(TEnum);

        var name = Enum.GetName(enumType, enumeration);

        ArgumentNullException.ThrowIfNull(name);

        var displayName = enumType.GetMember(name).First().GetCustomAttribute<DisplayAttribute>()?.ShortName;
        return displayName;
    }

    public static string DisplayName<TEnum>(this TEnum enumeration) where TEnum : Enum
    {
        var enumType = typeof(TEnum);

        var name = Enum.GetName(enumType, enumeration);
        var displayName = enumType.GetMember(name).First().GetCustomAttribute<DisplayAttribute>()?.Name;
        return displayName ?? name;
    }

    public static string DisplayFullName<TEnum>(this TEnum enumeration) where TEnum : Enum
    {
        var enumType = typeof(TEnum);
        var name = Enum.GetName(enumType, enumeration);
        var displayAttribute = enumType.GetMember(name).First().GetCustomAttribute<DisplayAttribute>();

        return $"{displayAttribute?.Name}({displayAttribute?.ShortName})";
    }
}
