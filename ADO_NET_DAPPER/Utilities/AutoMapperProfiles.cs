using ADO_NET_DAPPER.DTOs.Articulos;
using ADO_NET_DAPPER.Entities;
using AutoMapper;

namespace ADO_NET_DAPPER.Utilities
{
	public class AutoMapperProfiles: Profile
	{
        public AutoMapperProfiles()
        {
            CreateMap<ArticuloRequest, Articulo>();
            CreateMap<Articulo, ArticuloResponse>();
        }
    }
}
