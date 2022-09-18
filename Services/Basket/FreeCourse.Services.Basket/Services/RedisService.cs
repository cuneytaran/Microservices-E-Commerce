using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Services
{
    //TODO:Redis ayarı 4
    public class RedisService
    {
        //veritabanınya haberleşmesi için host ve port ayarları yapılıyor. veritabanı dediğim dockerdeki redisten bahsediyorum
        private readonly string _host;

        private readonly int _port;

        //ConnectionMultiplexer=redisexchange den gelen kütüphane
        private ConnectionMultiplexer _ConnectionMultiplexer;

        public RedisService(string host, int port)
        {
            _host = host;
            _port = port;
        }

        //dockerdeki redis ile bağlantı adresi işlemi
        public void Connect() => _ConnectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");
        //dockerdacdeki veritabanı bağlantı. burdaki db=1 bir çok veritabanı redis kullanırsın. bunları ayırmak için kullanıyoruz.Biz 1.redis veritabanını kaydetmek istiyoruz.
        public IDatabase GetDb(int db = 1) => _ConnectionMultiplexer.GetDatabase(db);//redis connectionstring gibi düşün. GetDb üzerinden erişim sağlayacağız.
    }
}