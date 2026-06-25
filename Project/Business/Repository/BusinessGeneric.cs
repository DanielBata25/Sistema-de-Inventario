using Data.Interfaces.IRepository;
using MapsterMapper;
using Utilities.Exceptions;
using Utilities.Helpers.Business;

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
            try
            {
                var entities = await Data.GetAllAsync();
                return Mapper.Map<IEnumerable<TSelectDto>>(entities);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener los registros.", ex);
            }
        }

        public override async Task<TSelectDto?> GetByIdAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                var entity = await Data.GetByIdAsync(id);

                if (entity == null)
                {
                    return default;
                }

                return Mapper.Map<TSelectDto>(entity);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al obtener el registro con ID {id}.", ex);
            }
        }

        public override async Task<TSelectDto> CreateAsync(TCreateDto dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                var entity = Mapper.Map<TEntity>(dto);
                var created = await Data.AddAsync(entity);

                return Mapper.Map<TSelectDto>(created);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al crear el registro.", ex);
            }
        }

        public override async Task<TSelectDto?> UpdateAsync(int id, TUpdateDto dto)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");
                BusinessValidationHelper.ThrowIfNull(dto, "El DTO no puede ser nulo.");

                var entity = Mapper.Map<TEntity>(dto);

                var property = typeof(TEntity).GetProperty("Id");

                if (property != null)
                {
                    property.SetValue(entity, id);
                }

                var updated = await Data.UpdateAsync(entity);

                return Mapper.Map<TSelectDto>(updated);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al actualizar el registro con ID {id}.", ex);
            }
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            try
            {
                BusinessValidationHelper.ThrowIfZeroOrLess(id, "El ID debe ser mayor que cero.");

                return await Data.DeleteAsync(id);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Error al eliminar el registro con ID {id}.", ex);
            }
        }
    }
}