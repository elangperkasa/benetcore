namespace BENETCORE.Model
{
    public class User
    {
        public string? Id { get; set; } = null;
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; } = null;
        public required string Status { get; set; }
        public required DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; } = null;

    }

    public class CreateUserRequest
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public required string Password { get; set; }
        public required string Status { get; set; } = "Active";
    }

    public class UpdateUserRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
    }

}
