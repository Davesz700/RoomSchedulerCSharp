public interface user{//Vai ter que adcionar observadores para notificar os usuários sobre as reservas
    private string Name { get; set; }
    private string Email { get; set; }
    private string Password { get; set; }
    private string Education_Role { get; set; }//Se é aluno ou professor
    private int Id { get; set; }
}

public class student : user{
    private string Name { get; set; }
    private string Email { get; set; }
    private string Password { get; set; }
    private string Education_Role { get; set; } = "student"; // Valor padrão
    private int Id { get; set; }

    // Propriedades da Classe
    private string Course { get; set; }
    private int Year { get; set; }
}

public class teacher : user{
    private string Name { get; set; }
    private string Email { get; set; }
    private string Password { get; set; }
    private string Education_Role { get; set; } = "teacher"; // Valor padrão

    private int Id { get; set; }
    // Propriedades da Classe
    private string Department { get; set; }
    private string Title { get; set; }
}
