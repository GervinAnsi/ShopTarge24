using ShopTarge24.Models.Spaceships;

namespace ShopTarge24.Models.Kindergarten
{
    public class KindergartenDeleteViewModel
    {
        public Guid? Id { get; set; }
        public string? KindergartenName { get; set; }
        public string? GroupName { get; set; }
        public string? TeacherName { get; set; }
        public int? ChildrenCount { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Lisame listi failide jaoks
        public List<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();
    }
}
