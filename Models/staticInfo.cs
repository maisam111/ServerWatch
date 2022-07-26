using System.ComponentModel.DataAnnotations;

namespace ServerWatch.Models
{
    public class staticInfo
    {
        [Key]
        public int Id { get; set; }
        public string info { get; set; }
    }
}
