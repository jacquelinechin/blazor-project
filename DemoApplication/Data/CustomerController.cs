using DemoApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO;

namespace DemoApplication.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CustomerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("Invalid customer data.");
            }

            _dbContext.Customer.Add(customer);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.CustomerID }, customer);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _dbContext.Customer.ToListAsync();
            if (customers == null)
            {
                return NotFound($"Customers not found.");
            }

            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customer = await _dbContext.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _dbContext.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            _dbContext.Customer.Remove(customer);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
        {
            if (updatedCustomer == null || id != updatedCustomer.CustomerID)
            {
                return BadRequest("Invalid customer data.");
            }

            var customer = await _dbContext.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            customer.FirstName = updatedCustomer.FirstName;
            customer.LastName = updatedCustomer.LastName;
            customer.Gender = updatedCustomer.Gender;
            customer.Race = updatedCustomer.Race;
            customer.DoB = updatedCustomer.DoB;
            customer.Email = updatedCustomer.Email;
            customer.PhoneNumber = updatedCustomer.PhoneNumber;
            customer.Address = updatedCustomer.Address;
            customer.Postcode = updatedCustomer.Postcode;
            customer.City = updatedCustomer.City;
            customer.Country = updatedCustomer.Country;
            customer.State = updatedCustomer.State;

            _dbContext.Customer.Update(customer);
            await _dbContext.SaveChangesAsync();

            return Ok(customer);
        }

        [HttpGet("export-to-excel")]
        public async Task<IActionResult> ExportToExcel()
        {
            Console.WriteLine("export to excel api called.");
            var customers = await _dbContext.Customer.ToListAsync();
            if (customers == null)
            {
                return NotFound($"Customers not found.");
            }

            using ExcelEngine excelEngine = new();
            IApplication application = excelEngine.Excel;
            application.DefaultVersion = ExcelVersion.Excel2016;

            IWorkbook workbook = application.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            worksheet.Range["A1"].Text = "Customer ID";
            worksheet.Range["B1"].Text = "First Name";
            worksheet.Range["C1"].Text = "Last Name";
            worksheet.Range["D1"].Text = "Gender";
            worksheet.Range["E1"].Text = "Race";
            worksheet.Range["F1"].Text = "Date of Birth";
            worksheet.Range["G1"].Text = "E-mail";
            worksheet.Range["H1"].Text = "Phone Number";
            worksheet.Range["I1"].Text = "Address";
            worksheet.Range["J1"].Text = "Postcode";
            worksheet.Range["K1"].Text = "City";
            worksheet.Range["L1"].Text = "Country";
            worksheet.Range["M1"].Text = "State";

            var i = 2;
            foreach (var customer in customers)
            {
                worksheet.Range[$"A{i}"].Number = customer.CustomerID;
                worksheet.Range[$"B{i}"].Text = customer.FirstName;
                worksheet.Range[$"C{i}"].Text = customer.LastName;
                worksheet.Range[$"D{i}"].Text = customer.Gender;
                worksheet.Range[$"E{i}"].Text = customer.Race;
                worksheet.Range[$"F{i}"].DateTime = customer.DoB.ToDateTime(TimeOnly.MinValue);
                worksheet.Range[$"G{i}"].Text = customer.Email;
                worksheet.Range[$"H{i}"].Text = customer.PhoneNumber;
                worksheet.Range[$"I{i}"].Text = customer.Address;
                worksheet.Range[$"J{i}"].Text = customer.Postcode;
                worksheet.Range[$"K{i}"].Text = customer.City;
                worksheet.Range[$"L{i}"].Text = customer.Country;
                worksheet.Range[$"M{i}"].Text = customer.State;
                i++;
            }

            using MemoryStream stream = new();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Customers.xlsx"
            );
        }
    }
}
