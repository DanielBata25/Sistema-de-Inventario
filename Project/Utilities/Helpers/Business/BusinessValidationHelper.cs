using Utilities.Exceptions;

namespace Utilities.Helpers.Business
{
    public static class BusinessValidationHelper
    {
        public static void ThrowIfNull(object? obj, string message)
        {
            if (obj == null)
            {
                throw new ValidationException(message);
            }
        }

        public static void ThrowIfTrue(bool condition, string message)
        {
            if (condition)
            {
                throw new BusinessRuleViolationException(message);
            }
        }

        public static void ThrowIfNullOrEmpty(string? value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ValidationException(message);
            }
        }

        public static void ThrowIfZeroOrLess(int number, string message)
        {
            if (number <= 0)
            {
                throw new ValidationException(message);
            }
        }

        public static void ThrowIfNegative(int number, string message)
        {
            if (number < 0)
            {
                throw new ValidationException(message);
            }
        }

        public static void ThrowIfNegative(decimal number, string message)
        {
            if (number < 0)
            {
                throw new ValidationException(message);
            }
        }
    }
}