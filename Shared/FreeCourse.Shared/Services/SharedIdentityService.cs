using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeCourse.Shared.Services
{
    //TODO:Her seferinde token içinden userid yi almak yerine otomatikleştirme 2
    public class SharedIdentityService : ISharedIdentityService
    {
        private IHttpContextAccessor _httpContextAccessor;//IHttpContextAccessor=Httpcontex nesnesine ulaşmak için kullanıyoruz. bunun özelliği. gelen token içindeki clean bilgileri tutulur. bunları okumak için kullanıyoruz.
        //startup içine IHttpContextAccessor eklemeyi unutma.Basket servis içindeki projenin startup içine...services.AddHttpContextAccessor();
        //aynı zamanda sharedservisini kullanmak için basket servis içindeki startup içinde serviside eklemen gerekiyor. services.AddScoped<ISharedIdentityService, SharedIdentityService>();
        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirst("sub").Value;
        //public string GetUserId => _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value; //üst satırla aynı işlemi yapıyor.
        //buradaki claims dediğimiz. token içindeki sub gibi user bilgilerinin tutulduğu yerdir.
    }
}