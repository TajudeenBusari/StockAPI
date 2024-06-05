using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Data;
using Stock.API.Dtos;
using Stock.API.Extensions;
using Stock.API.Interfaces;
using Stock.API.Mappers;
using Stock.API.Models;

namespace Stock.API.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController: ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;
    private readonly UserManager<AppUser> _userManager;
    public CommentController(ICommentRepository commentRepository, 
        IStockRepository stockRepository,
        UserManager<AppUser> userManager)
    {
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
        _userManager = userManager;
    }
    
    //GET ALL COMMENTS
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var comments = await _commentRepository.GetAllAsync();
        //we will use select, a javascript map for mapping here
        var commentsDto = comments
            .Select(s => s.MapFromCommentToCommentDto());
        return Ok(commentsDto);
    }
    
    //GET A SINGLE COMMENT
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var commentModel = await _commentRepository.GetByIdAsync(id);
        if (commentModel == null)
        {
            return NotFound();
        }
        //convert to Dto
        var foundCommentDto = commentModel.MapFromCommentToCommentDto();
        
        return Ok(foundCommentDto);
    }
    
    //CREATE A COMMENT
    [HttpPost]
    [Route("{stockId:int}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, 
        CreateCommentDto createCommentDto )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        //first check if stockId exists, for this we will create a method inside the stock repo
        if (!await _stockRepository.StockExists(stockId))
        {
            return BadRequest("Stock does not exist!");
        }
        
        //logic for adding user to comments
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);

        var createCommentModel = createCommentDto.MapFromCreateCommentDtoToComment(stockId);
        createCommentModel.AppUserId = appUser.Id;
        
        await _commentRepository.CreateAsync(createCommentModel);
        
        return CreatedAtAction(nameof(GetById), 
            new { id = createCommentModel.Id},
            createCommentModel.MapFromCommentToCommentDto());
    }
    
    //UPDATE A COMMENT
    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, 
        [FromBody] UpdateRequestCommentDto updateRequestCommentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var commentModel= updateRequestCommentDto.MapFromUpdateCommentDtoToComment();
        commentModel = await _commentRepository.UpdateAsync(id, commentModel);
        
        //check if commentModel exists
        if (commentModel == null)
        {
            return NotFound("Comment Not Found, Kindly provide the right Comment Id");
        }
        //else, convert commentModel back to Dto
        var updatedCommentDto = commentModel.MapFromCommentToCommentDto();
        return Ok(updatedCommentDto);
    }
    
    //DELETE A COMMENT
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var commentModel = await _commentRepository.DeleteAsync(id);
        if (commentModel == null)
        {
            return NotFound("Comment does not exist");
        }
        //map commentModel to dto
        var commentDto = commentModel.MapFromCommentToCommentDto();
        return Ok(commentDto);
    }
    
}

