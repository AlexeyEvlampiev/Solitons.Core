using System;

namespace Solitons;

/// <summary>
/// Represents a static constant of a specific type.
/// </summary>
/// <typeparam name="T">The type of the constant value.</typeparam>
public abstract record StaticConstant<T> : IEquatable<T> 
    where T : IEquatable<T>
{
    private readonly T _value;

    /// <summary>
    /// Initializes a new instance of the StaticConstant class with the specified value.
    /// </summary>
    /// <param name="value">The value of the constant.</param>
    /// <exception cref="ArgumentNullException">Thrown when value is null.</exception>
    protected StaticConstant(T value)
    {
        _value = ThrowIf.ArgumentNull(value);
    }

    /// <summary>
    /// Determines whether the specified value is equal to the current value.
    /// </summary>
    /// <param name="value">The value to compare with the current value.</param>
    /// <returns>true if the specified value is equal to the current value; otherwise, false.</returns>
    public bool Equals(T? value) => _value.Equals(value);

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public sealed override string ToString() => _value.ToString() ?? typeof(T).ToString();

    /// <summary>
    /// Returns a string that represents the current object, formatted with the specified formatter.
    /// </summary>
    /// <param name="formatter">A function that accepts a value of type T and returns a string.</param>
    /// <returns>A string that represents the current object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when formatter is null.</exception>
    public string ToString(Func<T, string> formatter)
    {
        ThrowIf.ArgumentNull(formatter);
        return formatter.Invoke(_value);
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() => _value.GetHashCode();

    /// <summary>
    /// Defines an implicit conversion of a StaticConstant{T} to its underlying type.
    /// </summary>
    /// <param name="staticConstant">The StaticConstant{T} to convert.</param>
    /// <returns>The underlying type of the StaticConstant{T}.</returns>
    public static implicit operator T(StaticConstant<T> staticConstant) => staticConstant._value;

    /// <summary>
    /// Defines an implicit conversion of a StaticConstant{T} to a string.
    /// </summary>
    /// <param name="staticConstant">The StaticConstant{T} to convert.</param>
    /// <returns>A string representation of the StaticConstant{T}.</returns>
    public static implicit operator string(StaticConstant<T> staticConstant) => staticConstant.ToString();

    /// <summary>
    /// Determines whether a specified instance of <see cref="StaticConstant{T}"/> is equal to another specified T.
    /// </summary>
    /// <param name="a">The first instance of <see cref="StaticConstant{T}"/> to compare.</param>
    /// <param name="b">The second T to compare.</param>
    /// <returns>true if a and b are equal; otherwise, false.</returns>
    public static bool operator ==(StaticConstant<T> a, T b) => a?._value.Equals(b) ?? false;

    /// <summary>
    /// Determines whether a specified instance of <see cref="StaticConstant{T}"/> is not equal to another specified T.
    /// </summary>
    /// <param name="a">The first instance of <see cref="StaticConstant{T}"/> to compare.</param>
    /// <param name="b">The second T to compare.</param>
    /// <returns>true if a and b are not equal; otherwise, false.</returns>
    public static bool operator !=(StaticConstant<T> a, T b) => !a?._value.Equals(b) ?? true;
}