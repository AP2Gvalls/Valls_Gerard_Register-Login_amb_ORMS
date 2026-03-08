using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegistreUsuari : MonoBehaviour
{
    #region Inspector
    [Header("UI")]
    public TMP_InputField inputUsuari;
    public TMP_InputField inputContrasenya;
    public Button botonRegistrar;
    public TextMeshProUGUI mensaje;

    [Header("Navegació")]
    public GameObject panellRegistre;
    public GameObject panellLogin;
    #endregion

    void Start()
    {
        botonRegistrar.onClick.AddListener(Registrar);
        mensaje.text = "Omple els camps per registrar-te.";
    }

    void Registrar()
    {
        string usuari = inputUsuari.text.Trim();
        string contrasenya = inputContrasenya.text.Trim();

        if (string.IsNullOrEmpty(usuari) || string.IsNullOrEmpty(contrasenya))
        {
            mensaje.text = "Usuari i/o contrasenya buits.";
            return;
        }

        if (contrasenya.Length < 8)
        {
            mensaje.text = "La contrasenya ha de tenir mínim 8 caràcters.";
            return;
        }

        if (DBContent.Instance == null)
        {
            mensaje.text = "Error: no hi ha DBContext a l'escena.";
            return;
        }

        try
        {

            bool ok = UsuariORM.Insert(DBContent.Instance.Connexio, usuari, contrasenya);

            if (ok)
            {
                mensaje.text = "Usuari registrat correctament!";
                Debug.Log("Nou usuari: " + usuari);

                // Torna al panell de login
                panellRegistre?.SetActive(false);
                panellLogin?.SetActive(true);
            }
            else
            {
                mensaje.text = "Error: l'usuari ja existeix o dades incorrectes.";
            }
        }
        catch (System.Exception e)
        {
            mensaje.text = "Error de BD.";
            Debug.LogError("Error ORM Registre: " + e.Message);
        }
    }
}
