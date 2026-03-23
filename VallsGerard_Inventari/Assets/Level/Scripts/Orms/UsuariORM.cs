using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class UsuariORM : MonoBehaviour
{
    public int Id { get; set; }
    public string Usuari { get; set; }
    public string Password { get; set; }

    public UsuariORM() { }

    public UsuariORM(int id, string usuari, string password)
    {
        Id = id;
        Usuari = usuari;
        Password = password;
    }


    //Retorna tots els usuaris de la BD
    public static List<UsuariORM> GetAll(IDbConnection conn)
    {
        var llista = new List<UsuariORM>();

        EnsureOpen(conn);
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT id, usuario, password FROM Usuarios";
            using (IDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    llista.Add(new UsuariORM(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2)
                    ));
                }
            }
        }
        return llista;
    }

    public static UsuariORM FindByCredentials(IDbConnection conn, string usuari, string password)
    {
        EnsureOpen(conn);
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText =
                "SELECT id, usuario, password FROM Usuarios " +
                "WHERE usuario = @u AND password = @p LIMIT 1";

            AddParam(cmd, "@u", usuari);
            AddParam(cmd, "@p", password);

            using (IDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                    return new UsuariORM(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
            }
        }
        return null;
    }

    public static bool Insert(IDbConnection conn, string usuari, string password)
    {
        if (password.Length < 8)
        {
            Debug.LogWarning("ORM: La contrasenya ha de tenir mínim 8 caràcters.");
            return false;
        }

        EnsureOpen(conn);
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText =
                "INSERT INTO Usuarios (usuario, password) VALUES (@u, @p)";

            AddParam(cmd, "@u", usuari);
            AddParam(cmd, "@p", password);

            int files = cmd.ExecuteNonQuery();
            return files > 0;
        }
    }

    public static bool DeleteById(IDbConnection conn, int id)
    {
        EnsureOpen(conn);
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "DELETE FROM Usuarios WHERE id = @id";
            AddParam(cmd, "@id", id);
            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public static bool DeleteByName(IDbConnection conn, string usuari)
    {
        EnsureOpen(conn);
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "DELETE FROM Usuarios WHERE usuario = @u";
            AddParam(cmd, "@u", usuari);
            return cmd.ExecuteNonQuery() > 0;
        }
    }

    public static void UpdateWallet(IDbConnection connexio, string usuari, float wallet)
    {
        using var cmd = connexio.CreateCommand();
        cmd.CommandText = "UPDATE Usuarios SET wallet = @wallet WHERE usuario = @usuari"; //same
        var p1 = cmd.CreateParameter(); p1.ParameterName = "@wallet"; p1.Value = wallet; cmd.Parameters.Add(p1);
        var p2 = cmd.CreateParameter(); p2.ParameterName = "@usuari"; p2.Value = usuari; cmd.Parameters.Add(p2);
        cmd.ExecuteNonQuery();
    }

    public static float? GetWallet(IDbConnection connexio, string usuari)
    {
        using var cmd = connexio.CreateCommand();
        cmd.CommandText = "SELECT wallet FROM Usuarios WHERE usuario = @usuari"; //coandos o com li vulgis dir de la bd
        var p1 = cmd.CreateParameter(); p1.ParameterName = "@usuari"; p1.Value = usuari; cmd.Parameters.Add(p1);
        var result = cmd.ExecuteScalar();
        return result != null ? Convert.ToSingle(result) : (float?)null;
    }

    private static void EnsureOpen(IDbConnection conn)
    {
        if (conn.State != ConnectionState.Open)
            conn.Open();
    }

    private static void AddParam(IDbCommand cmd, string name, object value)
    {
        var param = cmd.CreateParameter();
        param.ParameterName = name;
        param.Value = value;
        cmd.Parameters.Add(param);
    }
}
