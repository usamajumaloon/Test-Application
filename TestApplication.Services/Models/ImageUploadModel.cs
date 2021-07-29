using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestApplication.Services.Models
{
    public class ImageUploadModel
    {
        public string Name { get; set; }
        public string Nic { get; set; }
        public IFormFile NicCopy { get; set; }
        public IFormFile ProfilePic { get; set; }
    }
}
