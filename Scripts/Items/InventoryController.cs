using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private GameObject powerPrefab;
    [SerializeField] private GameObject speedPrefab;
    private GameObject _item;
    private GameObject tmp;
    private Item newItem;
    Dictionary<ItemName, GameObject> ItemPrefabs;

    private void Awake() {
        Messenger<GameObject>.AddListener(GameEvent.ENEMY_DIE, CreateItemOnScene);
    }
    private void OnDestroy() {
        Messenger<GameObject>.RemoveListener(GameEvent.ENEMY_DIE, CreateItemOnScene);
    }

    private void Start() {
        ItemPrefabs = new Dictionary<ItemName, GameObject>(3);
        ItemPrefabs[ItemName.Health] = healthPrefab;
        ItemPrefabs[ItemName.Power] = powerPrefab;
        ItemPrefabs[ItemName.Speed] = speedPrefab;
        foreach(Item item in Managers.Inventory._itemsOnScene) {
            tmp = Instantiate(ItemPrefabs[item.itemName]) as GameObject;
            tmp.transform.position = item.location;
        }
    }

    public void CreateItemOnScene(GameObject enemy) {
        newItem = null;
        //Messenger<string>.Broadcast(GameEvent.DEBUG, "Item Creation");
        Vector3 location = enemy.transform.position;
        int i = Random.Range(0, 3);
        ItemName itemName = ItemName.Nothing;
        switch(i) {
            case 0: itemName = ItemName.Health;
                newItem = new Item(itemName, location);
                _item = Instantiate(healthPrefab) as GameObject;
                _item.transform.position = location;
                break;
            case 1: itemName = ItemName.Power;
                newItem = new Item(itemName, location);
                _item = Instantiate(powerPrefab) as GameObject;
                _item.transform.position = location;
                break;
            case 2: itemName = ItemName.Speed;
                newItem = new Item(itemName, location);
                _item = Instantiate(speedPrefab) as GameObject;
                _item.transform.position = location;
                break;
        }
        Managers.Inventory.AddItemsOnScene(newItem);
    }

}
