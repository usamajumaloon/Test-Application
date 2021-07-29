using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestApplication.Services.Constants;
using TestApplication.Services.Models;
using TestApplication.Services.Services;

namespace TestApplication.WEB.Controllers
{
    public class ImageUploadController : ControllerBase
    {
        private readonly IImageUploadService imageUploadService;

        public ImageUploadController(IImageUploadService imageUploadService)
        {
            this.imageUploadService = imageUploadService;
        }

        [HttpPost("image-upload"), AllowAnonymous]
        public async Task<IActionResult> PostAsync(ImageUploadModel model)
        {
            try
            {
                return Ok(await imageUploadService.UploadImage(model));
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        protected IActionResult HandleException(Exception ex)
        {
            var exType = ex.GetType().Name;

            switch (exType)
            {
                case ExceptionType.ArgumentException: return Conflict(ex.Message);
                case ExceptionType.UnauthorizedAccessException: return StatusCode(403, ex.Message);
                case ExceptionType.AuthenticationException: return Unauthorized(ex.Message);

                //*****  status code 404 range *********
                case ExceptionType.NullReferenceException:
                    return NotFound(ex.Message);
                //*****  end status code 404 range *********


                //*****  status code 500 range *********
                case ExceptionType.SqlException:
                default: return StatusCode(500, ex.Message);
                    //*****  end status code 500 range *********

            }
        }
    }
}
