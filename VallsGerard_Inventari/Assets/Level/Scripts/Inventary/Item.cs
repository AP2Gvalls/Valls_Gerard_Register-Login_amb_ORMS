using System;
using UnityEngine;

[CreateAssetMenu(fileName ="item.asset", menuName ="Inventory/Item" )]// dresera de creacio de scriptableObject
public class Item : ScriptableObject
{
    public string ItemID;
    public string ItemName;
    public Sprite ItemSprite; //sprite de item

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(ItemID))  GenerateUniqueID();  //Flipa if sense claudators
    }

    //funcio de la id
    public void GenerateUniqueID()
    {
        ItemID = Guid.NewGuid().ToString();  //Genera una id i la agrega
    }
}
