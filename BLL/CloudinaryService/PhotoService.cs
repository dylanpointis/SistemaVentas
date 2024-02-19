using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;


namespace BLL.CloudinaryService
{
    public class PhotoService : IPhotoService
    {
        ////completar credenciales en appsettings.json
        private readonly Cloudinary _cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(account);
        }


        public async Task<ImageUploadResult> AgregarFoto(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if(file.Length > 0) 
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream)
                    //Transformation
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> BorrarFoto(string publicUrl)
        {
            var publicId = publicUrl.Split('/').Last().Split('.')[0]; //CONSIGUE EL ID DE LA FOTO DE LA URL
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}
