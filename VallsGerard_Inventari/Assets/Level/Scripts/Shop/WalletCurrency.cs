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

    public void LoadMoney()
    {
        bank = PlayerPrefs.GetFloat(SAVEGAMEKEY_MONEY, 20);
        UpdateUI();
    }




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

        if (save)
        {
            PlayerPrefs.Save();
        }
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

    void UpdateUI()
    {
        Score_txt.text = bank.ToString();
    }
}
