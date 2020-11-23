using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsList : MonoBehaviour {
    static public BuildingsList instance;

    // Start is called before the first frame update
    [SerializeField] GameObject[] BuildingsPrefabs;
    ItemSO[] ItemsData;

    void Awake() {
        instance = this;

        ItemsData = new ItemSO[BuildingsPrefabs.Length];
        for (ushort i = 0; i < BuildingsPrefabs.Length; ++i)
            ItemsData[i] = BuildingsPrefabs[i].GetComponent<BaseBuilding>().Item;
    }

    public GameObject GetItemPrefab(ItemSO item) {
        for (ushort i = 0; i < BuildingsPrefabs.Length; ++i) {
            if (ItemsData[i].Type == item.Type)
                return BuildingsPrefabs[i];
        }
        return null;
    }
}
