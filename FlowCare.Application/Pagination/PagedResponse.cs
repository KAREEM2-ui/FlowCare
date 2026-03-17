using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowCare.Application.Pagination
{
    public class PagedResponse<T> 
    {
        public T Result { get; set; }

        public int Total {  get; set; }
    }
}
