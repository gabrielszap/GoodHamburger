namespace GoodHamburger.Domain.Common;

public static class Guard
{
    public static string AgainstNullOrWhiteSpace(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", paramName);
        }

        return value.Trim();
    }

    public static Guid AgainstEmptyId(Guid value, string paramName)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty.", paramName);
        }

        return value;
    }

    public static int AgainstNegative(int value, string paramName)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(paramName, "Value cannot be negative.");
        }

        return value;
    }

    public static void AgainstSameIds(Guid first, Guid second, string firstName, string secondName)
    {
        if (first == second)
        {
            throw new ArgumentException($"{firstName} and {secondName} must be different.");
        }
    }
}
