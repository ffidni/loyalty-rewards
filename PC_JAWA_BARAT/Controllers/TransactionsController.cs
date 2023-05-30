using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PC_JAWA_BARAT.DTOS;
using PC_JAWA_BARAT.Models;
using System.Data.Entity;
using System.Text;

namespace PC_JAWA_BARAT.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly EsemkaOnePlusContext _context;

        public TransactionsController(EsemkaOnePlusContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<TransactionDTO>> Get([FromQuery] string? keyword)
        {
            IQueryable<Transaction> query = _context.Transactions;
            List<TransactionDTO> transactions = new List<TransactionDTO>();
            if (keyword != null)
            {
                keyword = keyword.ToLower(); ;
                query = query.Where(i => i.Customer.Name.ToLower().Contains(keyword) || i.Merchant.Name.ToLower().Contains(keyword) || i.TransactionDate.ToString().Contains(keyword));
            }


            transactions = query.OrderByDescending(i => i.TransactionDate).Select(i => new TransactionDTO
            {
                CustomerName = i.Customer.Name,
                MerchantName = i.Merchant.Name,
                Point = i.Point,
                Price = i.Price,
                TransactionDate = i.TransactionDate 
            }).ToList();
            return Ok(transactions);
        }

        [HttpPost]
        public ActionResult<TransactionDTO> Post([FromBody] TransactionPostDTO transaction)
        {
            List<Transaction> transactions = _context.Transactions.Where(i => i.TransactionDate == DateTime.Today && i.Merchant.Id == transaction.MerchantId).ToList();
            if (transactions.Count > 2)
            {
                return BadRequest();
            }
            Transaction newTransaction = new Transaction()
            {
                CreatedAt = DateTime.Now,
                CustomerId = transaction.CustomerId,
                MerchantId = transaction.MerchantId,
                Point = transaction.Price / 300,
                Price = transaction.Price,
                TransactionDate = DateTime.Today
            };
            _context.Transactions.Add(newTransaction);
            try
            {
                _context.SaveChanges();
                return StatusCode(201);
            }
            catch
            {
                return BadRequest();
            }



        }

        [HttpGet("{id}")]
        public ActionResult<TransactionDTO> Get([FromRoute] int id)
        {
            TransactionDTO transaction = _context.Transactions.Where(i => i.Id == id).Select(i => new TransactionDTO
            {
                CustomerName = i.Customer.Name,
                TransactionDate = i.TransactionDate,
                MerchantName = i.Merchant.Name,
                Point = i.Point,
                Price = i.Price,
            }).FirstOrDefault();
            if (transaction is null) return NotFound();
            return Ok(transaction);
        }

        [HttpGet("ExportCSV")]
        public IActionResult ExportCSV()
        {
            var transactions = _context.Transactions.Include("Customers").Include("Merchants").ToList();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("CustomerName,MerchantName,TransactionDate,Points");
            foreach (var transaction in transactions)
            {
                Customer customer = _context.Customers.Find(transaction.CustomerId);
                Merchant merchant = _context.Merchants.Find(transaction.MerchantId);
                builder.AppendLine($"{customer.Name},{merchant.Name},{transaction.TransactionDate.ToString()},{transaction.Point}");
            }
            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "Transactions.csv");
            
        }


    }
}
