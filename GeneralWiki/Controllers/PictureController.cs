using GeneralWiki.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GeneralWiki.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class PictureController(IPictureProvider pictureProvider) : ControllerBase
{
    //POST:上传图片
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<string>> UploadPicture(IFormFile pic)
    {
        var staff = User.FindFirstValue(ClaimTypes.Role);
        if (staff is "tourist") return Unauthorized("Only administrators or authors have permission to upload picture");

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