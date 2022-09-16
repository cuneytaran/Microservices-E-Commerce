using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos
{
    //işlemler sonucu return kısmında bir açıklama yaparak dönüş yapmak için kullanıyoruz. mesala update yaptık. sonucu bir null yerine bir metin döndüreceğiz.
    public class Response<T>
    {
        public T Data { get; set; } // başarılı olduktan sonra dönecek data.

        [JsonIgnore] // JsonIgnore= status kodunu kendi içimde kullanak istiyorum. yani zaten dönüşte 400 vs yapıyoruz. bunu göndermeye gerek yok. ama bu dönüşü program içinde kullanmak için yapıyoruz.
        public int StatusCode { get; set; }

        [JsonIgnore]
        public bool IsSuccessful { get; set; }//başarılımı, başarılı değilmi belirleyeceğiz.

        public List<string> Errors { get; set; }// hata da dönüş bilgileri

        // ********return yaparken successde içinde gönderilecek verier belirleniyor*********
        //return yaparken  dönüşte data ve satus kodu dönecekse
        public static Response<T> Success(T data, int statusCode) // data ve statusCode geliyorsa...
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccessful = true }; // dönüşü böyle olacak.
        }

        //return yaparken  dönüşte sadece satus kodu dönecekse
        public static Response<T> Success(int statusCode)// sadece statusCode geliyorsa...
        {
            return new Response<T> { Data = default(T), StatusCode = statusCode, IsSuccessful = true };
        }

        //return da hata alındıysa dönecek liste hata veriler ve status codu
        public static Response<T> Fail(List<string> errors, int statusCode) // başarısız durumu var ise list şeklinde dönüş yapacak.

        {
            return new Response<T>
            {
                Errors = errors,
                StatusCode = statusCode,
                IsSuccessful = false
            };
        }

        //return da hata alındıysa dönecek tek hata geliyorsa ve status codu
        public static Response<T> Fail(string error, int statusCode) // başarısız liste değilse
        {
            return new Response<T> { Errors = new List<string>() { error }, StatusCode = statusCode, IsSuccessful = false };
        }
    }
}