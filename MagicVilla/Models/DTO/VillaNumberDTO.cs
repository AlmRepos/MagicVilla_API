using System.ComponentModel.DataAnnotations;

namespace MagicVilla.Models.DTO
{
    public class VillaNumberDTO
    {
        public int VillaNo { get; set; }
        public string SpecialDetails { get; set; }
        public int VillaId { get; set; }
        public VillaDTO Villa { get; set; }

    }
}
