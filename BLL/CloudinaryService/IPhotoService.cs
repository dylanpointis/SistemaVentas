using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.CloudinaryService
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AgregarFoto(IFormFile file);
        Task<DeletionResult> BorrarFoto(string publicUrl);

    }
}
