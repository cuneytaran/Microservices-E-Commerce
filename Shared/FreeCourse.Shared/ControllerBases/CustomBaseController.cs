using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.ControllerBases
{
    //Tüm microservislerin ortak olarak kullanacağı bir yapı tanımlıyoruz
    public class CustomBaseController : ControllerBase
    {
        //ControllerBase= normalde ulaşılamaz biz FreeCourse.Shared projenin üzerine çift tıklayıp içine <FrameworkReference Include="Microsoft.AspNetCore.App" /> bunu eklersek ControllerBase artık ulaşabiliriz.
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode//response dan 404 gelirse 404 alıp body içine gömer.
            };
        }
    }
}