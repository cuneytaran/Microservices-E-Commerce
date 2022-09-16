using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Dtos
{
    public class ErrorDto//return yaparken, eğerki birşey dönmek istemiyorsam ve sadece hata mesajını dönmek istiyorsam bunumu kullanacağız.
    {
        public List<string> Errors { get; set; }
    }
}