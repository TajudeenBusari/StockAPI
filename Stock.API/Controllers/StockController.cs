using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.API.Data;
using Stock.API.Dtos;
using Stock.API.Interfaces;
using Stock.API.Mappers;
using Stock.API.Objects;

namespace Stock.API.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController: ControllerBase
{
    
    private readonly IStockRepository _stockRepository;
    public StockController(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }
    //GET ALL
    [HttpGet]
    [Authorize]
    public async Task <IActionResult> GetAll([FromQuery] QueryObject queryObject)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var stocks = await _stockRepository.GetAllAsync(queryObject);
        
        var stocksDto =  stocks
            .Select(s => StockMappers.ToStockDto(s)) //another wat to convert Domain to Dto
            .ToList(); 
        return Ok(stocksDto);
    }
    
    //GET A SINGLE STOCK
    [HttpGet]
    [Route("{id:int}")]
    [Authorize]
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
        var foundStockDto = StockMappers.ToStockDto(stockDomain);
        //stockDomain.ToStockDto()

        return Ok(foundStockDto);
    }
    
    //POST A STOCK
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateRequestStockDto createRequestStockDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        //stockModel.ToStockDto()
        //var stockModel = createRequestStockDto.ToStockFromStockDto();
        var stockModel = StockMappers.ToStockFromStockDto(createRequestStockDto);
        await _stockRepository.CreateAsync(stockModel);
        var stockDto = StockMappers.ToStockDto(stockModel);
        return CreatedAtAction(nameof(GetById), new {Id = stockModel.Id}, stockDto);
    }
    
    //Update A STOCK
    [Authorize]
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
        //stockModel.ToStockDto()
        var updatedStockDto = StockMappers.ToStockDto(stockModel);
        
        return Ok(updatedStockDto);
    }
    
    //DELETE A STOCK
    [Authorize]
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
        //existingStock.ToStockDto()
        var existingStockDto = StockMappers.ToStockDto(existingStock);
        
        return Ok(existingStockDto);
    }
}

/*dd controller to program.cs
 *
 * *
 */