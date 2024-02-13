using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.Models.ViewModels
{
	public class ProductVM
	{
        public IEnumerable<SelectListItem> MyProperty { get; set; }
    }
}
