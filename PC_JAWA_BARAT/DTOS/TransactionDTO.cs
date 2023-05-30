using PC_JAWA_BARAT.Models;

namespace PC_JAWA_BARAT.DTOS
{
    public class TransactionDTO
    {
        public string CustomerName { get; set; }
        public string MerchantName { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Price { get; set; }
        public decimal Point { get; set; }

    }
}
