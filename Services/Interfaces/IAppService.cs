namespace MyProject.Services.Interfaces
{
    // Interface base para todos os serviços de aplicação
    public interface IAppService
    {
    }

    // Definição única de ServiceResult e ServiceResult<T>
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
