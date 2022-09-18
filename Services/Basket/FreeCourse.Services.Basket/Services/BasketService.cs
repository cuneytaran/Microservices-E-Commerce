using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services
{
    //TODO:Redis ayarı 6
    //redis ile bağlantıya geçme
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<Response<BasketDto>> GetBasket(string userId)
        {
            //_redisService servis üzerinden redise bağlantı kuruyoruz.
            var existBasket = await _redisService.GetDb().StringGetAsync(userId);

            if (String.IsNullOrEmpty(existBasket))//null veya boş iste
            {
                return Response<BasketDto>.Fail("Basket not found", 404);
            }
            //TODO:Deserialize işlemi
            //newtonsoft kullanamaya gerek yok. 3.1 den itibaren kendi içinde geliyor. Ekstra bir kütüphane kullanmaya gerek yok.
            //kullandığı kütüphane using System.Text.Json;
            return Response<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existBasket), 200);
        }

        public async Task<Response<bool>> SaveOrUpdate(BasketDto basketDto)
        {
            //TODO:Serialize işlemi
            var status = await _redisService.GetDb().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));//StringSetAsync=key, value istiyor ve sonuç bool dönüyor
            //TODO:if kısa yazımı
            return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket could not update or save", 500);
        }

        public async Task<Response<bool>> Delete(string userId)
        {
            var status = await _redisService.GetDb().KeyDeleteAsync(userId);
            return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket not found", 404);
        }
        //startupda İnterface eklemeyi unutma!!!
    }
}