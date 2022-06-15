using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : MonoBehaviour
{

    public new ItemName name;

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<PlayerCharacter>()) {
            Managers.Inventory.AddItem(name);

            Destroy(this.gameObject);
        }
    }
}
