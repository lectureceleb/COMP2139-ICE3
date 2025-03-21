using System.ComponentModel.DataAnnotations;

namespace ICE3.Areas.ProjectManagement.Models;

public class ProjectComment
{
    [Key]
    public int CommentId { get; set; }
    
    [Display(Name = "Project Message")]
    [StringLength(100, ErrorMessage = "Project name cannot be longer than 100 characters.")]
    public required string Content { get; set; }
    
    [Display(Name = "Date Posted")]
    [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
    [DataType(DataType.DateTime)]
    private DateTime _datePosted;
    public DateTime DatePosted
    {
        get => _datePosted;
        set => _datePosted = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
    
    // Foreign Key
    public int ProjectId { get; set; }
    
    // Navigation property
    public Project? Project { get; set; }
}
