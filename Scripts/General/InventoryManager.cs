using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour, IGameManager {
    
    public ManagerStatus status { get; private set; }
    private Dictionary<ItemName, int> _items;
    public List<Item> _itemsOnScene { get; set; }

    
    public void Startup() {
        Debug.Log("Inventory manager starting...");
        UpdateData(new Dictionary<ItemName, int>(), new List<Item>() );
        status = ManagerStatus.Started;
    }

    private void DisplayItems() {
        string itemDisplay = "Items: ";
        foreach(KeyValuePair<ItemName, int> item in _items) {
            itemDisplay += item.Key + "(" + item.Value + ") ";
        }
        Debug.Log(itemDisplay);
    }

    public void AddItem(ItemName name) {
        if(_items.ContainsKey(name)) {
            _items[name] += 1;
        }
        else {
            _items[name] = 1;
        }
        DisplayItems();
    }

    public List<ItemName> GetItemList() {
        List<ItemName> list = new List<ItemName>(_items.Keys);
        return list;
    }

    public int GetItemCount(ItemName name) {
        if(_items.ContainsKey(name)) {
            return _items[name];
        }
        return 0;
    }

    public bool ConsumeItem(ItemName name) {
        if(_items.ContainsKey(name)) {
            _items[name]--;
            if(_items[name] == 0) {
                _items.Remove(name);
            } 
        }
        else {
            Debug.Log("Cannot consume " + name);
            return false;
        }
        DisplayItems();
        return true;
    }
    public void UpdateData(Dictionary<ItemName, int> items, List<Item> itemsOnScene) {
        _items = items;
        _itemsOnScene = itemsOnScene;
    }
    public Dictionary<ItemName, int> GetData1() {
        return _items;
    }
    public List<Item> GetData2() {
        return _itemsOnScene;
    }

    public void AddItemsOnScene(Item item) {
        _itemsOnScene.Add(item);
    }
}
