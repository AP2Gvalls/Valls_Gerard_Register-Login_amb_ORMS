using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WalletCurrency : MonoBehaviour
{
    public float bank = 0;

    public static WalletCurrency instance;

    public TextMeshProUGUI Score_txt;
    const string SAVEGAMEKEY_MONEY = "SavedMoney";




    //serveix per resetajar els diners (es un Singeltone)
    public void ResetMoney(bool save = true)
    {
        PlayerPrefs.DeleteKey(SAVEGAMEKEY_MONEY);
        if (save)
        {
            PlayerPrefs.Save();
        }

        LoadMoney();

    }

    public void SaveMoney(bool save = true)
    {
        PlayerPrefs.SetFloat(SAVEGAMEKEY_MONEY, bank);
        if (save) PlayerPrefs.Save();

        // Guardar a la BD
        if (DBContent.Instance != null && !string.IsNullOrEmpty(usuariActual))
            UsuariORM.UpdateWallet(DBContent.Instance.Connexio, usuariActual, bank);
    }


    private void Awake()
    {
        // Cridem per reiniciar el money
        ResetMoney();


        if (instance == null)
        {
            instance = this;
            LoadMoney();
        }
    }

    // Update is called once per frame
    public void Score(float points)
    {
        bank += points;
        SaveMoney();
        UpdateUI();
    }

    public void LoadMoney()
    {
        //carregar la Db o base de dades o taula com li vulguis dur
        if (DBContent.Instance != null && !string.IsNullOrEmpty(usuariActual))
        {
            float? walletBD = UsuariORM.GetWallet(DBContent.Instance.Connexio, usuariActual);
            if (walletBD.HasValue)
            {
                bank = walletBD.Value;
                UpdateUI();
                return;
            }
        }
        // Fallback a PlayerPrefs
        bank = PlayerPrefs.GetFloat(SAVEGAMEKEY_MONEY, 20);
        UpdateUI();
    }

    // Afegir camp i setter
    private string usuariActual = "";
    public void SetUsuari(string nom)
    {
        usuariActual = nom;
        LoadMoney(); // recarregar amb les dades d'aquest usuari
    }

    void UpdateUI()
    {
        Score_txt.text = bank.ToString();
    }
}
