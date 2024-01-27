using GeneralWiki.Application;
using GeneralWiki.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Service;

public class EntryDataProvider:IEntryDataProvider
{
    public Task<ActionResult<Entry>> GetEntry(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> PutEntry(int id, Entry entry)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> DeleteEntry(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult<IEnumerable<Entry>>> GetEntries()
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult<Entry>> PostEntry(Entry entry, IFormFile file)
    {
        throw new NotImplementedException();
    }
}