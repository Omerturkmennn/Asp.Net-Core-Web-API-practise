using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities1.RequestFeatures
{
    public abstract class RequestParameters
    {
        const int maxPageSize = 50; //Kullanýcý ne isterse istesin, bir sayfada en fazla 50 veri gönderebilirsin
        public int PageNumber { get; set; } = 1;
        private int _pageSize { get; set; } = 10;
        public int PageSize
        {
           get { return _pageSize; }
           set { _pageSize = value > maxPageSize ? maxPageSize : value ; }
        }
    }
}
