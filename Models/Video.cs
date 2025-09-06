using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace efcoreApi.Models
{
    public class Video
    {
        [Key]
        public string VideoId { get; set; }
        public string VideoTitle { get; set; }
        public string VideoDescription { get; set; }
        [NotMapped]
        public IFormFile VideoFile { get; set; }
        public string? VideoUrl { get; set; } 
        [NotMapped]
        public string? VideoSrc { get; set; } 
        public string? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public Video()
        {
            VideoId = Guid.NewGuid().ToString();    
        }
    }
}
