using System.ComponentModel.DataAnnotations.Schema;

namespace Stock.API.Models;

[Table("Comments")]
public class Comment
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    //navigation property
    public int? StockId { get; set; }
    public Stock? Stock { get; set; }
    
    //one to one
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}