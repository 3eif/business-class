using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// stores all the descriptions for planes
[CreateAssetMenu(fileName = "Shops", menuName = "Shop", order = 0)]
public class Shop : ScriptableObject
{
    public string planeName;
    public string description;
    public float fuel;
    public int price;
}
