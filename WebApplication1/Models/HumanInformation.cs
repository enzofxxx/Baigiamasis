using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class HumanInformation
    {
        [Key]
        [ForeignKey("UserInformation")]
        public int HumanInformationId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [MaxLength(11)]
        [MinLength(11)]
        public int? PersonalId { get; set; }
        [MaxLength(10)]
        [MinLength(8)]
        public string? PhoneNumber { get; set; }
        public string? Mail { get; set; }
        public byte[] Avatar { get; set; }
        public UserInformation UserInformation { get; set; }
        public LocationInformation? LocationInformation { get; set; }
    }
}
