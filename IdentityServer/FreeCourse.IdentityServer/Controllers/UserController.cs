using FreeCourse.IdentityServer.Dtos;
using FreeCourse.IdentityServer.Models;
using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace FreeCourse.IdentityServer.Controllers
{
    //TODO: İdentity kurulumu
    //identity server4 kullanmak için cmd ekranına dotnet new -i identityserver4.templates komutu ver.
    //IdentityServer adında dışarıda bir klasör oluştur el ile. sonra bu yolu koplaya.bu klasörün içine kuracağız. sonra projemize ekleyeceğiz.
    //cmd ekranına dotnet new is4aspid --name projeismi ve enter yap 
    //uyarı çıkacak veritabanını singlesinmi N yap.
    //projenin kurulumu gerçekleşir.
    //projende bir bir klasör oluştur (IdentityServer) mesela
    //klasöre sağ tıkla Add - Existing Project
    //yapmış olduğun klasörü bul seç
    //başka yerde oluşturmuş olduğun proje bu projenin içine entegere etmiş olacaksın.

    //********************************
    //https://identityserver4.readthedocs.io/en/latest/
    //https://identityserver4.readthedocs.io/en/latest/endpoints/discovery.html
    //Discorvery Endpoint=o anda bana sunulan tokenle sunulan endopintleri verir
    //http://localhost:59318/.well-known/openid-configuration // ile sorgu atarsan mevcut olan tüm endpointler gelir.
    //token almak için 4 farklı yöntemler vardır.bizim için önemli olan Resaurce Owner Credentianls ve Client Credentials Grand Type
    //client credintiantial clientId ve clientsecret göndeririz ve bize token gönderir.burdaki client Asp.Net MVC tüm proje gibi düşün.Çünkü Asp.Net MVC ile identity farklı projeler izole projeler gibi düşün. 
    //istek yaparken resource owner cradentials. clientId, clientsecret/email/pasword göndeririz bir token alırız. almış olduğumuz token ile diğer sayfalara istek yapabiliyor olacağız.user ile ilgili sayfalarda kullanacağız.
    //resource owner cradentials izin tipinde eğerki client güvenli ise kullanıyoruz. yani client veya api ikinisini biz yapıyorsak. yani bizim kontrolümüzde ise.
    //client credentials sabit token dır. genel bir tokendir. kullanıcıya özgü değildir. 30 günlük geçerliliği vardır. 
    //resource token kullanıcıya özgü bir token dir.

    //Burada identity server hem benim için token dağıyor, aynı zamanda bir kaç tane endpointlerine token doğrulama işlemi yapıyor.
    //returnda Response olarak dönmek için Response sharedin içinde olduğu uçin referans olarak shared e eklemen gerekiyor.

    //** User işlemleri bittikten sonra Config.cs dosyasına git. orada. hangi identity serverden kimden token alacak, hangi microservislere istek yapılacağını belirleyecek.


    [Authorize(LocalApi.PolicyName)]// PolicyName=IdentityServerAccessToken yani token alması gerekiyor.Bu aslında bir policy ismidir.Yani token içinde IdentityServerApi yazısını bekilyor. içinde bu olmalı yoksa devam ettirmez.
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignupDto signupDto)
        {
            var user = new ApplicationUser//User bilgileri ekleniyor
            {
                UserName = signupDto.UserName,
                Email = signupDto.Email,
                City = signupDto.City
            };

            var result = await _userManager.CreateAsync(user, signupDto.Password);//user kayıt işlemi

            if (!result.Succeeded)
            {
                return BadRequest(Response<NoContent>.Fail(result.Errors.Select(x => x.Description).ToList(), 400));//NoContent=içeriği boş
            }

            return NoContent();//geriye birşey dönmek istemiyoruz.
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userIdClaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null) return BadRequest();

            var user = await _userManager.FindByIdAsync(userIdClaim.Value);

            if (user == null) return BadRequest();

            return Ok(new { Id = user.Id, UserName = user.UserName, Email = user.Email, City = user.City });
        }
    }
}
