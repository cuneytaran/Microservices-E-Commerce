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
    //TODO:Authorize tek bir sefer çalıştırma. startupa git
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : CustomBaseController
    {
        //Önemli Not:Swagger mutlaka controllerin üstüne [HttpGet] put post istiyor. yoksa hata alırsın!!!!!
        //internal sadece bulunduğu projede erişim sağlayabilir.
        //CustomBaseController=içeriği kontrol etsin ve durum dönüştürsün
        private readonly ICategoryService _categoryService;

        //ctor add services Alt+Enter
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();

            return CreateActionResultInstance(categories);//CustomBaseControllerden dönüş parametlerini ekleyecek. 404 alaacaksa 404 body ye gönderecek.
        }

        //{id} olmasaydı courses?id=5 demen lazımdı
        //categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            return CreateActionResultInstance(category);//CustomBaseControllerden dönüş parametlerini ekleyecek. 404 alaacaksa 404 body ye gönderecek.
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            var response = await _categoryService.CreateAsync(categoryDto);

            return CreateActionResultInstance(response);
        }
    }
}