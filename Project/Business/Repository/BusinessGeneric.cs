using Data.Interfaces.IRepository;
using MapsterMapper;

namespace Business.Repository
{
    public class BusinessGeneric<TCreateDto, TUpdateDto, TSelectDto, TEntity>
        : ABusinessGeneric<TCreateDto, TUpdateDto, TSelectDto, TEntity>
        where TEntity : class
    {
        protected readonly IDataGeneric<TEntity> Data;
        protected readonly IMapper Mapper;

        public BusinessGeneric(IDataGeneric<TEntity> data, IMapper mapper)
        {
            Data = data;
            Mapper = mapper;
        }

        public override async Task<IEnumerable<TSelectDto>> GetAllAsync()
        {
            var entities = await Data.GetAllAsync();
            return Mapper.Map<IEnumerable<TSelectDto>>(entities);
        }

        public override async Task<TSelectDto?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser mayor que cero.");
            }

            var entity = await Data.GetByIdAsync(id);

            if (entity == null)
            {
                return default;
            }

            return Mapper.Map<TSelectDto>(entity);
        }

        public override async Task<TSelectDto> CreateAsync(TCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "El DTO no puede ser nulo.");
            }

            var entity = Mapper.Map<TEntity>(dto);
            var created = await Data.AddAsync(entity);

            return Mapper.Map<TSelectDto>(created);
        }

        public override async Task<TSelectDto?> UpdateAsync(int id, TUpdateDto dto)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser mayor que cero.");
            }

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "El DTO no puede ser nulo.");
            }

            var entity = Mapper.Map<TEntity>(dto);

            var property = typeof(TEntity).GetProperty("Id");

            if (property != null)
            {
                property.SetValue(entity, id);
            }

            var updated = await Data.UpdateAsync(entity);

            return Mapper.Map<TSelectDto>(updated);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser mayor que cero.");
            }

            return await Data.DeleteAsync(id);
        }
    }
}