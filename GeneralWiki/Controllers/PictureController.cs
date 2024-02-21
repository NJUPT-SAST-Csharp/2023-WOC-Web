using GeneralWiki.Application;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PictureController(IPictureProvider pictureProvider):ControllerBase
{
    //POST:上传图片
    [HttpPost]
    public async Task<ActionResult<string>> UploadPicture(IFormFile pic)
    {
        try
        {
            return Ok(await pictureProvider.UploadPicture(pic));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //GET:通过Id获取图片
    [HttpGet]
    public async Task<ActionResult<string>> GetPictureById(int id)
    {
        try
        {
            return Ok(await pictureProvider.GetPictureById(id));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}