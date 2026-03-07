using UnityEngine;
using UnityEngine.UI;

public class ReegisteeredScript : MonoBehaviour
{
    [Header("ExitButton")]
    public Button boton; 

    void Start()
    {
        if (boton != null)
        {
            boton.onClick.AddListener(Salir); 
        }
    }

    public void Salir()
    {
        Application.Quit();

        // Esto permite probar en el Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
