using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using System.IO;

public class DBContent : MonoBehaviour
{
    
    public static DBContent Instance { get; private set; }

    
    [Header("Configuració")]
    public string nombreDB = "MyDatabase.sqlite";
    public bool borrarDatosAlIniciar = false;

    public IDbConnection Connexio { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InicialitzarBD();
    }

    void OnDestroy()
    {
        Connexio?.Close();
        Connexio?.Dispose();
    }

    private void InicialitzarBD()
    {
        string dbPath = Application.persistentDataPath + "/" + nombreDB;

        if (borrarDatosAlIniciar && File.Exists(dbPath))
        {
            File.Delete(dbPath);
            Debug.Log("DBContext: BD anterior esborrada.");
        }

        if (!File.Exists(dbPath))
        {
            File.Create(dbPath).Close();
            Debug.Log("DBContext: BD creada a: " + dbPath);
        }

        string dbUri = "URI=file:" + dbPath;
        Connexio = new SqliteConnection(dbUri);
        Connexio.Open();

        CrearTaules();

        Debug.Log("DBContext: Connexió oberta i taules llestes.");
    }

    private void CrearTaules()
    {
        using (var cmd = Connexio.CreateCommand())
        {
            cmd.CommandText =
                "CREATE TABLE IF NOT EXISTS Usuarios (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "usuario TEXT UNIQUE, " +
                "password TEXT CHECK (length(password) >= 8))";
            cmd.ExecuteNonQuery();
        }

        if (borrarDatosAlIniciar)
        {
            using (var cmd = Connexio.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Usuarios";
                cmd.ExecuteNonQuery();
            }
            Debug.Log("DBContext: Tots els usuaris han estat esborrats.");
        }
    }
}
