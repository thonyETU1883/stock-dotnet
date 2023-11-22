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
}