using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IniciarSesion : MonoBehaviour
{
    #region Inspector
    [Header("UI Login")]
    public TMP_InputField inputUsuario;
    public TMP_InputField inputContraseña;
    public Button botonLogin;
    public TextMeshProUGUI mensaje;

    [Header("Objectes a controlar")]
    public GameObject canvasLogin;
    public GameObject canvasPrincipal;

    [Header("Refs")]
    public InventoryManager inventoryManager;
    #endregion

    // ── Lifecycle ────────────────────────────────────────────────────────────
    void Start()
    {
        botonLogin.onClick.AddListener(CheckLogin);
        mensaje.text = "Introdueix usuari i contrasenya";

        if (inventoryManager == null)
            inventoryManager = FindObjectOfType<InventoryManager>();
    }

    // ── Login amb ORM ────────────────────────────────────────────────────────
    void CheckLogin()
    {
        string usuari = inputUsuario.text.Trim();
        string contrasenya = inputContraseña.text.Trim();

        if (string.IsNullOrEmpty(usuari) || string.IsNullOrEmpty(contrasenya))
        {
            mensaje.text = "Usuari i/o contrasenya buits.";
            return;
        }

        if (DBContent.Instance == null)
        {
            mensaje.text = "Error: no hi ha DBContext a l'escena.";
            return;
        }

        try //pot ser que falli l'inici de sesio
        {
            UsuariORM usuariTrobat = UsuariORM.FindByCredentials(
                DBContent.Instance.Connexio, usuari, contrasenya);

            if (usuariTrobat != null)
            {
                mensaje.text = "Sessió iniciada correctament!";
                Debug.Log("Usuari autenticat: " + usuariTrobat.Usuari);

                inventoryManager?.SetUsuarioActual(usuariTrobat.Usuari);

                canvasPrincipal?.SetActive(true);
                canvasLogin?.SetActive(false);
            }
            else
            {
                mensaje.text = "Usuari o contrasenya incorrectes.";
            }
        }
        catch (System.Exception ex)
        {
            mensaje.text = "Error de BD.";
            Debug.LogError("Error ORM Login: " + ex.Message);
        }
    }
}
