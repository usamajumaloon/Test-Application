using System.Collections.Generic;
using System.Threading.Tasks;
using TestApplication.Services.Models;

namespace TestApplication.Services.Services
{
    public interface IImageUploadService
    {
        Task<List<UploadResult>> UploadImage(ImageUploadModel imageUploadModel);
    }
}