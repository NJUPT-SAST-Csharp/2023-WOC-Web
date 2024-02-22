using GeneralWiki.Service.DtoService;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Application;

public interface IEntryDataProvider
{
    public Task<EntryDto> GetEntryById(int id);
    public Task<List<EntryDto>> GetEntryByTitle(string title);
    public Task<ICollection<EntryDto>> GetEntriesByTags([FromQuery] List<string>? tagNames);
    public Task<ICollection<EntryDto>> GetEntriesByCategory(string categoryName);
    public Task<IEnumerable<EntryDto>> GetEntries();
    public Task<EntryDto> PostEntry(EntryDto entryDto);
    public Task<EntryDto> UpdateEntry(EntryDto entryDto);
    public Task<string> DeleteEntry(string title);
}