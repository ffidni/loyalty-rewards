using PC_JAWA_BARAT.Models;

namespace PC_JAWA_BARAT.DTOS
{
    public class TransactionPostDTO
    {
        public int CustomerId { get; set; }
        public int MerchantId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Price { get; set; }
        public decimal Point { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
