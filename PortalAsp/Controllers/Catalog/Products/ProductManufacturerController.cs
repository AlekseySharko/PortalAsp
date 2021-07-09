using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalModels.Catalog.Products;
using PortalModels.Catalog.Repositories.Products;
using PortalModels.Validators;
using PortalModels.Validators.Catalog;

namespace PortalAsp.Controllers.Catalog.Products
{
    [Route("api/catalog/product-manufacturers")]
    public class ProductManufacturerController : Controller
    {
        private IManufacturerRepository ManufacturerRepository { get; }
        public ProductManufacturerController(IManufacturerRepository manufacturerRepository) => ManufacturerRepository = manufacturerRepository;

        [HttpGet]
        public IActionResult GetManufacturers()
        {
            return Ok(ManufacturerRepository.GetAllManufacturers().OrderBy(m => m.Name));
        }

        [HttpPost]
        public async Task<IActionResult> PostManufacturer([FromBody] Manufacturer manufacturer)
        {
            ValidationResult validationResult =
                ManufacturerValidator.ValidateOnAdd(manufacturer, ManufacturerRepository.GetAllManufacturers());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await ManufacturerRepository.AddManufacturerAsync(manufacturer);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutManufacturer([FromBody] Manufacturer manufacturer)
        {
            ValidationResult validationResult =
                ManufacturerValidator.ValidateOnEdit(manufacturer, ManufacturerRepository.GetAllManufacturers());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await ManufacturerRepository.UpdateManufacturerAsync(manufacturer);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManufacturer([FromRoute]long id)
        {
            ValidationResult validationResult =
                ManufacturerValidator.ValidateOnDelete(id, ManufacturerRepository.GetAllManufacturers());
            if (validationResult.IsValid == false)
                return BadRequest(validationResult.Message);

            await ManufacturerRepository.DeleteManufacturerAsync(new Manufacturer {ManufacturerId = id});
            return Ok();
        }
    }
}
