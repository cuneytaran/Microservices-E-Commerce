using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Settings
{
    //TODO:Redis ayarı 3
    //docker da hangi host ve port kullanılacağı tanımlandığı yer.
    public class RedisSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}