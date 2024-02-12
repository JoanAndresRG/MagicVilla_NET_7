using AutoMapper;
using Entities.DTOs;
using Entities.Entiti;

namespace MagicVilla_API_2024.Utils
{
    public class MapperConfig : Profile
    {
        #region VillaMapper
        public MapperConfig()
        {
            CreateMap<Villa, VillaDTO>().ReverseMap();
        }
        #endregion
    }
}
