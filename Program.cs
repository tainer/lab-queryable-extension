// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using QueryableExtensions;

using (var context = new AppDbContext())
{
    context.Database.Migrate();

    var u = new User()
    {
        Id = Guid.NewGuid(),
        Email = "tainer@tainer.com.br",
        Name = "tainer"
    };
    context.Users.Add(u);
    context.SaveChanges();

    var userQuery = context.Users.AsQueryable();
    var simpleUserQuery = userQuery.Select<SimpleUser>();
    
    // Registro da SQL no console
    var sql = userQuery.ToQueryString();
    Console.WriteLine($"SQL UserQuery: {sql}");
    
    sql = simpleUserQuery.ToQueryString();
    Console.WriteLine($"SQL SimpleUserQuery: {sql}");
    
    var simpleUsers = simpleUserQuery.ToList();

    // Exibindo os resultados
    Console.WriteLine("SimpleUsers:");
    foreach (var user in simpleUsers)
    {
        Console.WriteLine($"Id: {user.Id}, Name: {user.Name}");
    }
}