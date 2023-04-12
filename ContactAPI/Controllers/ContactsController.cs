using ContactAPI.Data;
using ContactAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactAPI.Controllers
{
    [ApiController]
    // takes the Contacts name given below and injects it into the route
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactAPIDbContext dbContext;

        public ContactsController(ContactAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetContacts()
        {
            return Ok(dbContext.Contacts.ToList());
        }
        [HttpPost]
        public IActionResult AddContact(AddContactRequest addContactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address,
                Email = addContactRequest.Email,
                FullName = addContactRequest.FullName,
                Phone = addContactRequest.Phone

            };
            dbContext.Contacts.Add(contact);
            dbContext.SaveChanges();
            return Ok(contact);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = dbContext.Contacts.Find(id);
            if(contact != null)
            {
                contact.FullName = updateContactRequest.FullName;
                contact.Phone = updateContactRequest.Phone;
                contact.Email = updateContactRequest.Email;
                contact.Address = updateContactRequest.Address;
                dbContext.SaveChanges();
                return Ok(contact);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteContact([FromRoute] Guid id)
        {
            var contact = dbContext.Contacts.Find(id);
            if(contact != null) 
            {
                dbContext.Contacts.Remove(contact);
                dbContext.SaveChanges();
                return Ok(contact);
            }
            return NotFound();
        }
    }
}
