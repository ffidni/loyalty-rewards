using PC_JAWA_BARAT.Models;

namespace PC_JAWA_BARAT.DTOS
{
    public class TradeDTO
    {
        public string CustomerName { get; set; }
        public string VoucherName { get; set; }
        public string VoucherCode { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
