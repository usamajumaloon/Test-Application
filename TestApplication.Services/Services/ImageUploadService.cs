using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using TestApplication.Services.Models;
using UploadResult = TestApplication.Services.Models.UploadResult;

namespace TestApplication.Services.Services
{
    public class ImageUploadService : IImageUploadService
    {
        private const string Tags = "backend_PhotoAlbum";
        private readonly AppDbContext context;
        private readonly Cloudinary cloudinary;

        public ImageUploadService(AppDbContext context, Cloudinary cloudinary)
        {
            this.context = context;
            this.cloudinary = cloudinary;
        }

        public async Task<List<UploadResult>> UploadImage(ImageUploadModel imageUploadModel)
        {
            try
            {
                var results = new List<Dictionary<string, string>>();
                var uploadResults = new List<UploadResult>();
                var imagesList = new List<IFormFile>();
                imagesList.Add(imageUploadModel.NicCopy);
                imagesList.Add(imageUploadModel.ProfilePic);
                if (imagesList != null)
                {
                    IFormatProvider provider = CultureInfo.CreateSpecificCulture("en-US");
                    foreach (var image in imagesList)
                    {
                        var result = await cloudinary.UploadAsync(new ImageUploadParams
                        {
                            File = new FileDescription(image.FileName,
                                image.OpenReadStream()),
                            Tags = Tags
                        }).ConfigureAwait(false);

                        var tempResult = new UploadResult
                        {
                            Name = image.FileName,
                            Bytes = (int)result.Bytes,
                            CreatedAt = DateTime.Now,
                            Format = result.Format,
                            Height = result.Height,
                            Path = result.Url.AbsolutePath,
                            PublicId = result.PublicId,
                            ResourceType = result.ResourceType,
                            SecureUrl = result.SecureUrl.AbsoluteUri,
                            Signature = result.Signature,
                            Type = result.JsonObj["type"]?.ToString(),
                            Url = result.Url.AbsoluteUri,
                            Version = int.Parse(result.Version, provider),
                            Width = result.Width
                        };

                        uploadResults.Add(tempResult);
                        await context.UploadResults.AddAsync(tempResult).ConfigureAwait(false);
                    }

                }
                await context.SaveChangesAsync().ConfigureAwait(false);
                return uploadResults;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<UploadResult>> GetImages()
        {
            try
            {
                var results = await context.UploadResults.ToListAsync();
                return results;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
