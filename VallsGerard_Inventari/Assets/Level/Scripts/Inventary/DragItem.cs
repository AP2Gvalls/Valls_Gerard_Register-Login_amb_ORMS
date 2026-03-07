using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour
{
    public Item item;

    private Transform originalParent;
    private Vector3 originalPosition;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.position;

        // Hacer que el objeto sea semi-transparente mientras se arrastra
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // Mover a la raíz del canvas para que esté por encima de todo
        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Seguir el cursor
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        // Si no se soltó sobre un slot válido, volver a la posición original
        if (transform.parent == canvas.transform)
        {
            transform.SetParent(originalParent);
            transform.position = originalPosition;
        }
    }
}
