using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mobileStore.Application.Catalog.DiscountCodes;
using mobileStore.ViewModels.Catalog.Products;
using mobileStore.ViewModels.Sale;

namespace mobileStore.BackendApi.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountCodesController : ControllerBase
    {
        private readonly IDiscountCodeService _discountService;

        public DiscountCodesController(
            IDiscountCodeService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var discount = await _discountService.GetAll();
            return Ok(discount);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageProductPagingRequest request)
        {
            var discount = await _discountService.GetAllPaging(request);
            return Ok(discount);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var discount = await _discountService.GetById(id);
            return Ok(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DiscountCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var discountId = await _discountService.Create(request);

            if (discountId == 0)
                return BadRequest();

            var discount = await _discountService.GetById(discountId);

            return CreatedAtAction(nameof(GetById), new { id = discountId }, discount);
        }

        // HttpPut: update toàn phần
        [HttpPut("updateDiscount")]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] DiscountUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _discountService.Update(request);
            if (affectedResult == 0)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var affectedResult = await _discountService.Delete(id);
            if (affectedResult == 0)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
