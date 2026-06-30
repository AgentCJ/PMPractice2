namespace MasterFloorAPI.Models
{
    public class PartnerCreateUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public int TypeId { get; set; }
        public string? DirectorLastName { get; set; }
        public string? DirectorFirstName { get; set; }
        public string? DirectorMiddleName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? Rating { get; set; }
        public Address? LegalAddress { get; set; }
    }
}
