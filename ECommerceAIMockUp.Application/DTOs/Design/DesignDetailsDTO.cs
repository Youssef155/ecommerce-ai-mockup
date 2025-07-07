using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerceAIMockUp.Application.Wrappers;

namespace ECommerceAIMockUp.Application.DTOs.Design
{
    public class DesignDetailsDTO
    {
        [Required]  
        public int DesignId { get; set; }
        [Required]
        [Range(0.1, 1)]
        public float ScaleX { get; set; }
        [Required]
        [Range(0.1, 1)]
        public float ScaleY { get; set; }
        [Required]
        [Range(0, .5)]
        public float XAxis { get; set; }
        [Required]
        [Range(0, .5)]
        public float YAxis { get; set; }
        [Required]
        [Range(0, 1)]
        public float Opacity { get; set; }
        [Required]
        [Range(-180, 180)]
        public float Rotation { get; set; }
        [Required]
        [AllowedDesignPosition("front", "back", ErrorMessage = "Position must be either 'Front' or 'Back'.")]
        public string Position { get; set; }

    }
}
