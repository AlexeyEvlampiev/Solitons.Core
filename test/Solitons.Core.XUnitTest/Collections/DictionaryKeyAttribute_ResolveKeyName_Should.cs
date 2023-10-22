using Xunit;

namespace Solitons.Collections;

// ReSharper disable once InconsistentNaming
public sealed class DictionaryKeyAttribute_ResolveKeyName_Should
{
    [Fact]
    public void ReturnPropertyName_WhenAttributeIsNotApplied()
    {
        // Arrange
        var person = new Person { FirstName = "John", LastName = "Doe" };
        var propertyInfo = person.GetType().GetProperty(nameof(Person.LastName));

        // Act
        var keyName = DictionaryKeyAttribute.ResolveKeyName(propertyInfo);

        // Assert
        Assert.Equal(nameof(Person.LastName), keyName);
    }

    [Fact]
    public void ResolveKeyName_ShouldReturnCustomKeyName_WhenAttributeIsApplied()
    {
        // Arrange
        var person = new Person { FirstName = "John", LastName = "Doe" };
        var propertyInfo = person.GetType().GetProperty(nameof(Person.FirstName));

        // Act
        var keyName = DictionaryKeyAttribute.ResolveKeyName(propertyInfo);

        // Assert
        Assert.Equal("27b90765-7daf-4807-a562-030ff251e15f", keyName);
    }


    // Assuming the Person class is defined as below
    public class Person
    {
        [DictionaryKey("27b90765-7daf-4807-a562-030ff251e15f")]
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}