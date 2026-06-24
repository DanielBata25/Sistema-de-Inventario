namespace Business.Interfaces.IBusiness
{
    public interface IBusiness<TCreateDto, TUpdateDto, TSelectDto>
    {
        Task<IEnumerable<TSelectDto>> GetAllAsync();

        Task<TSelectDto?> GetByIdAsync(int id);

        Task<TSelectDto> CreateAsync(TCreateDto dto);

        Task<TSelectDto?> UpdateAsync(int id, TUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}