using System.Security.Claims;
using GeneralWiki.Application;
using GeneralWiki.Models;
using GeneralWiki.Service.DtoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class EntryController(IEntryDataProvider entryDataProviderService) : ControllerBase
{
    // GET: 通过Id获取词条
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Entry>> GetEntryById(int id)
    {
        try
        {
            return Ok(await entryDataProviderService.GetEntryById(id));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    //GET: 通过Title获取词条
    [HttpGet("{title}")]
    public async Task<ActionResult<List<Entry>>> GetEntryByTitle(string title)
    {
        try
        {
            return Ok(await entryDataProviderService.GetEntryByTitle(title));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    //GET: 通过Tags获取词条
    [HttpGet]
    public async Task<ActionResult<ICollection<Entry>>> GetEntriesByTags([FromQuery]List<string>? tagNames)
    {
        try
        {
            return Ok(await entryDataProviderService.GetEntriesByTags(tagNames));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    //GET: 通过Category获取词条
    [HttpGet]
    public async Task<ActionResult<ICollection<Entry>>> GetEntriesByCategory(string categoryName)
    {
        try
        {
            return Ok(await entryDataProviderService.GetEntriesByCategory(categoryName));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    //GET: 展示所有词条
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Entry>>> GetEntries()
    {
        try
        {
            return Ok(await entryDataProviderService.GetEntries());
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    //POST: 新建/编辑词条
    [HttpPost]
    public async Task<ActionResult<Entry>> PostEntry([FromForm]EntryDto entryDto)
    {
        try
        {
            return Ok(await entryDataProviderService.PostEntry(entryDto));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
       
    }
    
    //DELETE: 删除词条
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteEntry(string title)
    {
        var staff = User.FindFirstValue(ClaimTypes.Role);
        if (staff is not "Admin") return Unauthorized("Only administrators have permission to delete entries");
        try
        {
            return Ok(await entryDataProviderService.DeleteEntry(title));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}