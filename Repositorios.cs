namespace ReservaSalas.Core
{
    public sealed class RepositorioReservas : IReservaSubject
    {
        // Singleton
        private static readonly Lazy<RepositorioReservas> _instancia = new(() => new RepositorioReservas());
        public static RepositorioReservas Instancia => _instancia.Value;    
    }
}//preciso terminar a implementação do repositório de reservas, que é onde as reservas serão armazenadas e gerenciadas. Ele também implementa o padrão Observer para notificar os usuários sobre mudanças nas reservas.