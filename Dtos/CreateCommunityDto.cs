using System.ComponentModel.DataAnnotations;

namespace Reddit.Dtos
{
    public class CreateCommunityDto
    {
        public int OwnerId { get; set; }
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
