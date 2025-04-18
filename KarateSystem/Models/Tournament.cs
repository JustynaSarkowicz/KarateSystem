﻿using KarateSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarateSystem.Models
{
    public class Tournament
    {
        public int TourId { get; set; }
        public string TourName{ get; set; }
        public string TourPlace { get; set; }
        public DateTime TourDate { get; set; }
        public int Status { get; set; }
        public ICollection<TourCompetitor> TourCompetitors { get; set; } = new List<TourCompetitor>();
        public ICollection<TourCatKata> TourCatKatas { get; set; } = new List<TourCatKata>();
        public ICollection<TourCatKumite> TourCatKumites { get; set; } = new List<TourCatKumite>();
    }
}
