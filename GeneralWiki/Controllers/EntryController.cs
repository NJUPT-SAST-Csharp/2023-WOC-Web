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
    public async Task<ActionResult<ICollection<Entry>>> GetEntriesByTags([FromQuery] List<string>? tagNames)
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

    //POST: 新建词条
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Entry>> PostEntry([FromForm] EntryDto entryDto)
    {
        var staff = User.FindFirstValue(ClaimTypes.Role);
        if (staff is "tourist") return Unauthorized("Only administrators or authors have permission to post entries");
        try
        {
            return Ok(await entryDataProviderService.PostEntry(entryDto));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //PUT: 编辑词条
    [Authorize]
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateEntry(EntryDto entryDto)
    {
        var staff = User.FindFirstValue(ClaimTypes.Role);

        if (staff is "tourist") return Unauthorized("Only administrators or authors have permission to update entries");

        try
        {
            return Ok(await entryDataProviderService.UpdateEntry(entryDto));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    //DELETE: 删除词条
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteEntry(string title)
    {
        var staff = User.FindFirstValue(ClaimTypes.Role);
        if (staff is not "adminstrator") return Unauthorized("Only administrators have permission to delete entries");
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
