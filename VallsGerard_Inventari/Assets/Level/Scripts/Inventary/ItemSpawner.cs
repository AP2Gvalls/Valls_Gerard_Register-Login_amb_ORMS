using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSpawner : MonoBehaviour
{
    [Header("Items disponibles")]
    public Item[] itemsParaSpawnear;

    [Header("Referencias")]
    public InventoryManager inventoryManager;
    public Transform spawnPanel; // Slot on apareixeran els items

    [Header("Prefab")]
    public GameObject itemButtonPrefab;

    void Start()
    {
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
        }

        ButtonCreator();
    }

    void ButtonCreator()
    {
        foreach (Item item in itemsParaSpawnear)
        {
            // crear boto per a cada item
            GameObject botonGO = Instantiate(itemButtonPrefab, spawnPanel);
            Button boton = botonGO.GetComponent<Button>();

            // txt edel button
            Text textoBoton = botonGO.GetComponentInChildren<Text>();
            if (textoBoton != null)
            {
                textoBoton.text = item.ItemName;
            }

            // listener per a afegir el item
            Item itemCapturado = item; // Capturar variable para el closure
            boton.onClick.AddListener(() => AddItemInveratry(itemCapturado));
        }
    }

    void AddItemInveratry(Item item)
    {
        if (inventoryManager != null)
        {
            bool añadido = inventoryManager.AñadirItem(item);
            if (añadido)
            {
                Debug.Log($"Item {item.ItemName} añadido al inventario");
            }
            else
            {
                Debug.Log("Inventario lleno");
            }
        }
    }
}
