using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using SimpleWebService.Models;

namespace SimpleWebService.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : Controller
    { 
        [HttpGet]
        public ActionResult GetAll()
        {
            CustomerContext context = HttpContext.RequestServices.GetService(typeof(CustomerContext)) as CustomerContext;
            return Ok(context.GetAllCustomers());
        }

        [HttpGet("{id:int}")]
        public ActionResult Get(int id)
        {
            CustomerContext context = HttpContext.RequestServices.GetService(typeof(CustomerContext)) as CustomerContext;
            Customer customer = context.GetCustomer(id);
            if (customer != null)
                return Ok(customer);
            else
                return NotFound("Customer with ID "+ id + " not found");
        }
       
        [HttpPost]
        public ActionResult Post([FromBody]Customer customer)
        {
            CustomerContext context = HttpContext.RequestServices.GetService(typeof(CustomerContext)) as CustomerContext;
            try
            {
                context.CreateCustomer(customer);
            }
            catch(Exception ex)
            {
                return Json(new { status = "error", message = ex.Message }); 
            }
            
            return Ok(customer);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, [FromBody]Customer customer)
        {
            CustomerContext context = HttpContext.RequestServices.GetService(typeof(CustomerContext)) as CustomerContext;
            Customer customerFromDB = context.GetCustomer(id);
            if(customer.name == null || customer.surname == null)
                return Json(new { status = "error", message = "Customer name/surname is missing" });
            
            if(customerFromDB == null)
                return NotFound("Customer with ID " + id + " not found");
            
            else
            {
                if(context.UpdateCustomer(id, customer))
                {
                    customer.id = id;
                    return Ok(customer);
                }
                else
                    return NotFound("Customer with ID " + id + " not found");
            }
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            CustomerContext context = HttpContext.RequestServices.GetService(typeof(CustomerContext)) as CustomerContext;
            if (!context.DeleteCustomer(id))
               return NotFound("Customer with ID " + id + " not found");
            
            return StatusCode(204);
        }
    }
}
