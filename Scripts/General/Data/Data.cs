using System.Collections.Generic;
using System;

[Serializable]
public class Data
{
    public int health;
    public List<string> itemsInvetory;
    public List<Item> itemsOnScene;
    public int curLevel;
    public int curEnemies;
}
