using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Services
{
    //TODO:Her seferinde token içinden userid yi almak yerine otomatikleştirme 1
    //yani token içindeki sub: olan veri userid demektir. bunu almak yerine bir servis tanımlıyoruz. ve otomatik olarak oradan tanımlandığ yerden çekeceğiz
    public interface ISharedIdentityService
    {
        public string GetUserId { get; }
    }
}