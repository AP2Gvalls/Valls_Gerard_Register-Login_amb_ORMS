using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GestioUsuaris : MonoBehaviour
{
    #region Inspector
    [Header("UI")]
    public Transform contingutLlista;

    public GameObject prefabFilaUsuari;

    public Button botonRefrescar;

    public TextMeshProUGUI missatgeEstat;
    #endregion

    void Start()
    {
        botonRefrescar?.onClick.AddListener(CarregarUsuaris);
        CarregarUsuaris();
    }

    public void CarregarUsuaris()
    {
        // Buida la llista anterior
        foreach (Transform fill in contingutLlista)
            Destroy(fill.gameObject);

        if (DBContent.Instance == null)
        {
            missatgeEstat.text = "Error: DBContext no disponible.";
            return;
        }

        try
        {
            List<UsuariORM> usuaris = UsuariORM.GetAll(DBContent.Instance.Connexio);

            if (usuaris.Count == 0)
            {
                missatgeEstat.text = "No hi ha usuaris registrats.";
                return;
            }

            missatgeEstat.text = $"{usuaris.Count} usuari(s) trobats.";

            foreach (UsuariORM u in usuaris)
            {
                GameObject fila = Instantiate(prefabFilaUsuari, contingutLlista);
                FilaUsuari comp = fila.GetComponent<FilaUsuari>();
                if (comp != null)
                    comp.Inicialitzar(u.Id, u.Usuari, this);
            }
        }
        catch (System.Exception e)
        {
            missatgeEstat.text = "Error carregant usuaris.";
            Debug.LogError("Error ORM GetAll: " + e.Message);
        }
    }
    public void EsborrarUsuari(int id, string nomUsuari)
    {
        if (DBContent.Instance == null) return;

        try
        {

            bool ok = UsuariORM.DeleteById(DBContent.Instance.Connexio, id);

            if (ok)
            {
                Debug.Log("Usuari esborrat: " + nomUsuari);
                CarregarUsuaris(); // refresh
            }
            else
            {
                missatgeEstat.text = "No s'ha pogut esborrar l'usuari.";
            }
        }
        catch (System.Exception e)
        {
            missatgeEstat.text = "Error esborrant usuari.";
            Debug.LogError("Error ORM Delete: " + e.Message);
        }
    }
}
