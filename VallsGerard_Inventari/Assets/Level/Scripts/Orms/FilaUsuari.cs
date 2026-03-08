using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilaUsuari : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI labelNom;
    public Button botonEsborrar;

    private int _id;
    private string _nomUsuari;
    private GestioUsuaris _gestor;

    /// <summary>Inicialitza la fila amb les dades de l'usuari.</summary>
    public void Inicialitzar(int id, string nomUsuari, GestioUsuaris gestor)
    {
        _id = id;
        _nomUsuari = nomUsuari;
        _gestor = gestor;

        if (labelNom != null)
            labelNom.text = $"[{id}] {nomUsuari}";

        if (botonEsborrar != null)
            botonEsborrar.onClick.AddListener(() => _gestor.EsborrarUsuari(_id, _nomUsuari));
    }
}
