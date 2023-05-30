using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PC_JAWA_BARAT.DTOS;
using PC_JAWA_BARAT.Models;
using System.ComponentModel;

namespace PC_JAWA_BARAT.Controllers
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class TradesController : ControllerBase
    {
        private readonly EsemkaOnePlusContext _context;

        public TradesController(EsemkaOnePlusContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<TradeDTO>> Get()
        {
            IQueryable<Trade> query = _context.Trades;
            return Ok(query.OrderByDescending(i => i.CreatedAt).Select(i => new TradeDTO() { CustomerName = i.Customer.Name, VoucherCode = i.Voucher.Code, VoucherName = i.Voucher.Name, CreatedAt = i.CreatedAt}).ToList());
        }

        [HttpPost] ActionResult<TradeDTO> Post([FromBody] TradePostDTO trade)
        {
            _context.Trades.Add(new Trade
            {
                CustomerId = trade.CustomerId,
                VoucherId = trade.VoucherId,
            });
            try
            {
                _context.SaveChanges();
                return Ok(trade);
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpGet("{id}")]
        public ActionResult<TradeDTO> Get(int id)
        {
            TradeDTO trade = _context.Trades.Where(i => i.Id == id).Select(i => new TradeDTO
            {
                CreatedAt = i.CreatedAt,
                CustomerName = i.Customer.Name,
                VoucherCode = i.Voucher.Code,
                VoucherName = i.Voucher.Name,
            }).FirstOrDefault();
            if (trade is null) return NotFound();
            return Ok(trade);
        }

    }
}
