using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.API.Data;
using Stock.API.Dtos;
using Stock.API.Mappers;

namespace Stock.API.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController: ControllerBase
{
    private readonly ApplicationDBContext _context;
    public StockController(ApplicationDBContext context)
    {
        _context = context;
    }
    //GET ALL
    [HttpGet]
    public async Task <IActionResult> GetAll()
    {
        var stocks = await _context.Stock.ToListAsync();
         var stocksDto =  stocks.Select(s => s.ToStockDto()); //another wat to convert Domain to Dto
        return Ok(stocksDto);
    }
    
    //GET A SINGLE STOCK
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stockDto = await _context.Stock.FindAsync(id);
        if (stockDto == null)
        {
            return NotFound();
        }

        return Ok(stockDto);
    }
    
    //POST A STOCK
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRequestStockDto createRequestStockDto)
    {
        var stockModel = createRequestStockDto.ToStockFromStockDto();
        await _context.Stock.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new {Id = stockModel.Id}, stockModel.ToStockDto());
    }
    
    //Update A STOCK
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRequestStockDto updateRequestDto)
    {
        //first check if it exists
        var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
        if (stockModel == null)
        {
            return NotFound();
        }

        stockModel.CompanyName = updateRequestDto.CompanyName;
        stockModel.Symbol = updateRequestDto.Symbol;
        stockModel.Purchase = updateRequestDto.Purchase;
        stockModel.LastDiv = updateRequestDto.LastDiv;
        stockModel.Industry = updateRequestDto.Industry;
        stockModel.MarketCap = updateRequestDto.MarketCap;
        
        await _context.SaveChangesAsync();
        return Ok(stockModel.ToStockDto());
    }
    
    //DELETE A STOCK
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        //first check if exist
        var existingStock = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
        if (existingStock == null)
        {
            return NotFound();
        }
        
        _context.Stock.Remove(existingStock); //NB: remove method is not asynchronous 
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

/*dd controller to program.cs
 *
 * *
 */