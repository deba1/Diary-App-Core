using Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Common.Models
{
    public class Setting
    {
        [Key]
        public string Id { get; set; }
        public int Status { get; set; }
    }
}
