using GeneralWiki.Application;
using GeneralWiki.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EntryController:ControllerBase
{
    private readonly IEntryDataProvider _entryDataProvider;

    public EntryController(IEntryDataProvider entryDataProvider)
    {
        _entryDataProvider = entryDataProvider;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Entry>>> GetEntries()
    {
        await _entryDataProvider.GetEntries();
        return NotFound();
    }
    
    
}