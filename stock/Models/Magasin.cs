using Npgsql;
using System.Data;
namespace stock.Models;

class Magasin{

    String Idmagasin;
    String Nommagasin;
    String Lieu;

    public Magasin(){}
    
    public Magasin(String idmagasin,String nommagasin,String lieu){
        this.Idmagasin = idmagasin;
        this.Nommagasin = nommagasin;
        this.Lieu = lieu; 
    }

    public String getIdmagasin(){
        return this.Idmagasin;
    }

    public void setIdmagasin(String idmagasin){
        this.Idmagasin = idmagasin;
    }

    public String getNommagasin(){
        return this.Nommagasin;
    }

    public void setNommagasin(String nommagasin){
        this.Nommagasin = nommagasin;
    }

    public String getLieu(){
        return this.Lieu;
    }

    public void setLieu(String lieu){
        this.Lieu = lieu; 
    } 

    public List<Etatdestock> getetatdestockbyidarticle(String idarticle,String date,NpgsqlConnection liaisonbase){
        Article article = new Article();
        article.getArticleById(idarticle,liaisonbase);
        List<Article> listearticle =  article.getcategoriearticle(liaisonbase);
        List<Etatdestock> listeetatdestock = new List<Etatdestock>();
        foreach(Article a in listearticle){
            Etatdestock e = a.getetatdestockbydate(date,this.getIdmagasin(),liaisonbase);
            if(e.getIdmouvement()==null){
                e.setIdarticle(article.getIdarticle());
                e.setIdmagasin(this.getIdmagasin());
                e.setDate(DateTime.Parse(date));
            }
            listeetatdestock.Add(e);
        }
        return listeetatdestock;
    }

    public double sommemontant(String idarticle,String date,NpgsqlConnection liaisonbase){
        List<Etatdestock> liste = this.getetatdestockbyidarticle(idarticle,date,liaisonbase);
        double montant =0;
        foreach(Etatdestock e in liste){
            montant = montant+e.getMontant();
        }
        return montant;
    }

    public List<Magasin> getallmagasin(NpgsqlConnection liaisonbase){
        String sql = "SELECT * FROM magasin";
        if(liaisonbase == null || liaisonbase.State == ConnectionState.Closed){
            Connexion connexion = new Connexion ();
            liaisonbase = connexion.createLiaisonBase();
            liaisonbase.Open();
        }
        List<Magasin> listemagasin = new List<Magasin>(); 
        try{
            NpgsqlCommand cmd = new NpgsqlCommand(sql, liaisonbase);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read()){
                Magasin magasin = new Magasin(reader.GetString(0),reader.GetString(1),reader.GetString(2));
                listemagasin.Add(magasin);
            }
        }catch(Exception e){
            Console.WriteLine(e.Message);
        }finally{
            if(liaisonbase != null){
                liaisonbase.Close();
            }
        }
        return listemagasin;
    }

    public void getmagasinbyid(String idmagasin,NpgsqlConnection liaisonbase){
        String sql = "SELECT * FROM magasin WHERE idmagasin = @idmagasin";
        if(liaisonbase == null || liaisonbase.State == ConnectionState.Closed){
            Connexion connexion = new Connexion ();
            liaisonbase = connexion.createLiaisonBase();
            liaisonbase.Open();
        }

        try{
            NpgsqlCommand cmd = new NpgsqlCommand(sql, liaisonbase);
            cmd.Parameters.AddWithValue("@idmagasin",idmagasin);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if(reader.Read()){
                this.setIdmagasin(reader.GetString(0));
                this.setNommagasin(reader.GetString(1));
                this.setLieu(reader.GetString(2));
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