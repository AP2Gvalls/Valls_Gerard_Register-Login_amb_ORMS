using UnityEngine;
using UnityEngine.UI;

public class RegisteredScript : MonoBehaviour
{
    [Header("Exitbutton")]
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
