using System.ComponentModel.DataAnnotations;

namespace ICE3.Areas.ProjectManagement.Models;

public class Project
{
    /// <summary>
    /// The unique identifier for the project
    /// </summary>
    [Key]
    public int ProjectId { get; set; }
    
    /// <summary>
    /// Required field describing a project's name
    /// </summary>
    [Display(Name = "Project Name")]
    [Required]
    [StringLength(100, ErrorMessage = "Project name cannot be longer than 100 characters.")]
    public required string Name { get; set; }
    
    /// <summary>
    /// Optional project description
    /// </summary>
    [Display(Name = "Project Description")]
    [DataType(DataType.MultilineText)]
    [StringLength(500, ErrorMessage = "Project name cannot be longer than 500 characters.")]
    public string? Description { get; set; }

    /// <summary>
    /// A project's start date
    /// </summary>
    [Display(Name = "Project Start Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}")]
    public DateTime StartDate { get; set; } 

    /// <summary>
    /// A project's end date
    /// </summary>
    [Display(Name = "Project End Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}")]
    public DateTime EndDate { get; set; }
    
    /// <summary>
    /// A project's completion status
    /// </summary>
    [Display(Name = "Project Status")]
    public string? Status { get; set; }
    
    
    // Navigation Properties

    /// <summary>
    /// The collection of tasks that belong to this project
    /// </summary>
    public List<ProjectTask>? Tasks { get; set; } = new();

}