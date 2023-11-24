using Npgsql;
using System.Data;
namespace stock.Models;

class Etatdestock{
    String Idmouvement;
    DateTime Date;
    double Quantite;
    String Idarticle;
    String Idmagasin;
    double Prixunitaire;

    public Etatdestock(){}

    public Etatdestock(String idmouvement,DateTime date,double quantite,String idarticle,String idmagasin,double prixuniatre){
        this.Idmouvement = idmouvement;
        this.Date = date;
        this.Quantite = quantite;
        this.Idarticle = idarticle;
        this.Idmagasin = idmagasin;
        this.Prixunitaire = prixuniatre;
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

   public double getMontant(){
        return this.getQuantite() * this.getPrixunitaire();
    }
}