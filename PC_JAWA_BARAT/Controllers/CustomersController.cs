using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PC_JAWA_BARAT.DTOS;
using PC_JAWA_BARAT.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PC_JAWA_BARAT.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly EsemkaOnePlusContext _context;

        public CustomersController(EsemkaOnePlusContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<CustomerDTO>> Get([FromQuery] string? keyword)
        {
            if (Session.Role == 0)
            {
                IQueryable<Customer> query = _context.Customers;
                List<CustomerDTO> customers = new List<CustomerDTO>();
                if (keyword != null)
                {
                    keyword = keyword.ToLower(); ;
                    query = query.Where(i => i.Email.ToLower().Contains(keyword) || i.Name.ToLower().Contains(keyword) || i.PhoneNumber.Contains(keyword) || i.Address.ToLower().Contains(keyword) || i.Loyalty.Name.ToLower().Contains(keyword));
                }


                customers = query.OrderBy(i => i.Name).ThenBy(i => i.Email).Select(i => new CustomerDTO
                {
                    CustomerEmail = i.Email,
                    CustomerName = i.Name,
                    Address = i.Address,
                    DateOfBirth = i.DateOfBirth,
                    Gender = i.Gender,
                    PhoneNumber = i.PhoneNumber,
                    LoyaltyName = i.Loyalty.Name,
                    TotalPoint = i.TotalPoint
                }).ToList();
                return Ok(customers);
            }
            return StatusCode(403);
        }

        [HttpGet("{email}")]
        public ActionResult<CustomerDTO> GetByEmail([FromRoute] string email)
        {
            if (Session.Role == 0)
            {

                CustomerDTO customer = _context.Customers.Where(i => i.Email.ToLower() == email.ToLower()).Select(i => new CustomerDTO
                {
                    CustomerEmail = i.Email,
                    CustomerName = i.Name,
                    Address = i.Address,
                    DateOfBirth = i.DateOfBirth,
                    Gender = i.Gender,
                    PhoneNumber = i.PhoneNumber,
                    LoyaltyName = i.Loyalty.Name,
                    TotalPoint = i.TotalPoint
                }).FirstOrDefault();
                if (customer is null) return NotFound();
                return Ok(customer);
            }
            return StatusCode(403);
        }

        [HttpPut("{email}")]
        public ActionResult<CustomerDTO> Put([FromRoute] String email, [FromBody] CustomerPostDTO customer)
        {
            if (Session.Role == 0)
            {
                Customer getCustomer = _context.Customers.Where(i => i.Email.ToLower() == email.ToLower()).FirstOrDefault();
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
            return StatusCode(403);

        }

        [HttpPost]
        public ActionResult<CustomerPostDTO> Post([FromBody] CustomerPostDTO customer)
        {
            if (Session.Role == 0)
            {
                _context.Customers.Add(
                new Customer()
                {
                    Name = customer.Name,
                    Address = customer.Address,
                    CreatedAt = DateTime.Now,
                    DateOfBirth = customer.DateOfBirth,
                    Email = customer.Email,
                    Gender = customer.Gender,
                    LoyaltyId = customer.LoyaltyId,
                    Password = customer.Password,
                    PhoneNumber = customer.PhoneNumber,
                    PhotoPath = customer.PhotoPath,
                    Role = customer.Role,
                    TotalPoint = customer.TotalPoint,
                    LoyaltyExpiredDate = customer.LoyaltyExpiredDate
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
    }
}
