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
    public class MerchantsController : ControllerBase
    {
        private readonly EsemkaOnePlusContext _context;

        public MerchantsController(EsemkaOnePlusContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<MerchantDTO>> Get()
        {
            IQueryable<Merchant> query = _context.Merchants;
            List<MerchantDTO> result = new List<MerchantDTO>();
            return Ok(query.OrderBy(i => i.Location).ThenBy(i => i.Name).Select(i => new MerchantDTO() { Name = i.Name, Description = i.Description, Location = i.Location, Multiplier = i.Multiplier}).ToList());
        }

        [HttpPost]
        public ActionResult<MerchantDTO> Post([FromBody] MerchantDTO merchant)
        {
            if (Session.Role == 0)
            {
                Merchant asMerchant = new Merchant()
                {
                    Name = merchant.Name,
                    Description = merchant.Description,
                    CreatedAt = DateTime.Now,
                    Location = merchant.Location,
                    Multiplier = merchant.Multiplier,

                };
                _context.Merchants.Add(asMerchant);
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

        public ActionResult<MerchantDTO> Put([FromRoute] int id, [FromBody] MerchantDTO merchant)
        {

            if (Session.Role == 0)
            {
                Merchant getMerchant = _context.Merchants.Find(id);
                if (getMerchant is null) return NotFound();

                getMerchant.Location = merchant.Location;
                getMerchant.Multiplier = merchant.Multiplier;
                getMerchant.Description = merchant.Description;
                getMerchant.Name = merchant.Name;
                try
                {
                    _context.SaveChanges();
                    return Ok(merchant);
                }
                catch
                {
                    return BadRequest();
                }
            }
            return StatusCode(403);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            Console.WriteLine(Session.Role);
            Console.WriteLine(Session.Email);
            if (Session.Role == 0)
            {
                Merchant getMerchant = _context.Merchants.Find(id);
                if (getMerchant is null) return NotFound();
                _context.Merchants.Remove(getMerchant);
                try
                {
                    _context.SaveChanges();
                    return Ok();
                }
                catch
                {
                    return StatusCode(400);
                }
            }
            return StatusCode(403);
        }


    }
}
