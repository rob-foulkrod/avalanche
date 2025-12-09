using System.ComponentModel.DataAnnotations;
using AvalancheComments.Models;

namespace AvalancheComments.Tests;

public class CommentModelTests
{
    [Fact]
    public void CommentModel_DefaultValues_ShouldBeEmpty()
    {
        // Arrange & Act
        var comment = new CommentModel();

        // Assert
        Assert.Equal(string.Empty, comment.Name);
        Assert.Equal(string.Empty, comment.Text);
        Assert.Equal(default(DateTime), comment.Timestamp);
    }

    [Fact]
    public void CommentModel_WithValidData_ShouldPassValidation()
    {
        // Arrange
        var comment = new CommentModel
        {
            Name = "John Doe",
            Text = "This is a valid comment.",
            Timestamp = DateTime.Now
        };

        // Act
        var validationResults = ValidateModel(comment);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void CommentModel_WithEmptyName_ShouldFailValidation()
    {
        // Arrange
        var comment = new CommentModel
        {
            Name = "",
            Text = "This is a valid comment.",
            Timestamp = DateTime.Now
        };

        // Act
        var validationResults = ValidateModel(comment);

        // Assert
        Assert.Single(validationResults);
        Assert.Contains(validationResults, v => v.ErrorMessage == "Name is required");
    }

    [Fact]
    public void CommentModel_WithEmptyText_ShouldFailValidation()
    {
        // Arrange
        var comment = new CommentModel
        {
            Name = "John Doe",
            Text = "",
            Timestamp = DateTime.Now
        };

        // Act
        var validationResults = ValidateModel(comment);

        // Assert
        Assert.Single(validationResults);
        Assert.Contains(validationResults, v => v.ErrorMessage == "Comment is required");
    }

    [Fact]
    public void CommentModel_WithNameTooLong_ShouldFailValidation()
    {
        // Arrange
        var comment = new CommentModel
        {
            Name = new string('A', 51), // 51 characters, max is 50
            Text = "This is a valid comment.",
            Timestamp = DateTime.Now
        };

        // Act
        var validationResults = ValidateModel(comment);

        // Assert
        Assert.Single(validationResults);
        Assert.Contains(validationResults, v => v.ErrorMessage == "Name must be less than 50 characters");
    }

    [Fact]
    public void CommentModel_WithTextTooLong_ShouldFailValidation()
    {
        // Arrange
        var comment = new CommentModel
        {
            Name = "John Doe",
            Text = new string('A', 501), // 501 characters, max is 500
            Timestamp = DateTime.Now
        };

        // Act
        var validationResults = ValidateModel(comment);

        // Assert
        Assert.Single(validationResults);
        Assert.Contains(validationResults, v => v.ErrorMessage == "Comment must be less than 500 characters");
    }

    [Fact]
    public void CommentModel_WithExactMaxLengthName_ShouldPassValidation()
    {
        // Arrange
        var comment = new CommentModel
        {
            Name = new string('A', 50), // Exactly 50 characters
            Text = "This is a valid comment.",
            Timestamp = DateTime.Now
        };

        // Act
        var validationResults = ValidateModel(comment);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void CommentModel_WithExactMaxLengthText_ShouldPassValidation()
    {
        // Arrange
        var comment = new CommentModel
        {
            Name = "John Doe",
            Text = new string('A', 500), // Exactly 500 characters
            Timestamp = DateTime.Now
        };

        // Act
        var validationResults = ValidateModel(comment);

        // Assert
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData("Alice", "Great website!")]
    [InlineData("Bob", "Love the avalanche theme")]
    [InlineData("Charlie", "Cool design")]
    public void CommentModel_WithVariousValidInputs_ShouldPassValidation(string name, string text)
    {
        // Arrange
        var comment = new CommentModel
        {
            Name = name,
            Text = text,
            Timestamp = DateTime.Now
        };

        // Act
        var validationResults = ValidateModel(comment);

        // Assert
        Assert.Empty(validationResults);
    }

    [Fact]
    public void CommentModel_Timestamp_ShouldBeSettable()
    {
        // Arrange
        var expectedTimestamp = new DateTime(2024, 1, 15, 10, 30, 0);
        var comment = new CommentModel
        {
            Name = "John Doe",
            Text = "Test comment",
            Timestamp = expectedTimestamp
        };

        // Assert
        Assert.Equal(expectedTimestamp, comment.Timestamp);
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model);
        Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);
        return validationResults;
    }
}
