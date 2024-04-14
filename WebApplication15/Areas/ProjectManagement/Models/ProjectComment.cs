﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication15.Areas.ProjectManagement.Models
{
    public class ProjectComment
    {
        public int ProjectCommentId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        public string? Content { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate{ get; set; }

        // Foreign key for Project
        public int ProjectId { get; set; }

        // Navigation property to Project
        public Project? Project { get; set; }
    }
}
