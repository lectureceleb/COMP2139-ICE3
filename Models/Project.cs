﻿using System.ComponentModel.DataAnnotations;

namespace ICE3.Models;

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
    [Required]
    public required string Name { get; set; }
    
    /// <summary>
    /// Optional project description
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// A project's start date
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// A project's end date
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    
    /// <summary>
    /// A project's completion status
    /// </summary>
    public string? Status { get; set; }
    
}