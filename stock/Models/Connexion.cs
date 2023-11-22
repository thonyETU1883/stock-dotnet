using Npgsql;

namespace stock.Models;
class Connexion{
     public NpgsqlConnection createLiaisonBase(){
        string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=0000;Database=stock;";
        try{
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            return connection;
        }catch(Exception ex){
            Console.WriteLine($"Erreur lors de la connexion à PostgreSQL : {ex.Message}");
        }
        return null;
     }
}