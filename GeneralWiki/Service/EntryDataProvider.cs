using GeneralWiki.Application;
using GeneralWiki.Data;
using GeneralWiki.Models;
using GeneralWiki.Service.DtoService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeneralWiki.Service;

public class EntryDataProvider(WikiContext context):IEntryDataProvider
{
    // GET: 通过Id获取单个词条
    public async Task<EntryDto> GetEntryById(int id)
    {
        var entry = await context.Entries
            .Include(e=>e.Category)
            .Include(e=>e.Tags)
            .FirstOrDefaultAsync(e=>e.EntryId==id);
        if (entry == null) throw new Exception("There is no entry for this id");
        return new EntryDto
        {
            Id=entry.EntryId,
            Title = entry.Title,
            Content = entry.Content,
            CategoryName = entry.Category!.CategoryName,
            TagNames = entry.GetTagNames().ToList()
        };
    }
    
    //GET: 通过Title获取词条
    public async Task<List<EntryDto>> GetEntryByTitle(string title)
    {
        if (title==null) throw new Exception("please specify the title");
        var entries = await context.Entries
            .Where(entry => entry.Title.Contains(title))
            .Include(e=>e.Tags)
            .Include(entry => entry.Category)
            .ToListAsync();
        if(entries.Count == 0) throw new Exception("There is no entry with this title");
        return entries.Select(e => new EntryDto
        {
            Id=e.EntryId,
            Title = e.Title,
            Content = e.Content,
            CategoryName = e.Category!.CategoryName,
            TagNames = e.GetTagNames().ToList()
        }).ToList();
    }
    
    //GET: 通过Tags获取词条
    public async Task<ICollection<EntryDto>> GetEntriesByTags([FromQuery]List<string>? tagNames)
    {
        if (tagNames==null) throw new Exception("please specify the tags");
        var entries = await context.Entries
            .Where(entry => entry.Tags.Any(tag => tagNames.Contains(tag.TagName)))
            .OrderByDescending(entry => entry.Tags.Count)
            .Include(entry => entry.Category!)
            .Include(e=>e.Tags)
            .ToListAsync();
        if (entries.Count==0) throw new Exception("There is no entry under these tags");
        return entries.Select(entry => new EntryDto
        {
            Id=entry.EntryId,
            Title = entry.Title,
            Content = entry.Content,
            CategoryName = entry.Category!.CategoryName,
            TagNames = entry.GetTagNames().ToList()
        }).ToList();
    }
    
    //GET: 通过Category获取词条
    public async Task<ICollection<EntryDto>> GetEntriesByCategory(string categoryName)
    {
        if (categoryName==null) throw new Exception("please specify the category");

        var existCategory = await context.Categories
            .Where(c => c.CategoryName == categoryName)
            .ToListAsync();
        if (existCategory == null) throw new Exception("There is no such category");
        
        var entries = await context.Entries
            .Where(entry => entry.Category!.CategoryName == categoryName)
            .Include(e=>e.Tags)
            .Include(e=>e.Category)
            .ToListAsync();
        if (entries.Count==0) throw new Exception("There are no entries under this category");
        return entries.Select(entry => new EntryDto
        {
            Id=entry.EntryId,
            Title = entry.Title,
            Content = entry.Content,
            CategoryName = entry.Category!.CategoryName,
            TagNames = entry.GetTagNames().ToList()
        }).ToList();
    }

    // GET: 展示所有词条
    public async Task<IEnumerable<EntryDto>> GetEntries()
    {
        var entries=await context.Entries
            .Include(entry => entry.Category!)
            .Include(e=>e.Tags)
            .ToListAsync();
        if(entries==null)throw new Exception("No entry!");
        return entries.Select(entry => new EntryDto
        {
            Id=entry.EntryId,
            Title = entry.Title,
            Content = "",
            CategoryName = entry.Category!.CategoryName,
            TagNames = entry.GetTagNames().ToList()
        }).ToList();
    }
    
    //POST: 新建词条
    public async Task<int?> PostEntry(EntryDto entryDto)
    {
        //检查是否有已存在的词条
        var previousEntry = await context.Entries.FirstOrDefaultAsync(e => e.Title == entryDto.Title);
        
        if (previousEntry != null) throw new Exception("The entry has already exist!");
        
        var tagsInDto = entryDto.TagNames;//取出请求体中的Tag列表
        var entry = new Entry
        {
            EntryId=null,
            Title = entryDto.Title,
            Content = entryDto.Content,
            Category = await GetOrCreateCategory(entryDto.CategoryName)
        };
        
        //检查Dto的标签中是否有数据库中已经存在的标签，如果有，则把那些标签取出来
        var existingTags = await context.Tags
            .Where(tagInDataBase => tagsInDto.Contains(tagInDataBase.TagName))
            .ToListAsync(); //EF Core开始跟踪
        
        foreach (var tagInDto in tagsInDto)
        {
            //检查数据库中已经存在的标签中的哪一个是当前的tag
            var existingTag = existingTags.FirstOrDefault(tag => tag.TagName == tagInDto);
            var newTag = new Tag
            {
                TagName = tagInDto
            };
            if (existingTag == null)//如果数据库中没有这个标签，就存起来
            {
                entry.Tags.Add(newTag);
                newTag.Entries?.Add(entry);
            }
            else//如果有，则用现有的，并更新导航属性
            {
                entry.Tags.Add(existingTag);
                existingTag.Entries?.Add(entry);
            }
        }
        await context.Entries.AddAsync(entry);
        await context.SaveChangesAsync();
        return entry.EntryId;
    }

    //PUT: 编辑词条
    public async Task<string> UpdateEntry(EntryDto entryDto)
    {
        var previousEntry = await context.Entries.FirstOrDefaultAsync(e => e.EntryId == entryDto.Id);
        if (previousEntry == null) throw new Exception("The Entry isn't exist!");
        context.Entries.Remove(previousEntry);
        var tagsInDto = entryDto.TagNames;
        var entry = new Entry
        {
            Title = entryDto.Title,
            Content = entryDto.Content,
            Category = await GetOrCreateCategory(entryDto.CategoryName)
        };
        var existingTags = await context.Tags
            .Where(tagInDataBase => tagsInDto.Contains(tagInDataBase.TagName))
            .ToListAsync(); 
        foreach (var tagInDto in tagsInDto)
        {
            var existingTag = existingTags.FirstOrDefault(tag => tag.TagName == tagInDto);
            var newTag = new Tag
            {
                TagName = tagInDto
            };
            if (existingTag == null)
            {
                entry.Tags.Add(newTag);
                newTag.Entries?.Add(entry);
            }
            else
            {
                entry.Tags.Add(existingTag);
                existingTag.Entries?.Add(entry);
            }
        }
        await context.Entries.AddAsync(entry);
        await context.SaveChangesAsync();
        return "Update success!";
    }
    
    //DELETE：删除词条
    public async Task<string> DeleteEntry(string title)
    {
        var entry = await context.Entries.FirstOrDefaultAsync(e=>e.Title==title);
        if (entry == null) throw new Exception("There is no entry with this title");
        if(entry.ImgUrl!=null&& File.Exists(entry.ImgUrl))
            File.Delete(entry.ImgUrl);
        context.Entries.Remove(entry);
        await context.SaveChangesAsync();
        return "Delete success";
    }

    private async Task<Category> GetOrCreateCategory(string categoryName)
    {
        var existCategory = await context.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName);
        if (existCategory != null) return existCategory;
        var newCategory = new Category
        {
            CategoryName = categoryName,
            Description = ""
        };
        context.Categories.Add(newCategory);
        await context.SaveChangesAsync();
        return newCategory;
    }
}