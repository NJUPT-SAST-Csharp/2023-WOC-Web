using GeneralWiki.Application;
using GeneralWiki.Data;
using GeneralWiki.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeneralWiki.Service;

public class PictureProvider(WikiContext context,IWebHostEnvironment hostEnvironment):IPictureProvider
{
    //POST:上传图片
    public async Task<Picture> UploadPicture(IFormFile pic)
    {
        if (pic.Length == 0) throw new Exception("please upload a picture");
        var rootPath = hostEnvironment.WebRootPath;
        var picName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(pic.FileName)}";
        if (string.IsNullOrEmpty(picName))
        {
            var thisPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot");
            if (!Directory.Exists(thisPath)) Directory.CreateDirectory(thisPath);
            var uploadsPath = Path.Combine(thisPath, "uploads");
            if (!Directory.Exists(uploadsPath)) Directory.CreateDirectory(uploadsPath);
        }
        var picPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot","uploads", picName);
        var showPath=Path.Combine("uploads", picName);
        await using var stream = new FileStream(picPath, FileMode.Create);
        await pic.CopyToAsync(stream);
        var picture = new Picture { PictureUrl = showPath };
        context.Pictures.Add(picture); 
        await context.SaveChangesAsync();
        return picture;
    }
    //GET:根据Id获取图片
    public async Task<FileContentResult> GetPictureById(int id)
    {
        var existPicture = await context.Pictures.FirstOrDefaultAsync(p => p.PictureId == id);
        if (existPicture == null) throw new Exception("There is no picture for this id");
        var imageBytes = File.ReadAllBytes(existPicture.PictureUrl);
        return new FileContentResult(imageBytes, "image/jpeg");
    }
}