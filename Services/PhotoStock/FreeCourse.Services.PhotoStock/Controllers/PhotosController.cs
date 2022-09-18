using FreeCourse.Services.PhotoStock.Dtos;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController//sharet referans ver ve CustomBaseControlleri al.
    {
        //TODO: Asenkron işlemi sadece hata fırlatarak sonlandırabiliriz!!!! yoksa arka planda çalışmaya devam eder.
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)//CancellationToken= buraya fotoğraf geldiğinde eğer bu endpoint işlemi sondırırsa fotoğraf kaydetmeyi sonlandırsın.
        {
            if (photo != null && photo.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);//Directory.GetCurrentDirectory()=gücel direktory al.

                using var stream = new FileStream(path, FileMode.Create);//dosya oluştur
                await photo.CopyToAsync(stream, cancellationToken);//dosyayı kopyala. bizim streame kopyalıyoruz. eğer browser kapatılırsa cancellationToken ile işlemi sonlandırabiliyorum.

                var returnPath = photo.FileName;//kaydedildikten sonra nasıl bir yol görünecek. http://wwww.api.com/photos/dosyaismi.jpg gibi

                PhotoDto photoDto = new() { Url = returnPath };

                return CreateActionResultInstance(Response<PhotoDto>.Success(photoDto, 200));
            }

            return CreateActionResultInstance(Response<PhotoDto>.Fail("photo is empty", 400));
        }


        [HttpDelete]
        public IActionResult PhotoDelete(string photoUrl)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoUrl);//Directory.GetCurrentDirectory()=güncel direktory ver ve silinecek dosya yolunu hazırla.
            if (!System.IO.File.Exists(path))//System.IO.File.Exists=böyle bir yol yok ise.
            {
                return CreateActionResultInstance(Response<NoContent>.Fail("photo not found", 404));
            }

            System.IO.File.Delete(path);//resmi sil

            return CreateActionResultInstance(Response<NoContent>.Success(204));
        }
    }
}