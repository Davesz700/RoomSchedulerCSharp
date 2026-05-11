public enum ClassroomType
{
    Laboratory,
    StudyRoom,
    WorkgroupRoom,
}//Tipagem de sala de aula, para diferenciar os tipos de salas disponíveis



 public interface IClassroom
{
    int Id { get; }
        int Numero { get; }
        int Capacidade { get; }
        TipoSala Tipo { get; }
        bool EstaDisponivel(DateTime data, TimeSpan inicio, TimeSpan fim);
        string Descricao { get; }
    
}//interface para definir as propriedades e métodos que as salas de aula devem implementar

public abstract class Classroom : IClassroom
{
    public int Id { get; protected set; }
        public int Numero { get; protected set; }
        public int Capacidade { get; protected set; }
        public abstract TipoSala Tipo { get; }
        public abstract string Descricao { get; }

        // Delegamos a verificação ao repositório — a sala não conhece suas reservas diretamente
        public bool EstaDisponivel(DateTime data, TimeSpan inicio, TimeSpan fim)
        {
            var repo = RepositorioReservas.Instancia;
            return !repo.ExisteConflito(Id, data, inicio, fim, reservaIgnoradaId: null);
        }

        protected Classroom(int id, int numero, int capacidade)
        {
            Id = id;
            Numero = numero;
            Capacidade = capacidade;
        }

        public override string ToString() => $"{Descricao} #{Numero} (cap. {Capacidade})";
}//Utilizamos uma classe base para representar salas de aula

public class Laboratory : Classroom
{
    public override TipoSala Tipo => TipoSala.Laboratorio;
        public override string Descricao => "Laboratório";

        public Laboratorio(int id, int numero, int capacidade)
            : base(id, numero, capacidade) { }
}

public class StudyRoom : Classroom
{
    public override TipoSala Tipo => TipoSala.SalaEstudo;
    public override string Descricao => "Sala de Estudo";

        public SalaEstudo(int id, int numero, int capacidade)
            : base(id, numero, capacidade) { }
}

public class WorkgroupRoom : Classroom
{
     public override TipoSala Tipo => TipoSala.SalaTrabalhoGrupo;
        public override string Descricao => "Sala de Trabalho em Grupo";

        public SalaTrabalhoGrupo(int id, int numero, int capacidade)
            : base(id, numero, capacidade) { }
}



public class ClassroomFactory//Fábrica para criar instâncias de salas de aula com base no tipo especificado
{
    public abstract ISala CriarSala(int id, int numero, int capacidade);
    }

    public class LaboratorioFactory : SalaFactory
    {
        public override ISala CriarSala(int id, int numero, int capacidade)
            => new Laboratorio(id, numero, capacidade);
    }

    public class SalaEstudoFactory : SalaFactory
    {
        public override ISala CriarSala(int id, int numero, int capacidade)
            => new SalaEstudo(id, numero, capacidade);
    }

    public class SalaTrabalhoGrupoFactory : SalaFactory
    {
        public override ISala CriarSala(int id, int numero, int capacidade)
            => new SalaTrabalhoGrupo(id, numero, capacidade);
    }

    // Utilitário que mapeia enum → factory (mantém o switch em um único lugar)
    public static class SalaFactoryProvider
    {
        public static SalaFactory ObterFactory(TipoSala tipo) => tipo switch
        {
            TipoSala.Laboratorio        => new LaboratorioFactory(),
            TipoSala.SalaEstudo         => new SalaEstudoFactory(),
            TipoSala.SalaTrabalhoGrupo  => new SalaTrabalhoGrupoFactory(),
            _ => throw new ArgumentException($"Tipo de sala desconhecido: {tipo}")
        };
    }

