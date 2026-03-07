using UnityEngine;
using UnityEngine.UI;

public class BotonReg : MonoBehaviour
{
    [Header("var")]
    public GameObject registro;    
    public GameObject LogIn; 

    [Header("buttons")]
    public Button buton;                

    void Start()
    {
        if (buton != null)
        {
            buton.onClick.AddListener(ToogleScenes);
        }

    }

    public void ToogleScenes()
    {
        if (registro != null)
            registro.SetActive(true);

        if (LogIn != null)
            LogIn.SetActive(false);
    }
}
