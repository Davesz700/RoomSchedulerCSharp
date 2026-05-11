public interface user{//Vai ter que adcionar observadores para notificar os usuários sobre as reservas
    int Id { get; }
        string Nome { get; }
        string Email { get; }
        string EducationRole { get; }   // "student" | "teacher"
        bool PodeReservar { get; }
}

public class student : user{
     public int Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Curso { get; private set; }
        public int Semestre { get; private set; }
        public string EducationRole => "student";
        // Estudantes podem reservar, mas com menor prioridade (usado pelo Strategy)
        public bool PodeReservar => true;

        private string _senhaHash;

        public Estudante(int id, string nome, string email, string senhaHash, string curso, int semestre)
        {
            Id = id;
            Nome = nome;
            Email = email;
            _senhaHash = senhaHash;
            Curso = curso;
            Semestre = semestre;
        }

        public override string ToString() => $"Estudante [{Id}] {Nome} ({Curso}, {Semestre}º semestre)";
}

public class teacher : user{
    public int Id { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string EducationRole => "teacher";
    public bool PodeReservar => true;
    public string Titulo { get; private set; }
    private string _senhaHash;

    public Professor(int id, string nome, string email, string senhaHash, string departamento, string titulo)
    {
        Id = id;
        Nome = nome;
        Email = email;
        _senhaHash = senhaHash;
        Department = departamento;
        Title = titulo;
    }

    public override string ToString() => $"Professor [{Id}] {Nome} ({Department}, {Title})";
}
