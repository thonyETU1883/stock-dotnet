using Npgsql;
using System.Data;
namespace stock.Models;
class Article{

    String Idarticle ;
    String Nomarticle;
    int Type;

    public Article(){}

    public Article(String idarticle,String nomarticle,int type){
        this.Idarticle = idarticle;
        this.Nomarticle = nomarticle;
        this.Type = type;
    }

    public String getIdarticle(){
        return this.Idarticle;
    } 

    public void setIdarticle(String Idarticle){
        this.Idarticle = Idarticle;
    }

    public String getNomarticle(){
        return this.Nomarticle;
    }

    public void setNomarticle(String nomarticle){
        this.Nomarticle = nomarticle;
    }

    public int getType(){
        return this.Type;
    }

    public void setType(int type){
        this.Type = type;
    }

    public Etatdestock getetatdestockbydate(String date,String idmagasin,NpgsqlConnection liaisonbase){
        String sql = "select * from etatdestock where date <= @date AND idarticle = @idarticle  AND idmagasin = @idmagasin LIMIT 1";
        if(liaisonbase == null || liaisonbase.State == ConnectionState.Closed){
            Connexion connexion = new Connexion ();
            liaisonbase = connexion.createLiaisonBase();
            liaisonbase.Open();
        }

        Etatdestock etatdestock = new Etatdestock();

        try{
            NpgsqlCommand cmd = new NpgsqlCommand(sql, liaisonbase);
            cmd.Parameters.AddWithValue("@date", DateTime.Parse(date));
            cmd.Parameters.AddWithValue("@idarticle", this.getIdarticle());
            cmd.Parameters.AddWithValue("@idmagasin", idmagasin);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            if(reader.Read()){
                etatdestock.setIdmouvement(reader.GetString(0));
                etatdestock.setDate(reader.GetDateTime(reader.GetOrdinal("date")));
                etatdestock.setQuantite(reader.GetDouble(2));
                etatdestock.setIdarticle(reader.GetString(3));
                etatdestock.setIdmagasin(reader.GetString(4));
            }
        }catch(Exception e){
            Console.WriteLine(e.Message);
        }finally{
            if(liaisonbase != null){
                liaisonbase.Close();
            }
        }
        return etatdestock;
    }

    public void getArticleById(String idarticle,NpgsqlConnection liaisonbase){
        String sql = "SELECT * FROM article WHERE idarticle = @idarticle";

        if(liaisonbase == null || liaisonbase.State == ConnectionState.Closed){
            Connexion connexion = new Connexion ();
            liaisonbase = connexion.createLiaisonBase();
            liaisonbase.Open();
        }

        try{
            NpgsqlCommand cmd = new NpgsqlCommand(sql, liaisonbase);
            cmd.Parameters.AddWithValue("@idarticle",idarticle);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read()){
                this.setIdarticle(reader.GetString(0));
                this.setNomarticle(reader.GetString(1));
                this.setType(reader.GetInt32("type"));
            }

        }catch(Exception e){
            Console.WriteLine(e.Message);
        }finally{
            if(liaisonbase != null){
                liaisonbase.Close();
            }
        }
    }

    public Mouvement getlastmouvement(NpgsqlConnection liaisonbase,String idmagasin){
        String sql = "SELECT * FROM etatdestock WHERE idarticle = @idarticle AND idmagasin = @idmagasin ORDER BY date DESC LIMIT 1";

        if(liaisonbase == null || liaisonbase.State == ConnectionState.Closed){
            Connexion connexion = new Connexion ();
            liaisonbase = connexion.createLiaisonBase();
            liaisonbase.Open();
        }
        Mouvement mouvement = new Mouvement();
        try{
            NpgsqlCommand cmd = new NpgsqlCommand(sql, liaisonbase);
            cmd.Parameters.AddWithValue("@idarticle",this.getIdarticle());
            cmd.Parameters.AddWithValue("@idmagasin",idmagasin);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read()){
                mouvement.setIdmouvement(reader.GetString(0));
                mouvement.setDate(reader.GetDateTime(reader.GetOrdinal("date")));
                mouvement.setQuantite(reader.GetDouble(2));
                mouvement.setIdarticle(reader.GetString(3));
                mouvement.setIdmagasin(reader.GetString(4));
            }   
        }catch(Exception e){
            Console.WriteLine(e.Message);
        }finally{
            if(liaisonbase != null){
                liaisonbase.Close();
            }
        }
        return mouvement;
    }
}