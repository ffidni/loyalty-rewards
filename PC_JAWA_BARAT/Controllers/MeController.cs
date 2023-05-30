using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PC_JAWA_BARAT.DTOS;
using PC_JAWA_BARAT.Models;
using System.IdentityModel.Tokens.Jwt;

namespace PC_JAWA_BARAT.Controllers
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class MeController : ControllerBase
    {
        private readonly EsemkaOnePlusContext _context;

        public MeController(EsemkaOnePlusContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<Customer> Get()
        {
            Customer customer = _context.Customers.Where(i => i.Email == Session.Email).FirstOrDefault();
            if (customer is null) return NotFound();
            return Ok(customer);
        }

        [HttpPut]
        public ActionResult<CustomerDTO> Put([FromBody] CustomerPostDTO customer)
        {
            Customer getCustomer = _context.Customers.Where(i => i.Email == Session.Email).FirstOrDefault();
            if (getCustomer is null) return NotFound();
            getCustomer.Email = customer.Email;
            getCustomer.Address = customer.Address;
            getCustomer.PhoneNumber = customer.PhoneNumber;
            getCustomer.PhotoPath = customer.PhotoPath;
            getCustomer.DateOfBirth = customer.DateOfBirth;
            getCustomer.Gender = customer.Gender;
            getCustomer.LoyaltyId = customer.LoyaltyId;
            getCustomer.LoyaltyExpiredDate = customer.LoyaltyExpiredDate;
            getCustomer.Name = customer.Name;
            getCustomer.Password = customer.Password;
            getCustomer.Role = customer.Role;
            getCustomer.TotalPoint = customer.TotalPoint;

            try
            {
                _context.SaveChanges();
                return Ok(customer);
            }
            catch
            {
                return BadRequest();
            }

        }

    }
}
