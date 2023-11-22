using Npgsql;
using System.Data;
namespace stock.Models;

class Mouvement{
    String Idmouvement;
    DateTime Date;
    double Quantite;
    String Idarticle;
    String Idmagasin;

    public Mouvement(){}

    public Mouvement(String idmouvement,DateTime date,double quantite,String idarticle,String idmagasin){
        this.Idmouvement  = idmouvement;
        this.Date = date;
        this.Quantite = quantite;
        this.Idarticle = idarticle;
        this.Idmagasin = idmagasin;
    }

    public Mouvement(DateTime date,double quantite,String idarticle,String idmagasin){
        this.Date = date;
        this.Quantite = quantite;
        this.Idarticle = idarticle;
        this.Idmagasin = idmagasin;
    }


    public String getIdmouvement(){
        return this.Idmouvement;
    }

    public void setIdmouvement(String idmouvement){
        this.Idmouvement = idmouvement;
    }

    public DateTime getDate(){
        return this.Date;
    }

    public void setDate(DateTime date){
        this.Date = date;
    }

    public double getQuantite(){
        return this.Quantite;
    }

    public void setQuantite(double quantite){
        this.Quantite = quantite;
    }

    public String getIdarticle(){
        return this.Idarticle;
    } 

    public void setIdarticle(String Idarticle){
        this.Idarticle = Idarticle;
    }

    public String getIdmagasin(){
        return this.Idmagasin;
    }

    public void setIdmagasin(String idmagasin){
        this.Idmagasin = idmagasin;
    }

    public void insertionsortie(NpgsqlConnection liaisonbase){
        
        String sql = "INSERT INTO sortie(date,quantite,idarticle,idmagasin,etat) VALUES (@date,@quantite,@idarticle,@idmagasin,0) RETURNING idsortie"; 
        
        if(liaisonbase == null || liaisonbase.State == ConnectionState.Closed){
            Connexion connexion = new Connexion ();
            liaisonbase = connexion.createLiaisonBase();
            liaisonbase.Open();
        }

        try{
            NpgsqlCommand cmd = new NpgsqlCommand(sql, liaisonbase);
            cmd.Parameters.AddWithValue("@date",this.getDate());
            cmd.Parameters.AddWithValue("@quantite", this.getQuantite());
            cmd.Parameters.AddWithValue("@idarticle", this.getIdarticle());
            cmd.Parameters.AddWithValue("@idmagasin", this.getIdmagasin());
            String insertedId = cmd.ExecuteScalar().ToString();
            this.setIdmouvement(insertedId);
        }catch(Exception e){
            Console.WriteLine(e.Message);
        }finally{
            if(liaisonbase != null){
                liaisonbase.Close();
            }
        }

    }

    public List<Mouvement> decomposer(NpgsqlConnection liaisonbase){
        List<Mouvement> listemouvement =  new List<Mouvement>();
        Article article =  new Article();
        article.getArticleById(this.getIdarticle(),liaisonbase);
        String sql = "";
        DateTime maintenant = DateTime.Now;
        
        if(article.getType()==1){
            sql = "SELECT * FROM mouvement_reste WHERE idarticle = @idarticle AND idmagasin = @idmagasin AND date <= @date ORDER BY date DESC";
        }else if(article.getType()==2){
            sql = "SELECT * FROM mouvement_reste WHERE idarticle = @idarticle AND idmagasin = @idmagasin AND date <= @date ORDER BY date ASC";
        }

        if(liaisonbase == null || liaisonbase.State == ConnectionState.Closed){
            Connexion connexion = new Connexion ();
            liaisonbase = connexion.createLiaisonBase();
            liaisonbase.Open();
        }
        try{
            NpgsqlCommand cmd = new NpgsqlCommand(sql, liaisonbase);
            cmd.Parameters.AddWithValue("@idarticle",this.getIdarticle());
            cmd.Parameters.AddWithValue("@idmagasin",this.getIdmagasin());
            cmd.Parameters.AddWithValue("@date",this.getDate());
            NpgsqlDataReader reader = cmd.ExecuteReader();
            double reste = 0;
            double quantite = this.getQuantite();
            while(reader.Read() && quantite > 0){
                reste = reader.GetDouble(5);
                if(reste <= quantite){
                    quantite = quantite - reste;
                    Mouvement mouvement = new Mouvement(maintenant,reste,this.getIdarticle(),this.getIdmagasin());
                    listemouvement.Add(mouvement);
                }else{
                    Mouvement mouvement = new Mouvement(maintenant,quantite,this.getIdarticle(),this.getIdmagasin());
                    listemouvement.Add(mouvement);
                    quantite = 0;
                }
            }
        }catch(Exception e){
            Console.WriteLine(e.Message);
        }finally{
            if(liaisonbase != null){
                liaisonbase.Close();
            }
        }

        return listemouvement;
    }
}
