using Entity.DTOs.Products;
using Entity.DTOs.Users;
using Entity.Model;
using Entity.Model.Auth;
using Mapster;

namespace Business.Mapping
{
    public static class MapsterConfig
    {
        public static TypeAdapterConfig Register()
        {
            var config = TypeAdapterConfig.GlobalSettings;

            config.NewConfig<ProductCreateDto, Product>()
                .Ignore(dest => dest.Id)
                .Ignore(dest => dest.Activo)
                .Ignore(dest => dest.FechaCreacion)
                .Ignore(dest => dest.FechaActualizacion!);

            config.NewConfig<ProductUpdateDto, Product>()
                .Ignore(dest => dest.FechaCreacion)
                .Ignore(dest => dest.FechaActualizacion!);

            config.NewConfig<Product, ProductSelectDto>();

            config.NewConfig<User, UserSelectDto>();

            return config;
        }
    }
}