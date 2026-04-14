using AvalancheComments.Models;
using AvalancheComments.Services;

namespace AvalancheComments.Tests;

public class CommentFilterServiceTests
{
    private static List<CommentModel> CreateTestComments()
    {
        return new List<CommentModel>
        {
            new CommentModel { Name = "Alice", Text = "First comment", Timestamp = new DateTime(2024, 1, 10, 9, 0, 0) },
            new CommentModel { Name = "Bob", Text = "Second comment", Timestamp = new DateTime(2024, 1, 15, 12, 0, 0) },
            new CommentModel { Name = "Charlie", Text = "Third comment", Timestamp = new DateTime(2024, 1, 20, 18, 30, 0) },
            new CommentModel { Name = "Diana", Text = "Fourth comment", Timestamp = new DateTime(2024, 2, 1, 8, 0, 0) },
            new CommentModel { Name = "Eve", Text = "Fifth comment", Timestamp = new DateTime(2024, 2, 15, 14, 45, 0) },
        };
    }

    [Fact]
    public void FilterByDate_NoDates_ReturnsAllComments()
    {
        // Arrange
        var comments = CreateTestComments();

        // Act
        var result = CommentFilterService.FilterByDate(comments, null, null);

        // Assert
        Assert.Equal(5, result.Count);
    }

    [Fact]
    public void FilterByDate_WithFromDate_ReturnsCommentsOnOrAfterDate()
    {
        // Arrange
        var comments = CreateTestComments();
        var fromDate = new DateTime(2024, 1, 15);

        // Act
        var result = CommentFilterService.FilterByDate(comments, fromDate, null);

        // Assert
        Assert.Equal(4, result.Count);
        Assert.All(result, c => Assert.True(c.Timestamp >= fromDate.Date));
    }

    [Fact]
    public void FilterByDate_WithToDate_ReturnsCommentsOnOrBeforeDate()
    {
        // Arrange
        var comments = CreateTestComments();
        var toDate = new DateTime(2024, 1, 20);

        // Act
        var result = CommentFilterService.FilterByDate(comments, null, toDate);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.All(result, c => Assert.True(c.Timestamp <= toDate.Date.AddDays(1).AddTicks(-1)));
    }

    [Fact]
    public void FilterByDate_WithBothDates_ReturnsCommentsInRange()
    {
        // Arrange
        var comments = CreateTestComments();
        var fromDate = new DateTime(2024, 1, 15);
        var toDate = new DateTime(2024, 2, 1);

        // Act
        var result = CommentFilterService.FilterByDate(comments, fromDate, toDate);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(result, c => c.Name == "Bob");
        Assert.Contains(result, c => c.Name == "Charlie");
        Assert.Contains(result, c => c.Name == "Diana");
    }

    [Fact]
    public void FilterByDate_WithSameFromAndToDate_ReturnsCommentsOnThatDay()
    {
        // Arrange
        var comments = CreateTestComments();
        var date = new DateTime(2024, 1, 15);

        // Act
        var result = CommentFilterService.FilterByDate(comments, date, date);

        // Assert
        Assert.Single(result);
        Assert.Equal("Bob", result[0].Name);
    }

    [Fact]
    public void FilterByDate_NoMatchingComments_ReturnsEmptyList()
    {
        // Arrange
        var comments = CreateTestComments();
        var fromDate = new DateTime(2025, 1, 1);

        // Act
        var result = CommentFilterService.FilterByDate(comments, fromDate, null);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void FilterByDate_EmptyCommentsList_ReturnsEmptyList()
    {
        // Arrange
        var comments = new List<CommentModel>();

        // Act
        var result = CommentFilterService.FilterByDate(comments, new DateTime(2024, 1, 1), new DateTime(2024, 12, 31));

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void FilterByDate_ToDateIncludesEntireDay()
    {
        // Arrange - comment at end of day should be included
        var comments = new List<CommentModel>
        {
            new CommentModel { Name = "Late", Text = "Late night comment", Timestamp = new DateTime(2024, 1, 15, 23, 59, 59) }
        };
        var toDate = new DateTime(2024, 1, 15);

        // Act
        var result = CommentFilterService.FilterByDate(comments, null, toDate);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public void FilterByDate_FromDateIncludesStartOfDay()
    {
        // Arrange - comment at start of day should be included
        var comments = new List<CommentModel>
        {
            new CommentModel { Name = "Early", Text = "Early morning comment", Timestamp = new DateTime(2024, 1, 15, 0, 0, 0) }
        };
        var fromDate = new DateTime(2024, 1, 15);

        // Act
        var result = CommentFilterService.FilterByDate(comments, fromDate, null);

        // Assert
        Assert.Single(result);
    }
}
