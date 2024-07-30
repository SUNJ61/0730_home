using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private Transform ItemTr;
    private Transform inventoryTr;
    private Transform ItemList;
    private CanvasGroup canvasGroup;

    private string invenStr = "Inventory";
    private string itemListStr = "ItemList";

    public static GameObject draggingItem = null;
    void Start()
    {
        inventoryTr = GameObject.Find(invenStr).transform;
        ItemList = GameObject.Find(itemListStr).transform;
        ItemTr = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        ItemTr.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggingItem = null;
        canvasGroup.blocksRaycasts = true;
        if (ItemTr.parent == inventoryTr)
        {
            ItemTr.SetParent(ItemList);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.transform.SetParent(inventoryTr);
        draggingItem = this.gameObject;
        canvasGroup.blocksRaycasts = false;
    }
}
