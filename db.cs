using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.Linq;
using Clase1;
using UnityEngine.EventSystems;
using System.Threading;

/**
*Creado por Jose Manuel Rodriguez Altamirano
*
*/




public class db : MonoBehaviour {
    public int count;
    private IDbConnection dbconn;
    public IDataReader reader;
    private IDbCommand dbcmd;
    public GameObject Script;
    private string conectString;
    public string NPCText;
   
    public string textocorrutina;
    public string directorio;
    public float tiempoEspera = 5f;
    public Text textoInformativo;
    public int dato;
    string sqlQuery;
    public static int contador;

    public void LanzarRandom()
    {
        Myclase utils = new Myclase();      
        print(Myclase.GenerateRandom(0, 100));
    }
    
    //funciona desde dll 
  
   

    public void cierre()
    {
        reader.Close();
        reader = null;
        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;
    }
    IEnumerator Esperar()
    {        
        yield return new WaitForSeconds(tiempoEspera);
        if (dato < contador) {
            lectura(++dato + 1);
        }
        
    }

    //=================================================

   private void GetTable()
    {
        using(IDbConnection dbConnection = new SqliteConnection(conectString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = "SELECT * FROM Texto";
                dbCmd.CommandText = sqlQuery;
                using (IDataReader reader= dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int indice = reader.GetInt32(0);
                       // Debug.Log("Registro =  "+indice);
                        contador = indice;
                    }
                    
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }
    }

    //======================================================
    private void InsertTable( string registro)
    {
        using (IDbConnection dbConnection = new SqliteConnection(conectString))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                string sqlQuery = String.Format("INSERT * INTO Texto Where NPCText= "+ registro);
               // string sqlQuery = "INSERT INTO  Texto WHERE NPCText = " + (registro);
               // string sqlQuery = "INSERT INTO  Texto WHERE NPCText = " + (registro="holaaaaaaaaa");
                dbCmd.CommandText = sqlQuery;
                // dbCmd.ExecuteScalar();
                //dbConnection.Close();
              
                Debug.Log("Registro añadido con exito¡¡=  "+sqlQuery);
            }
        }
    }
    //===============================================
    void Start()
    {
        conectString = "URI=file:" + Application.dataPath + "/Plugins/Items.s3db";
        GetTable();
        
        Debug.Log("Registrocontador =  " + contador);
    }
    public  void lectura(int dato)
    {             
        string conn = "URI=file:" + Application.dataPath + "/Plugins/Items.s3db";

        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        IDbCommand dbcmd = dbconn.CreateCommand();
       
        string sqlQuery = "SELECT * FROM Texto WHERE idTexto = " + (dato);
        dbcmd.CommandText = sqlQuery;      
        
        dbcmd.CommandText = sqlQuery;
       
        IDataReader reader = dbcmd.ExecuteReader();
        reader.Read();

        




        int Id = reader.GetInt32(0);       
        string NPCText = reader.GetString(1); 

        textoInformativo.text = NPCText;
        Debug.Log("Valor de contador= "+contador );
        Debug.Log("Valor de dato= "+dato);
        if (dato < contador)
        {
            StartCoroutine("Esperar");
        }
        if (dato==contador)
        {

            InsertTable("1");
        }
            
        
         //LanzarRandom();    


    }
  
}