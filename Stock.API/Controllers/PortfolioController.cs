using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Extensions;
using Stock.API.Interfaces;
using Stock.API.Models;

namespace Stock.API.Controllers;

[Route("api/portfolio")]
[ApiController]
public class PortfolioController: ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IStockRepository _stockRepository;
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly IFMPService _fmpService;
    
    public PortfolioController(UserManager<AppUser> userManager, 
        IStockRepository stockRepository, 
        IPortfolioRepository portfolioRepository,
        IFMPService fmpService)
    {
        _portfolioRepository = portfolioRepository;
        _userManager = userManager;
        _stockRepository = stockRepository;
        _fmpService = fmpService;

    }
    
    //GET USER PORTFOLIO
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolio()
    {
        //User is inherited from controller base
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username); //you can find by email async as well
        var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
        return Ok(userPortfolio);
    }
    
    //CREATE USER PORTFOLIO
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var stock = await _stockRepository.GetBySymbolAsync(symbol);
        
        if (stock == null)
        {
            stock = await _fmpService.FindStockBySymbolAsync(symbol);
            if (stock == null)
            {
                return BadRequest("Stock does not exist");
            }
            else
            {
                await _stockRepository.CreateAsync(stock);
            }
        }
        
        
        //do quick check if stock exist
        //user check is already being performed by authorization
        if (stock == null) 
            return BadRequest("Stock not found");
        
        var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
        //check if symbol we provide already exists in portfolio list
        if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower()))
            return BadRequest("Cannot add same stock to portfolio");
        
        //create a new portfolio object
        var portfolioModel = new Portfolio
        {
            StockId = stock.Id,
            AppUserId = appUser.Id
        };
        await _portfolioRepository.CreateUserPortfolioAsync(portfolioModel);
        if (portfolioModel == null)
        {
            return StatusCode(500, "Could not create");
        }
        else
        {
           return Created();
        }
        
    }
    
    //DELETE USER PORTFOLIO
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
        
        //compare the user portfolio with the symbol provided
        var filteredStocks = userPortfolio
            .Where(s => s.Symbol.ToLower() == symbol.ToLower())
            .ToList();

        if (filteredStocks.Count() == 1)
        {
            await _portfolioRepository.DeleteUserPortfolioAsync(appUser, symbol);
        }
        else
        {
            return BadRequest("Stock not in your portfolio");
        }

        return Ok("Portfolio successfully deleted");
    }
}