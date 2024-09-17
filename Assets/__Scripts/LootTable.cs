using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    [Serializable]
    public class Drop 
    {
        public GameObject item;
        public int dropChance;
    }

    public List<Drop> table;

    public List<GameObject> GetDrop() 
    {
        List<GameObject> items = new List<GameObject>();
        int roll = 0;

        for (int i = 0; i < table.Count; i++) 
        {
            roll = UnityEngine.Random.Range(0, 100);
            if (table[i].dropChance > roll)
                items.Add(table[i].item);
        }

        return items;
    }
}
