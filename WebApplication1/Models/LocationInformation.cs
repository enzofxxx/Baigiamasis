using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class LocationInformation
    {
        [Key]
        [ForeignKey("HumanInformation")]
        public int LocationInformationID { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? HouseNr { get; set; }
        public string? ApartmentNr { get; set; }
        public HumanInformation HumanInformation { get; set; }
    }
}
