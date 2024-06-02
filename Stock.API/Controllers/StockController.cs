using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.API.Data;
using Stock.API.Dtos;
using Stock.API.Interfaces;
using Stock.API.Mappers;

namespace Stock.API.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController: ControllerBase
{
    private readonly ApplicationDBContext _context;
    private readonly IStockRepository _stockRepository;
    public StockController(ApplicationDBContext context, IStockRepository stockRepository)
    {
        _context = context;
        _stockRepository = stockRepository;
    }
    //GET ALL
    [HttpGet]
    public async Task <IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var stocks = await _stockRepository.GetAllAsync();
        var stocksDto =  stocks.Select(s => s.ToStockDto()); //another wat to convert Domain to Dto
        return Ok(stocksDto);
    }
    
    //GET A SINGLE STOCK
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var stockDomain = await _stockRepository.GetByIdAsync(id);
        if (stockDomain == null)
        {
            return NotFound();
        }

        //convert to dto
        var foundStockDto = stockDomain.ToStockDto();

        return Ok(foundStockDto);
    }
    
    //POST A STOCK
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRequestStockDto createRequestStockDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var stockModel = createRequestStockDto.ToStockFromStockDto();
        await _stockRepository.CreateAsync(stockModel);
        return CreatedAtAction(nameof(GetById), new {Id = stockModel.Id}, stockModel.ToStockDto());
    }
    
    //Update A STOCK
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRequestStockDto updateRequestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var stockModel = await _stockRepository.UpdateAsync(id, updateRequestDto);
        //check if it exists
        if (stockModel == null)
        {
            return NotFound();
        }
        
        //convert updated stockModel to dto
        var updatedStockDto = stockModel.ToStockDto();
        
        return Ok(updatedStockDto);
    }
    
    //DELETE A STOCK
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        //first check if exist
        var existingStock = await _stockRepository.DeleteAsync(id);
        if (existingStock == null)
        {
            return NotFound();
        }
        //map domain to dto
        var existingStockDto = existingStock.ToStockDto();
        
        return Ok(existingStockDto);
    }
}

/*dd controller to program.cs
 *
 * *
 */