namespace OngProject.Core.DTOs.UserDTOs
{
    public class UserLoginResponseDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public string Role { get; set; }
        public string CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}