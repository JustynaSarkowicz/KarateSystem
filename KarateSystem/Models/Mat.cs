using KarateSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace KarateSystem.Models
{
    public class Mat
    {
        [Key]
        public int MatId { get; set; }
        public string MatName { get; set; }
        public ICollection<Kata> MatKatas { get; set; } = new List<Kata>();
        public ICollection<Fight> MatFights { get; set; } = new List<Fight>();
    }
}
