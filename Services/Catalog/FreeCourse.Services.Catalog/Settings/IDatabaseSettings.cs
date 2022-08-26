using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Settings
{
    public interface IDatabaseSettings //appsettingsden gelen değişkenlerin isimlerini buraya yazıyoruz.
    {
        public string CourseCollectionName { get; set; }
        public string CategoryCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    //DatabaseSettings classında implement ediyoruz. Çünkü
}