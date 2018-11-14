using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication5.Models
{
    public class McqViewModel
    {
        public int? Id { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int ChapterId { get; set; }
        [Required]
        public int ExamId { get; set; }
        [Required]
        public string Question { get; set; }
        [Required]
        public string OptionA { get; set; }
        [Required]
        public string OptionB { get; set; }
        [Required]
        public string OptionC { get; set; }
        [Required]
        public string OptionD { get; set; }
        [Required]
        public string CorrectOption { get; set; }
        [Required]
        public string Status { get; set; }
        public DateTime EntryDate { get; set; }
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public string Exam { get; set; }
        public string Subject { get; set; }
        public string Chapter { get; set; }


    }
}