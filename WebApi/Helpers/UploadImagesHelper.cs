using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Application.Exceptions;
using Domain.Entities;

namespace WebApi.Helpers
{
  public class UploadImagesHelper
  {
    static List<string> allowedExtensions = new List<string> { ".jpg", ".png" };
    public static async Task<IList<Image>> UploadImages(HttpRequest request)
    {
      
      var formCollection = await request.ReadFormAsync();

      var folderName = Path.Combine("Resources", "Images");
      var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

      var imagesToAdd = new List<Image>();

      foreach (var file in formCollection.Files)
      {
        if (file.Length > 0)
        {
          var fileExtension = Path.GetExtension(file.FileName).ToLower();
          if (!allowedExtensions.Contains(fileExtension)) throw new ApiException("File extension not supported");

          var fileName = Guid.NewGuid().ToString() + fileExtension;
          var fullPath = Path.Combine(pathToSave, fileName);
          var dbPath = Path.Combine(folderName, fileName);
          using (var stream = new FileStream(fullPath, FileMode.Create))
          {
            file.CopyTo(stream);
          }

          imagesToAdd.Add(new Image { Path = dbPath });
        }
        else
        {
          throw new ApiException("A problem occured with one of files");
        }
      }

      return imagesToAdd;
    }
  }
}
