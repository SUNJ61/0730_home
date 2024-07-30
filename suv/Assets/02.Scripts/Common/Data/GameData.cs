using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataInfo
{
    public class GameData
    {
        public int KillCount = 0;
        public float hp = 120f;
        public float damage = 10f;
        public float speed = 10f;
        public List<Item> equipItem = new List<Item>();
    }
    public class Item
    {
        public enum ItemType { HP, SPEED, GRANADE, DAMAGE };
        public enum ItemCalc { VALUE, PERSENT };

        public ItemType itemType;
        public ItemCalc itemCalc;

        public string name;
        public string desc;
        public float value;
    }
}
