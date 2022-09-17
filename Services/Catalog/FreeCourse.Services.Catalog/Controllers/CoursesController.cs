using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Shared.ControllerBases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : CustomBaseController
    {        
        //Önemli Not:Swagger mutlaka controllerin üstüne [HttpGet] put post istiyor. yoksa hata alırsın!!!!!
        //internal sadece bulunduğu projede erişim sağlayabilir.
        //CustomBaseController=içeriği kontrol etsin ve durum dönüştürsün

        private readonly ICourseService _courseService;

        //ctor add services Alt+Enter
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _courseService.GetAllAsync();

            return CreateActionResultInstance(response);
        }

        //{id} olmasaydı courses?id=5 demen lazımdı
        //courses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _courseService.GetByIdAsync(id);

            return CreateActionResultInstance(response);//CustomBaseControllerden dönüş parametlerini ekleyecek. 404 alaacaksa 404 body ye gönderecek.
        }

        [HttpGet]
        [Route("/api/[controller]/GetAllByUserId/{userId}")]//GetAllByUserId ulaşması için root yazıyoruz. yoksa buraya bulamaz.
        public async Task<IActionResult> GetAllByUserId(string userId)
        {
            var response = await _courseService.GetAllByUserIdAsync(userId);

            return CreateActionResultInstance(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateDto courseCreateDto)
        {
            var response = await _courseService.CreateAsync(courseCreateDto);

            return CreateActionResultInstance(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateDto courseUpdateDto)
        {
            var response = await _courseService.UpdateAsync(courseUpdateDto);

            return CreateActionResultInstance(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _courseService.DeleteAsync(id);

            return CreateActionResultInstance(response);
        }
    }
}