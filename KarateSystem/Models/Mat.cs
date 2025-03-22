using KarateSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace KarateSystem.Models
{
    public class Mat
    {
        public int MatId { get; set; }
        public string MatName { get; set; }
        public ICollection<Kata> Katas { get; set; } = new List<Kata>();
        public ICollection<Fight> Fights { get; set; } = new List<Fight>();
    }
}
