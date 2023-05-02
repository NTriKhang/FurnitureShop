using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Models.dtos
{
    public class ImageUpdateDto
    {
        public string userName { set; get; } = string.Empty;
        public string base64data { get; set; } = string.Empty;
        public string fileName { get; set; } = string.Empty;
    }
}
