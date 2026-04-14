using AvalancheComments.Models;

namespace AvalancheComments.Services;

public static class CommentFilterService
{
    /// <summary>
    /// Filters a list of comments by an optional date range based on their Timestamp.
    /// </summary>
    /// <param name="comments">The comments to filter.</param>
    /// <param name="fromDate">If specified, only comments on or after this date are included.</param>
    /// <param name="toDate">If specified, only comments on or before the end of this date are included.</param>
    /// <returns>The filtered list of comments.</returns>
    public static List<CommentModel> FilterByDate(
        IEnumerable<CommentModel> comments,
        DateTime? fromDate,
        DateTime? toDate)
    {
        var filtered = comments.AsEnumerable();

        if (fromDate.HasValue)
        {
            var from = fromDate.Value.Date;
            filtered = filtered.Where(c => c.Timestamp >= from);
        }

        if (toDate.HasValue)
        {
            var to = toDate.Value.Date.AddDays(1);
            filtered = filtered.Where(c => c.Timestamp < to);
        }

        return filtered.ToList();
    }
}
