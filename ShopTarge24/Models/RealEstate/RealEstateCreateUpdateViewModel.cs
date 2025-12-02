using System.ComponentModel.DataAnnotations;
using ShopTarge24.Models.RealEstate;

namespace ShopTarge24.Models.RealEstate
{
    public class RealEstateCreateUpdateViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Value cannot be negative.")]
        public double? Area { get; set; }

        //[Required]
        public string? Location { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "Value cannot be negative.")]
        [Range(1, 1000)]
        public int RoomNumber { get; set; }
        public string? BuildingType { get; set; }

        public List<IFormFile>? Files { get; set; }
        public List<RealEstateImageViewModel> Image { get; set; }
            = new List<RealEstateImageViewModel>();

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}