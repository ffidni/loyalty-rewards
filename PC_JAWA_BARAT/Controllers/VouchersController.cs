using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PC_JAWA_BARAT.DTOS;
using PC_JAWA_BARAT.Models;

namespace PC_JAWA_BARAT.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase
    {
        private readonly EsemkaOnePlusContext _context;

        public VouchersController(EsemkaOnePlusContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<VoucherDTO>> Get()
        {
            if (Session.Role == 0)
            {
                List<VoucherDTO> vouchers = _context.Vouchers.OrderBy(i => i.Code).Select(i => new VoucherDTO
                {
                    ActivatedAt = i.ActivatedAt,
                    Cost = i.Cost,
                    Description = i.Description,
                    ExpiredAt = i.ExpiredAt,
                    Limit = i.Limit,
                    VoucherCode = i.Code,
                    VoucherName = i.Name,
                    IsActive = DateTime.Now < i.ExpiredAt
                }).ToList();
                return Ok(vouchers);
            }
            return StatusCode(403);
        }

        [HttpPost]
        public ActionResult<VoucherPostDTO> Post([FromBody] VoucherPostDTO voucher)
        {
            if (Session.Role == 0)
            {
                _context.Vouchers.Add(
                new Voucher()
                {
                    ActivatedAt = DateTime.Now,
                    Code = voucher.Code,
                    Cost = voucher.Cost,
                    CreatedAt = DateTime.Now,
                    Description = voucher.Description,
                    ExpiredAt = voucher.ExpiredAt,
                    Limit = voucher.Limit,
                    Name = voucher.Name,
                }
                );
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
            return StatusCode(403);
        }


        [HttpPut("{id}")]
        public ActionResult<VoucherPostDTO> Put([FromRoute] int id, [FromBody] VoucherPostDTO voucher)
        {
            if (Session.Role == 0)
            {
                Voucher getVoucher = _context.Vouchers.Find(id);
                if (getVoucher is null) return NotFound();

                getVoucher.ExpiredAt = voucher.ExpiredAt;
                getVoucher.Description = voucher.Description;
                getVoucher.Cost = voucher.Cost;
                getVoucher.ActivatedAt = voucher.ActivatedAt;
                getVoucher.Code = voucher.Code;
                getVoucher.Limit = voucher.Limit;
                getVoucher.Name = voucher.Name;

                try
                {
                    _context.SaveChanges();
                    return Ok(voucher);
                }
                catch
                {
                    return BadRequest();
                }
            }
            return StatusCode(403);
        }
    }
}
