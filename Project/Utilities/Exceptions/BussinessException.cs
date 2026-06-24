namespace Utilities.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class EntityNotFoundException : BusinessException
    {
        public string EntityType { get; } = string.Empty;
        public object? EntityId { get; }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string entityType, object entityId)
            : base($"La entidad '{entityType}' con ID '{entityId}' no fue encontrada.")
        {
            EntityType = entityType;
            EntityId = entityId;
        }

        public EntityNotFoundException(string entityType, object entityId, Exception innerException)
            : base($"La entidad '{entityType}' con ID '{entityId}' no fue encontrada.", innerException)
        {
            EntityType = entityType;
            EntityId = entityId;
        }
    }

    public class ValidationException : BusinessException
    {
        public string PropertyName { get; } = string.Empty;

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string propertyName, string message)
            : base($"Error de validación en '{propertyName}': {message}")
        {
            PropertyName = propertyName;
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class BusinessRuleViolationException : BusinessException
    {
        public string RuleCode { get; } = string.Empty;

        public BusinessRuleViolationException(string message) : base(message)
        {
        }

        public BusinessRuleViolationException(string ruleCode, string message)
            : base($"Violación de regla de negocio [{ruleCode}]: {message}")
        {
            RuleCode = ruleCode;
        }

        public BusinessRuleViolationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class ExternalServiceException : BusinessException
    {
        public string ServiceName { get; } = string.Empty;

        public ExternalServiceException(string message) : base(message)
        {
        }

        public ExternalServiceException(string serviceName, string message)
            : base($"Error en el servicio externo '{serviceName}': {message}")
        {
            ServiceName = serviceName;
        }

        public ExternalServiceException(string serviceName, string message, Exception innerException)
            : base($"Error en el servicio externo '{serviceName}': {message}", innerException)
        {
            ServiceName = serviceName;
        }
    }
}