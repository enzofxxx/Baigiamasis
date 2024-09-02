using Baigiamasis.Data;
using Baigiamasis.Dto;
using Baigiamasis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Baigiamasis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PersonController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContext;

        public PersonController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContext = httpContextAccessor;
        }

        [HttpPost("add")]
    public async Task<IActionResult> AddPersonInfo([FromForm] PersonDto personDto, [FromForm] string name)
    {
            var currentName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;
            if (name != currentName)
            {
                return BadRequest("User can modify only own information");
            }
            var user = await _context.Users.Include(u => u.Person).FirstOrDefaultAsync(u => u.Username == name);

            if (user == null)
                return NotFound("User not found");

            if (user.Person != null)
                return BadRequest("User already has person information");

            var person = new Person
            {
                FirstName = personDto.FirstName,
                LastName = personDto.LastName,
                PersonalCode = personDto.PersonalCode,
                PhoneNumber = personDto.PhoneNumber,
                Email = personDto.Email,
                Address = new Address
                {
                    City = personDto.Address.City,
                    Street = personDto.Address.Street,
                    HouseNumber = personDto.Address.HouseNumber,
                    ApartmentNumber = personDto.Address.ApartmentNumber
                },
                //UserId = userId // Pridėkite vartotojo ID
            };

            // Tik jei ProfilePictureBase64 yra nustatytas, pridėkite paveikslėlį
            if (!string.IsNullOrEmpty(personDto.ProfilePictureBase64))
            {
                person.ProfilePicture = Convert.FromBase64String(personDto.ProfilePictureBase64);
            }

            _context.Persons.Add(person);
            user.Person = person;

            await _context.SaveChangesAsync();

            return Ok(person);
        }

        [HttpGet("get-user")]
        public async Task<IActionResult> GetPersonInfo()
        {
            var currentName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;
            /*if (name != currentName)
            {
                return BadRequest("User can see only own information");
            }*/
            var user = await _context.Users.Include(u => u.Person).ThenInclude(p => p.Address)
                                           .FirstOrDefaultAsync(u => u.Username == currentName);

            if (user == null || user.Person == null)
                return NotFound("Person information not found");

            var personDto = new PersonDto
            {
                FirstName = user.Person.FirstName,
                LastName = user.Person.LastName,
                PersonalCode = user.Person.PersonalCode,
                PhoneNumber = user.Person.PhoneNumber,
                Email = user.Person.Email,
                ProfilePictureBase64 = null,
                Address = new AddressDto
                {
                    City = user.Person.Address.City,
                    Street = user.Person.Address.Street,
                    HouseNumber = user.Person.Address.HouseNumber,
                    ApartmentNumber = user.Person.Address.ApartmentNumber
                }
            };

            return Ok(personDto);
        }

        [HttpPut("update-firstname")]
        public async Task<IActionResult> UpdateFirstName([FromBody] UpdateFirstNameDto dto)
        {
            var currentName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == currentName);
            var person = await _context.Persons.SingleOrDefaultAsync(p => p.UserId == user.Id);

            if (person == null)
                return NotFound();

            person.FirstName = dto.FirstName;
            await _context.SaveChangesAsync();

            return Ok("First name updated succesfully.");
        }

        [HttpPut("update-lastname")]
        public async Task<IActionResult> UpdateLastName([FromBody] UpdateLastNameDto dto)
        {
            var currentName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == currentName);
            var person = await _context.Persons.SingleOrDefaultAsync(p => p.UserId == user.Id);

            if (user == null || user.Person == null)
                return NotFound("Person information not found");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                return BadRequest("Last name cannot be empty");

            person.LastName = dto.LastName;
            await _context.SaveChangesAsync();

            return Ok("Last name updated succesfully.");
        }

        [HttpPut("update-personalcode")]
        public async Task<IActionResult> UpdatePersonalCode([FromBody] UpdatePersonalCodeDto dto)
        {
            var currentName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == currentName);
            var person = await _context.Persons.SingleOrDefaultAsync(p => p.UserId == user.Id);

            if (user == null || user.Person == null)
                return NotFound("Person information not found");

            if (string.IsNullOrWhiteSpace(dto.PersonalCode))
                return BadRequest("Personal code cannot be empty");

            person.PersonalCode = dto.PersonalCode;
            await _context.SaveChangesAsync();

            return Ok("Personal code updated succesfully.");
        }

        [HttpPut("update-phonenumber")]
        public async Task<IActionResult> UpdatePhoneNumber([FromBody] UpdatePhoneNumberDto dto)
        {
            var currentName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == currentName);
            var person = await _context.Persons.SingleOrDefaultAsync(p => p.UserId == user.Id);

            if (user == null || user.Person == null)
                return NotFound("Person information not found");

            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                return BadRequest("Phone number cannot be empty");

            person.PhoneNumber = dto.PhoneNumber;
            await _context.SaveChangesAsync();

            return Ok("Phone number updated succesfully.");
        }

        [HttpPut("update-email")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailDto email)
        {
            if (string.IsNullOrWhiteSpace(email.Email))
                return BadRequest("City cannot be empty or whitespace.");

            var currentName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == currentName);
            var person = await _context.Persons.SingleOrDefaultAsync(p => p.UserId == user.Id);

            if (user == null || user.Person == null)
                return NotFound("Person information not found");

            person.Email = email.Email;
            await _context.SaveChangesAsync();

            return Ok("Email updated succesfully.");
        }

        [HttpPut("update-city")]
        public async Task<IActionResult> UpdateCity([FromBody] UpdateCityDto city)
        {
            if (string.IsNullOrWhiteSpace(city.City))
                return BadRequest("City cannot be empty or whitespace.");

            var currentName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == currentName);
            var person = await _context.Persons.SingleOrDefaultAsync(p => p.UserId == user.Id);

            if (person == null)
                return NotFound("Person not found or does not belong to the user.");

            var address = await _context.Addresses.SingleOrDefaultAsync(a => a.PersonId == person.Id);

            if (address == null)
                return NotFound("Address not found or does not belong to the user.");

            address.City = city.City;
            await _context.SaveChangesAsync();

            return Ok("City updated successfully.");
        }

        [HttpPut("update-street")]
        public async Task<IActionResult> UpdateStreet([FromBody] UpdateStreetDto street)
        {
            if (string.IsNullOrWhiteSpace(street.Street))
                return BadRequest("Street cannot be empty or whitespace.");

            var currentName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == currentName);
            var person = await _context.Persons.SingleOrDefaultAsync(p => p.UserId == user.Id);

            if (person == null)
                return NotFound("Person not found or does not belong to the user.");

            var address = await _context.Addresses.SingleOrDefaultAsync(a => a.PersonId == person.Id);

            if (address == null)
                return NotFound("Address not found or does not belong to the user.");

            address.Street = street.Street;
            await _context.SaveChangesAsync();

            return Ok("Street updated successfully.");
        }
        [HttpPut("update-housenumber")]
        public async Task<IActionResult> UpdateHouseNumber([FromBody] UpdateHouseNumberDto houseNumber)
        {
            if (string.IsNullOrWhiteSpace(houseNumber.HouseNumber))
                return BadRequest("House number cannot be empty or whitespace.");

            var currentName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == currentName);
            var person = await _context.Persons.SingleOrDefaultAsync(p => p.UserId == user.Id);

            if (person == null)
                return NotFound("Person not found or does not belong to the user.");

            var address = await _context.Addresses.SingleOrDefaultAsync(a => a.PersonId == person.Id);

            if (address == null)
                return NotFound("Address not found or does not belong to the user.");

            address.HouseNumber = houseNumber.HouseNumber;
            await _context.SaveChangesAsync();

            return Ok("House number updated successfully.");
        }

        [HttpPut("update-apartmentnumber")]
        public async Task<IActionResult> UpdateApartmentNumber([FromBody] UpdateApartmentNumberDto apartmentNumber)
        {
            if (string.IsNullOrWhiteSpace(apartmentNumber.ApartmentNumber))
                return BadRequest("Apartment number cannot be empty or whitespace.");

            var currentName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault().Value;

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == currentName);
            var person = await _context.Persons.SingleOrDefaultAsync(p => p.UserId == user.Id);

            if (person == null)
                return NotFound("Person not found or does not belong to the user.");

            var address = await _context.Addresses.SingleOrDefaultAsync(a => a.PersonId == person.Id);

            if (address == null)
                return NotFound("Address not found or does not belong to the user.");

            address.ApartmentNumber = apartmentNumber.ApartmentNumber;
            await _context.SaveChangesAsync();

            return Ok("Apartment number updated successfully.");
        }
    }
}
