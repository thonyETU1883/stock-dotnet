using Npgsql;
using System.Data;
namespace stock.Models;

class Mouvement{
    String Idmouvement;
    DateTime Date;
    double Quantite;
    String Idarticle;
    String Idmagasin;
    double Prixunitaire;

    public Mouvement(){}

    public Mouvement(String idmouvement,DateTime date,double quantite,String idarticle,String idmagasin,double prixunitaire){
        this.Idmouvement  = idmouvement;
        this.Date = date;
        this.Quantite = quantite;
        this.Idarticle = idarticle;
        this.Idmagasin = idmagasin;
        this.Prixunitaire = prixunitaire;
    }

    public Mouvement(DateTime date,double quantite,String idarticle,String idmagasin,double prixunitaire){
        this.Date = date;
        this.Quantite = quantite;
        this.Idarticle = idarticle;
        this.Idmagasin = idmagasin;
        this.Prixunitaire = prixunitaire;
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

    public double getPrixunitaire(){
        return this.Prixunitaire;
    }

    public void setPrixunitaire(double prixunitaire){
        this.Prixunitaire = prixunitaire;
    }

    public void insertionsortie(NpgsqlConnection liaisonbase){
        
        String sql = "INSERT INTO sortie(date,quantite,idarticle,idmagasin,etat,prixunitaire) VALUES (@date,@quantite,@idarticle,@idmagasin,0,@prixunitaire) RETURNING idsortie"; 
        
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
            cmd.Parameters.AddWithValue("@prixunitaire", this.getPrixunitaire());
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
            sql = "SELECT * FROM mouvement_reste WHERE idarticle = @idarticle AND idmagasin = @idmagasin AND date <= @date ORDER BY date ASC";
        }else if(article.getType()==2){
            sql = "SELECT * FROM mouvement_reste WHERE idarticle = @idarticle AND idmagasin = @idmagasin AND date <= @date ORDER BY date DESC";
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
                reste = reader.GetDouble(6);
                if(reste <= quantite){
                    quantite = quantite - reste;
                    Mouvement mouvement = new Mouvement(reader.GetString(0),maintenant,reste,this.getIdarticle(),this.getIdmagasin(),reader.GetDouble(5));
                    listemouvement.Add(mouvement);
                }else{
                    Mouvement mouvement = new Mouvement(reader.GetString(0),maintenant,quantite,this.getIdarticle(),this.getIdmagasin(),reader.GetDouble(5));
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

    public bool controledate(NpgsqlConnection liaisonbase){
        Article article = new Article();
        article.getArticleById(this.getIdarticle(),liaisonbase);
        Mouvement lastmouvement = article.getlastmouvement(liaisonbase,this.getIdmagasin());
        if(this.getDate() < lastmouvement.getDate()){
            return false;
        }
        return true;
    }

    public bool controlestock(NpgsqlConnection liaisonbase){
        Article article = new Article();
        article.getArticleById(this.getIdarticle(),liaisonbase);
        Mouvement lastmouvement = article.getlastmouvement(liaisonbase,this.getIdmagasin());
        if(this.getQuantite() > lastmouvement.getQuantite()){
            return false;
        }
        return true;
    }

    public void changeetatsortie(NpgsqlConnection liaisonbase){
        String sql = "UPDATE sortie SET etat = 1 WHERE idsortie = @idsortie";

        if(liaisonbase == null || liaisonbase.State == ConnectionState.Closed){
            Connexion connexion = new Connexion ();
            liaisonbase = connexion.createLiaisonBase();
            liaisonbase.Open();
        }

        try{
            NpgsqlCommand cmd = new NpgsqlCommand(sql, liaisonbase);
            cmd.Parameters.AddWithValue("@idsortie",this.getIdmouvement());
            cmd.ExecuteNonQuery();
            Console.WriteLine("update : "+this.getIdmouvement());
        }catch(Exception e){
            Console.WriteLine(e.Message);
        }finally{
            if(liaisonbase != null){
                liaisonbase.Close();
            }
        }
    } 

    public void insertionmouvement(String idsortie,NpgsqlConnection liaisonbase){
        String sql = "INSERT INTO mouvement(date,idsortie,identree,quantite) VALUES (@date,@idsortie,@identree,@quantite)";
        
        if(liaisonbase == null || liaisonbase.State == ConnectionState.Closed){
            Connexion connexion = new Connexion ();
            liaisonbase = connexion.createLiaisonBase();
            liaisonbase.Open();
        }

        try{
            NpgsqlCommand cmd = new NpgsqlCommand(sql, liaisonbase);
            cmd.Parameters.AddWithValue("@date",this.getDate());
            cmd.Parameters.AddWithValue("@idsortie",idsortie);
            cmd.Parameters.AddWithValue("@identree",this.getIdmouvement());
            cmd.Parameters.AddWithValue("@quantite",this.getQuantite());
            cmd.ExecuteNonQuery();
            Console.WriteLine("insertion mouvement terminer");
        }catch(Exception e){
            Console.WriteLine(e.Message);
        }finally{
            if(liaisonbase != null){
                liaisonbase.Close();
            }
        }
    }  

    public void validationsortie(NpgsqlConnection liaisonbase){
        if(this.controledate(liaisonbase) == false){
            throw new Exceptionpersonnalise("date incorrecte");
        }else if(this.controlestock(liaisonbase) == false){
            throw new Exceptionpersonnalise("quantite insuffisant");
        }
        this.changeetatsortie(liaisonbase);
        List<Mouvement> listedecompose = this.decomposer(liaisonbase);
        foreach(Mouvement mouvement in listedecompose){
            mouvement.insertionmouvement(this.getIdmouvement(),liaisonbase);
        }
        Console.WriteLine("validation terminer");
    }

    public List<Mouvement> getallmouvementnovalider(NpgsqlConnection liaisonbase){
        String sql = "SELECT * FROM sortie WHERE etat=0";
        
        if(liaisonbase == null || liaisonbase.State == ConnectionState.Closed){
            Connexion connexion = new Connexion ();
            liaisonbase = connexion.createLiaisonBase();
            liaisonbase.Open();
        }
        List<Mouvement> listemouvement= new List<Mouvement>();
        try{
            NpgsqlCommand cmd = new NpgsqlCommand(sql, liaisonbase);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read()){
                Mouvement mouvement =  new Mouvement(reader.GetString(0),reader.GetDateTime(reader.GetOrdinal("date")),reader.GetDouble(2),reader.GetString(3),reader.GetString(4),reader.GetDouble(6));
                listemouvement.Add(mouvement);
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

    public void getsortiebyid(String idmouvement,NpgsqlConnection liaisonbase){
        String sql = "SELECT * FROM sortie WHERE idsortie = @idsortie";
         if(liaisonbase == null || liaisonbase.State == ConnectionState.Closed){
            Connexion connexion = new Connexion ();
            liaisonbase = connexion.createLiaisonBase();
            liaisonbase.Open();
        }
        try{
            NpgsqlCommand cmd = new NpgsqlCommand(sql, liaisonbase);
            cmd.Parameters.AddWithValue("@idsortie",idmouvement);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read()){
                this.setIdmouvement(reader.GetString(0));
                this.setDate(reader.GetDateTime(reader.GetOrdinal("date")));
                this.setQuantite(reader.GetDouble(2));
                this.setIdarticle(reader.GetString(3));
                this.setIdmagasin(reader.GetString(4));
                this.setPrixunitaire(reader.GetDouble(6));
            }
        }catch(Exception e){
            Console.WriteLine(e.Message);
        }finally{
            if(liaisonbase != null){
                liaisonbase.Close();
            }
        }
    } 

 
}
