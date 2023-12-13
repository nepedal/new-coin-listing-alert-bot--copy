namespace Shared;

public static class EnumHelpers
{
    public static T CombineEnumValues<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        uint combined = 0;

        foreach (var value in values)
        {
            if (!value.Equals(Enum.GetValues(typeof(T)).GetValue(0)))
            {
                var intValue = (uint)value;
                combined |= intValue;
            }
        }

        return (T)Enum.ToObject(typeof(T), combined);
    }

    public static bool IsValuePresent<T>(this Enum flags, T value) where T : Enum
    {
        var flagsValue = (uint)(object)flags;
        var valueToCheck = (uint)(object)value;

        return (flagsValue & valueToCheck) != 0;
    }
}
