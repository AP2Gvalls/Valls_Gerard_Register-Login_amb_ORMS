using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using System.IO;

[DefaultExecutionOrder(-100)]
public class CrearBaseDatos : MonoBehaviour
{
    [Header("Configuración")]
    public bool borrarDatosAlIniciar = false; 

    private void Start()
    {
        CreateDatabase();
    }

    private void CreateDatabase()
    {
        string dbPath = Application.persistentDataPath + "/MyDatabase.sqlite";
        Debug.Log(dbPath);

        if (borrarDatosAlIniciar && File.Exists(dbPath))
        {
            File.Delete(dbPath);
            Debug.Log("Base de datos anterior borrada");
        }

        if (!File.Exists(dbPath))
        {
            File.Create(dbPath).Close();
            Debug.Log("Base de datos creada en: " + dbPath);
        }

        string dbUri = "URI=file:" + dbPath;

        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();

        IDbCommand dbCommandCreateTable = dbConnection.CreateCommand();
        dbCommandCreateTable.CommandText =
            "CREATE TABLE IF NOT EXISTS Usuarios (" +
            "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "usuario TEXT, " +
            "password TEXT, " +
            "wallet REAL DEFAULT 20" +
            ")";

        dbCommandCreateTable.ExecuteNonQuery();

        if (borrarDatosAlIniciar)
        {
            IDbCommand dbCommandDelete = dbConnection.CreateCommand();
            dbCommandDelete.CommandText = "DELETE FROM Usuarios";
            dbCommandDelete.ExecuteNonQuery();
            dbCommandDelete.Dispose();
            Debug.Log("Todos los usuarios han sido borrados");
        }

        dbCommandCreateTable.Dispose();
        dbConnection.Close();

        Debug.Log("Tabla Usuarios creada/actualizada correctamente");
    }
}