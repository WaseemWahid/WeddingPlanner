using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeddingPlanner.Models
{
    public class Attendance
    {
        [Key]

        public int AttendanceId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public int UserId { get; set; }

        public int WeddingId { get; set; }

        public User User { get; set; }

        public Wedding Wedding { get; set; }
    }
}