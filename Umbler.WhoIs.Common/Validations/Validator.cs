using FluentValidation;

namespace Umbler.WhoIs.Common.Validations;

/// <summary>
/// Utility that resolves and executes a FluentValidation validator for a given instance.
/// </summary>
public static class Validator
{
    /// <summary>
    /// Validates an instance by discovering its IValidator&lt;T&gt; at runtime.
    /// </summary>
    /// <typeparam name="T">Type to validate.</typeparam>
    /// <param name="instance">Object instance.</param>
    /// <returns>Enumerable of errors (empty if valid).</returns>
    /// <exception cref="InvalidOperationException">Thrown if no validator is found.</exception>
    public static async Task<IEnumerable<ValidationErrorDetail>> ValidateAsync<T>(T instance)
    {
        Type validatorType = typeof(IValidator<>).MakeGenericType(typeof(T));

        if (Activator.CreateInstance(validatorType) is not IValidator validator)
            throw new InvalidOperationException($"No validator found for: {typeof(T).Name}");

        var result = await validator.ValidateAsync(new ValidationContext<T>(instance));

        return !result.IsValid
            ? result.Errors.Select(o => (ValidationErrorDetail)o)
            : [];
    }
}