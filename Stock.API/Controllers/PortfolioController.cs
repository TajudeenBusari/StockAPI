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
    
    public PortfolioController(UserManager<AppUser> userManager, 
        IStockRepository stockRepository, 
        IPortfolioRepository portfolioRepository)
    {
        _portfolioRepository = portfolioRepository;
        _userManager = userManager;
        _stockRepository = stockRepository;
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
}