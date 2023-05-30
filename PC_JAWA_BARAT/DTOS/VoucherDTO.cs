using PC_JAWA_BARAT.Models;

namespace PC_JAWA_BARAT.DTOS
{
    public class VoucherDTO
    {

        public string VoucherCode { get; set; } = null!;
        public string VoucherName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Cost { get; set; }
        public int Limit { get; set; }
        public DateTime ActivatedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
        public bool IsActive { get; set; }

    }
}
