using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Dtos
{//sepet içeriği= mitar, cursun id si vs.. bilgiler tutulacak bölüm.
    public class BasketItemDto
    {
        public int Quantity { get; set; }

        public string CourseId { get; set; }
        public string CourseName { get; set; }

        public decimal Price { get; set; }
    }
}