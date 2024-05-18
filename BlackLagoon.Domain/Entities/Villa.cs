using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackLagoon.Domain.Entities
{
    public class Villa
    {
        public int Id { get; set; }
        [MaxLength(50,ErrorMessage = "Name cannot exceed 50 characters")]
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Range(1, 10000)]
        [DisplayName("Price per night")]
        public double Price { get; set; }
        [DisplayName("Size by sqare-feet")]
        public int Sqft { get; set; }
        [Range(1, 1000)]
        public int Occupancy { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
        [DisplayName("Image url")]
        public string? ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [ValidateNever]
        public IEnumerable<Amenity> VillaAmenity { get; set; }
    }
}
