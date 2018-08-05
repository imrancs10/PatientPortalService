using System.ComponentModel.DataAnnotations;

namespace SWN.MobileService.Api.Data.Entities
{
    public class MobileRecipient
    {
        [Required]
        [Key]
        public long Id { get; set; }
        [Required]
        public long MobileUserId { get; set; }
    }
}