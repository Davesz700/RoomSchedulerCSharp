namespace ReservaSalas.Core
{
        public interface IPoliticaReserva
    {
        string Nome { get; }

        string ? VerificarConflito(Reserva novaReserva, IEnumerable<Reserva> reservasExistentes);
    }

    public class Politica_Primeiro_a_Reservar: IPoliticaReserva
    {
        public string Nome => "Primeiro a Reservar";

        public string ? VerificarConflito(Reserva novaReserva, IEnumerable<Reserva> reservasExistentes)
        {
            // Se houver alguma reserva ativa para o mesmo horário, bloqueia a nova reserva
            bool existeConflito = reservasExistentes.Any(r =>
                r.Ativa &&
                r.Data.Date == novaReserva.Data.Date &&
                ((novaReserva.Inicio < r.Fim) && (novaReserva.Fim > r.Inicio))
            );

            return existeConflito ? "Conflito: Já existe uma reserva ativa para este horário." : null;
        }

    }

    public class Politica_Professor_Prioritario : IPoliticaReserva
    {
        public string Nome => "Professor Prioritário";

        public string? VerificarConflito(Reserva nova, IEnumerable<Reserva> existentes, IUsuario solicitante)
        {
            var repo = RepositorioReservas.Instancia;
            var usuariosRepo = RepositorioUsuarios.Instancia;

            var conflitos = existentes
                .Where(r => r.Ativa && r.Id != nova.Id && nova.ColideComOutra(r))
                .ToList();

            if (!conflitos.Any()) return null;

            if (solicitante.EducationRole == "teacher")
            {
                // Professor pode cancelar reservas de estudantes em conflito
                var reservasDeEstudantes = conflitos
                    .Where(c =>
                    {
                        var u = usuariosRepo.BuscarPorId(c.UsuarioId);
                        return u?.EducationRole == "student";
                    })
                    .ToList();

                var reservasDeDocentes = conflitos.Except(reservasDeEstudantes).ToList();

                if (reservasDeDocentes.Any())
                {
                    var ids = string.Join(", #", reservasDeDocentes.Select(r => r.Id));
                    return $"Conflito com reserva(s) de outro(s) docente(s): #{ids}.";
                }

                // Cancela reservas de estudantes automaticamente e registra motivo
                foreach (var c in reservasDeEstudantes)
                    c.Cancelar();

                Console.WriteLine($"[Política] Reservas de estudantes canceladas por prioridade docente: "
                    + string.Join(", #", reservasDeEstudantes.Select(r => $"#{r.Id}")));

                return null; // professor pode prosseguir
            }

            // Estudante não tem prioridade — retorna conflito normalmente
            var primeiroConflito = conflitos.First();
            return $"Conflito de horário com reserva #{primeiroConflito.Id} "
                 + $"({primeiroConflito.Inicio:hh\\:mm}-{primeiroConflito.Fim:hh\\:mm}).";
        }
    }

    //Ainda falta ter que implementar Possibilite troca em tempo de execução.
}