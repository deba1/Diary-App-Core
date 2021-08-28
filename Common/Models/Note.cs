using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class Note
    {
        public int Id { get; set; }
        [Required, MaxLength(255)]
        public string Title { get; set; }
        public string Body { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Date { get; set; }
        public bool Deleted { get; set; }
    }
}
