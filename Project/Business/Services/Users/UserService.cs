using Business.Interfaces.Implements.Users;
using Business.Repository;
using Data.Interfaces.Implements.Users;
using Data.Interfaces.IRepository;
using Entity.DTOs.Users;
using Entity.Model.Auth;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

namespace Business.Services.Users
{
    public class UserService
        : BusinessGeneric<UserCreateDto, UserUpdateDto, UserSelectDto, User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(
            IDataGeneric<User> data,
            IMapper mapper,
            IUserRepository userRepository) : base(data, mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<UserSelectDto?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("El correo electrónico es obligatorio.");
            }

            var user = await _userRepository.GetByEmailAsync(NormalizeEmail(email));

            if (user == null)
            {
                return null;
            }

            return Mapper.Map<UserSelectDto>(user);
        }

        public override async Task<UserSelectDto> CreateAsync(UserCreateDto dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Los datos del usuario son obligatorios.");
            }

            ValidateUser(dto.Nombre, dto.Email, dto.Rol);
            ValidatePassword(dto.Password);

            var email = NormalizeEmail(dto.Email);
            var role = NormalizeRole(dto.Rol);

            var exists = await _userRepository.EmailExistsAsync(email);

            if (exists)
            {
                throw new InvalidOperationException("Ya existe un usuario registrado con este correo electrónico.");
            }

            var user = new User
            {
                Nombre = dto.Nombre.Trim(),
                Email = email,
                Rol = role,
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            var created = await _userRepository.AddAsync(user);

            return Mapper.Map<UserSelectDto>(created);
        }

        public override async Task<UserSelectDto?> UpdateAsync(int id, UserUpdateDto dto)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser mayor que cero.");
            }

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto), "Los datos del usuario son obligatorios.");
            }

            ValidateUser(dto.Nombre, dto.Email, dto.Rol);

            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return null;
            }

            var email = NormalizeEmail(dto.Email);
            var role = NormalizeRole(dto.Rol);

            var existingUser = await _userRepository.GetByEmailAsync(email);

            if (existingUser != null && existingUser.Id != id)
            {
                throw new InvalidOperationException("Ya existe otro usuario registrado con este correo electrónico.");
            }

            user.Nombre = dto.Nombre.Trim();
            user.Email = email;
            user.Rol = role;
            user.Activo = dto.Activo;
            user.FechaActualizacion = DateTime.Now;

            var updated = await _userRepository.UpdateAsync(user);

            return Mapper.Map<UserSelectDto>(updated);
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID debe ser mayor que cero.");
            }

            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return false;
            }

            user.Activo = false;
            user.FechaActualizacion = DateTime.Now;

            await _userRepository.UpdateAsync(user);

            return true;
        }

        private static void ValidateUser(string nombre, string email, string rol)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ArgumentException("El nombre del usuario es obligatorio.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("El correo electrónico es obligatorio.");
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                throw new ArgumentException("El correo electrónico no tiene un formato válido.");
            }

            if (string.IsNullOrWhiteSpace(rol))
            {
                throw new ArgumentException("El rol del usuario es obligatorio.");
            }

            ValidateRole(rol);
        }

        private static void ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("La contraseña es obligatoria.");
            }

            if (password.Length < 6)
            {
                throw new ArgumentException("La contraseña debe tener mínimo 6 caracteres.");
            }
        }

        private static void ValidateRole(string rol)
        {
            var validRoles = new[] { "Admin", "Employee", "Viewer" };

            if (!validRoles.Contains(rol, StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException("El rol no es válido. Roles permitidos: Admin, Employee, Viewer.");
            }
        }

        private static string NormalizeEmail(string email)
        {
            return email.Trim().ToLower();
        }

        private static string NormalizeRole(string rol)
        {
            return rol.Trim().ToLower() switch
            {
                "admin" => "Admin",
                "employee" => "Employee",
                "viewer" => "Viewer",
                _ => "Viewer"
            };
        }
    }
}