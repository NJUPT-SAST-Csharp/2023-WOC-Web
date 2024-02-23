using GeneralWiki.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Application;

public interface IPictureProvider
{
    public Task<Picture> UploadPicture(IFormFile pic);
    public Task<FileContentResult> GetPictureById(int id);
}