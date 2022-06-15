using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryPopup : MonoBehaviour
{
    [SerializeField] private Image[] itemIcons;
    [SerializeField] private Text[] itemLabels;
    [SerializeField] private Text curItemLabel;
    [SerializeField] private Button useButton;

    private new ItemName _curItem;
    public void Refresh() {
        List<ItemName> itemList = Managers.Inventory.GetItemList();

        int len = itemIcons.Length;
        for(int i = 0; i < len; i++) {
            if(i < itemList.Count) {
                itemIcons[i].gameObject.SetActive(true);
                itemLabels[i].gameObject.SetActive(true);

                ItemName item = itemList[i];

                Sprite sprite = Resources.Load<Sprite>("Icons/" + item);
                itemIcons[i].sprite = sprite;
                //itemIcons[i].SetNativeSize();
                int count = Managers.Inventory.GetItemCount(item);
                string message = "x" + count;
                
                itemLabels[i].text = message;
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((BaseEventData data) => {
                    OnItem(item);
                });
                EventTrigger trigger = itemIcons[i].GetComponent<EventTrigger>();
                trigger.triggers.Clear();
                trigger.triggers.Add(entry);

            }
            else {
                itemIcons[i].gameObject.SetActive(false);
                itemLabels[i].gameObject.SetActive(false);
            }
        }

        if(!itemList.Contains(_curItem)) {
            _curItem = ItemName.Nothing;
        }
        if(_curItem == ItemName.Nothing) {
            curItemLabel.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
        }
        else {
            curItemLabel.gameObject.SetActive(true);
            useButton.gameObject.SetActive(true);
            curItemLabel.text = _curItem.ToString();
        }
    }
    public void OnItem(ItemName item) {
        _curItem = item;
        Refresh();
    }
    public void OnUse() {
        Managers.Inventory.ConsumeItem(_curItem);

        switch(_curItem) {
            case ItemName.Health: Managers.Player.ChangeHealth(25); break;
            case ItemName.Power: Managers.Player.RaisePower(); break;
            case ItemName.Speed: Managers.Player.RaiseSpeed(); break;
        }
        Refresh();
    }
}
