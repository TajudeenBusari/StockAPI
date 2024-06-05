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
}