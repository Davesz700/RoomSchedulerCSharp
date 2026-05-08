public interface user{//Vai ter que adcionar observadores para notificar os usuários sobre as reservas
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Education_Role { get; set;}//Se é aluno ou professor
}

public class student : user{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string EducationRole { get; set; } = "student"; // Valor padrão

    // Propriedades da Classe
    public string Course { get; set; }
    public int Year { get; set; }
}

public class teacher : user{
   public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string EducationRole { get; set; } = "teacher"; // Valor padrão

    // Propriedades da Classe
    public string Department { get; set; }
    public string Title { get; set; }
}
