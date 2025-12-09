using System.ComponentModel.DataAnnotations;

namespace AvalancheComments.Models;

public class CommentModel
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name must be less than 50 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Comment is required")]
    [StringLength(500, ErrorMessage = "Comment must be less than 500 characters")]
    public string Text { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }
}
