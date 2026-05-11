namespace ReservaSalas.core
{
    

    public class reserve
    {
        public int Id { get; private set; }
        public int SalaId { get; private set; }
        public int UsuarioId { get; private set; }
        public DateTime Data { get; private set; }
        public TimeSpan Inicio { get; private set; }
        public TimeSpan Fim { get; private set; }
        public bool Ativa { get; private set; } = true;

        // Serviços adicionais opcionais Decorator
        private readonly List<string> _servicosExtras = new();
        public IReadOnlyList<string> ServicosExtras => _servicosExtras;

        public Reserva(int id, int salaId, int usuarioId, DateTime data, TimeSpan inicio, TimeSpan fim)
        {
            if (fim <= inicio)
                throw new ArgumentException("Horário de fim deve ser posterior ao início.");

            Id = id;
            SalaId = salaId;
            UsuarioId = usuarioId;
            Data = data;
            Inicio = inicio;
            Fim = fim;
        }

        public void Cancelar() => Ativa = false;

        public void Modificar(DateTime novaData, TimeSpan novoInicio, TimeSpan novoFim)
        {
            if (novoFim <= novoInicio)
                throw new ArgumentException("Horário de fim deve ser posterior ao início.");
            Data = novaData;
            Inicio = novoInicio;
            Fim = novoFim;
        }

        public bool ColideComOutra(Reserva outra)
        {
            if (outra.SalaId != SalaId || outra.Data.Date != Data.Date)
                return false;
            return Inicio < outra.Fim && Fim > outra.Inicio;
        }

    }
}