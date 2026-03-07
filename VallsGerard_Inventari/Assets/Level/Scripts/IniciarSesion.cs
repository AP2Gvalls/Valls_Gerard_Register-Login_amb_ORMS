using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IniciarSesion : MonoBehaviour
{
    #region Components
    [Header("UI")]
    public TMP_InputField inputUsuario;
    public TMP_InputField inputContraseña;
    public Button botonLogin;
    public TextMeshProUGUI mensaje;

    [Header("Base de datos")]
    public string nombreDB = "MyDatabase.sqlite"; 
    private string rutaDB;

    [Header("Objetos a controlar")]
    public GameObject canvasLogin;   
    public GameObject canvasPrincipal;

    [Header("Refs")]
    public InventoryManager inventoryManager;
    #endregion


    void Start()
    {
        rutaDB = Application.persistentDataPath + "/" + nombreDB;

        botonLogin.onClick.AddListener(CheckLogin);
        mensaje.text = "insert user & Password";

        if (inventoryManager == null) //comprobem q existeixi
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
        }
    }

    void CheckLogin()
    {
        string usuario = inputUsuario.text.Trim();
        string contraseña = inputContraseña.text.Trim();

        if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(contraseña))
        {
            mensaje.text = "usuari i/o Contraseña buits";
            return;
        }

        string dbUri = "URI=file:" + rutaDB;

        try
        {
            using (var conexion = new SqliteConnection(dbUri))
            {
                conexion.Open();

                string consulta = "SELECT * FROM Usuarios WHERE usuario=@usuario AND password=@password";

                using (var comando = new SqliteCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@usuario", usuario);
                    comando.Parameters.AddWithValue("@password", contraseña);

                    using (IDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            mensaje.text = "Inicion sesiada";
                            Debug.Log("Usuario autenticado: " + usuario);

                            if (inventoryManager != null)
                            {
                                inventoryManager.SetUsuarioActual(usuario);
                            }

                            if (canvasPrincipal != null) canvasPrincipal.SetActive(true);
                            if (canvasLogin != null) canvasLogin.SetActive(false);
                        }
                        else
                        {
                            mensaje.text = "wrong Usser or password";
                        }
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            mensaje.text = "DB Error";
            Debug.LogError("Error SQLite: " + e.Message);
        }
    }
}
