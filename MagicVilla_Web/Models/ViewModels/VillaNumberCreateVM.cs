using MagicVilla_Web.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.ViewModels
{
    public class VillaNumberCreateVM
    {
        public VillaNumberCreateVM()
        {
            createDTO = new();
        }

        public VillaNumberCreateDTO createDTO { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> VillaList { get; set; }
    }
}
