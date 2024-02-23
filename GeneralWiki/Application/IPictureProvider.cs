using GeneralWiki.Models;

namespace GeneralWiki.Application;

public interface IPictureProvider
{
    public Task<string> UploadPicture(IFormFile pic);
    public Task<string> GetPictureById(int id);
}