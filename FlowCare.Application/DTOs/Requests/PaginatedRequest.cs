using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowCare.Application.DTOs.Requests
{
    public class PaginatedRequest
    {
        
        public int skip = 0;

        [Range(1,20)]
        public int take = 20;
    }
}
