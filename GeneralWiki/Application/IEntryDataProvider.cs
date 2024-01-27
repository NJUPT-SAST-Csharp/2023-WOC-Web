using GeneralWiki.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Application;

public interface IEntryDataProvider
{
    // GET: 展示单个词条
    public Task<ActionResult<Entry>> GetEntry(int id);
    
    // PUT: 编辑词条
    public Task<IActionResult> PutEntry(int id, Entry entry);
    
    // DELETE: 删除词条
    public Task<IActionResult> DeleteEntry(int id);
    
    // GET: 展示所有词条
    public Task<ActionResult<IEnumerable<Entry>>>GetEntries();
    
    // POST: 新建词条
    public Task<ActionResult<Entry>> PostEntry([FromForm] Entry entry, IFormFile file);
}