using Microsoft.AspNetCore.Mvc.Rendering;

namespace Desafio_Backend.Services
{
    public static class CnhHelper
    {
        public readonly record struct CnhType(string DisplayName, string Value);

        // this should be a table - TODO
        private static List<CnhType> _cnhTypes = [new CnhType("A", "A"), new CnhType("B", "B"), new CnhType("A+B", "AB")];

        public static CnhType A => _cnhTypes[0];

        public static CnhType AB => _cnhTypes[2];

        public static CnhType B => _cnhTypes[1];

        public static List<SelectListItem> GetAllTypesAsDropdown()
        {
            return _cnhTypes.Select(cnh => new SelectListItem(cnh.DisplayName, cnh.Value)).ToList();
        }
    }
}