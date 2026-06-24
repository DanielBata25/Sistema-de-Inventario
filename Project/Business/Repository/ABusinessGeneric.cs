using Business.Interfaces.IBusiness;

namespace Business.Repository
{
    public abstract class ABusinessGeneric<TCreateDto, TUpdateDto, TSelectDto, TEntity>
        : IBusiness<TCreateDto, TUpdateDto, TSelectDto>
        where TEntity : class
    {
        public abstract Task<IEnumerable<TSelectDto>> GetAllAsync();

        public abstract Task<TSelectDto?> GetByIdAsync(int id);

        public abstract Task<TSelectDto> CreateAsync(TCreateDto dto);

        public abstract Task<TSelectDto?> UpdateAsync(int id, TUpdateDto dto);

        public abstract Task<bool> DeleteAsync(int id);
    }
}