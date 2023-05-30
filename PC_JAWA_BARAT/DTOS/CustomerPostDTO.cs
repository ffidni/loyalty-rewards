namespace PC_JAWA_BARAT.DTOS
{
    public class CustomerPostDTO
    {
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int Role { get; set; }
        public int LoyaltyId { get; set; }
        public DateTime LoyaltyExpiredDate { get; set; }
        public string? PhotoPath { get; set; }
        public int TotalPoint { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
