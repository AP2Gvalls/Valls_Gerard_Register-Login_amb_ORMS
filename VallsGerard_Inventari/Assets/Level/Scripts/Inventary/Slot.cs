using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [Header("UI")]
    public Image iconImage;
    public TextMeshProUGUI itemNameText;
    public Button deleteButton;

    [Header("Slot Info")]
    public int slotIndex;

    private Item currentItem;

    void Start()
    {
        // Inicializar slot vacío
        //ClearSlot();
        
        // Configurar el botón de eliminar
        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(EliminarItem);
        }

        // Debug para verificar referencias
        Debug.Log($"Slot {slotIndex} - IconImage asignada: {iconImage != null}");
        Debug.Log($"Slot {slotIndex} - ItemNameText asignada: {itemNameText != null}");
    }

    public void SetItem(Item newItem)
    {
        currentItem = newItem;

        if (currentItem != null)
        {

            // Mostrar el icono del item
            if (iconImage != null)
            {
                // IMPORTANTE: Activar el GameObject del icono
                iconImage.gameObject.SetActive(true);
                iconImage.enabled = true;

                // Si el item tiene sprite, mostrarlo
                if (currentItem.ItemSprite != null)
                {
                    iconImage.sprite = currentItem.ItemSprite;
                    iconImage.color = Color.white; // Asegurar que es visible
                    Debug.Log($"Sprite asignado: {currentItem.ItemSprite.name}");
                }

                // Asegurar que la imagen está en modo visible
                var canvasGroup = iconImage.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1f;
                }
            }
            else
            {
                Debug.LogWarning($"iconImage es NULL en slot {slotIndex}!");
            }

            // Mostrar el nombre del item
            if (itemNameText != null)
            {
                itemNameText.gameObject.SetActive(true);
                itemNameText.text = currentItem.ItemName;
                itemNameText.enabled = true;
                itemNameText.color = Color.white; // Asegurar que es visible
                
            }

            // Mostrar el botón de eliminar cuando hay un item
            if (deleteButton != null)
            {
                deleteButton.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log($"SetItem llamado con item NULL en slot {slotIndex}");
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        currentItem = null;

        if (iconImage != null)
        {
            iconImage.gameObject.SetActive(false);
            iconImage.enabled = false;
            iconImage.sprite = null;
        }

        if (itemNameText != null)
        {
            itemNameText.gameObject.SetActive(false);
            itemNameText.text = "";
            itemNameText.enabled = false;
        }

        // Ocultar el botón de eliminar cuando el slot está vacío
        if (deleteButton != null)
        {
            deleteButton.gameObject.SetActive(false);
        }
    }

    public Item GetItem()
    {
        return currentItem;
    }

    // Método para eliminar el item del slot
    public void EliminarItem()
    {
        if (currentItem != null)
        {
            Debug.Log($"Eliminando {currentItem.ItemName} del slot {slotIndex}");

            ClearSlot();

            // Notificar al InventoryManager que hubo cambios
            InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
            if (inventoryManager != null)
            {
                inventoryManager.OnInventoryChanged();
            }
        }
    }
}