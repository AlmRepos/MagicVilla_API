using MagicVilla_Web.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.ViewModels
{
    public class VillaNumberDeleteVM
    {
        public VillaNumberDeleteVM()
        {
            Villa = new();
        }

        public VillaNumberDTO Villa { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
