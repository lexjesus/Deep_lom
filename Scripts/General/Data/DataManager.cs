using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    private string _filename;
    Data gamestate;
    public void Startup() {
        _filename = Path.Combine(Application.persistentDataPath, "game.json");
        status = ManagerStatus.Started;
    }

    private List<string> DictToList(Dictionary<ItemName, int> dict) {
        List<string> list = new List<string>(dict.Count*2);
        foreach(var item in dict) {
            list.Add(item.Key.ToString());
            list.Add(item.Value.ToString());
        }
        return list;
    }
    private Dictionary<ItemName, int> ListToDict(List<string> list) {
        Dictionary<ItemName, int> dict = new Dictionary<ItemName, int>();
        for(int i = 0; i < list.Count; i+=2) {
            var tmp = StringToEnum(list[i]);
            dict.Add(StringToEnum(list[i]), int.Parse(list[i + 1]));
        }
        return dict;
    }
    private ItemName StringToEnum(string s) {
        switch (s){
            case "Health": return ItemName.Health;
            case "Power": return ItemName.Power;
            case "Speed": return ItemName.Speed;
            default: return ItemName.Nothing;
        }
    }

    public void SaveGameState() {
        gamestate = new Data();
        
        gamestate.health = Managers.Player.health;
        Dictionary<ItemName, int> dict = Managers.Inventory.GetData1();

        gamestate.itemsInvetory = DictToList(dict);
        gamestate.itemsOnScene = Managers.Inventory.GetData2();
        gamestate.curLevel = Managers.Mission.curLevel;
        gamestate.curEnemies = Managers.Enemy.GetCurLevelEnemies();
        
        File.WriteAllText(_filename, JsonUtility.ToJson(gamestate));
        Messenger<string>.Broadcast(GameEvent.DEBUG, "Game saved!");
    }
    public void LoadGameState() {
        gamestate = new Data();
        if(!File.Exists(_filename)) {
            Messenger<string>.Broadcast(GameEvent.DEBUG, "No saved game");
            return;
        }
        gamestate = JsonUtility.FromJson<Data>(File.ReadAllText(_filename));
        Managers.Player.UpdateData(gamestate.health);
        Dictionary<ItemName, int> t = ListToDict(gamestate.itemsInvetory);
        Managers.Inventory.UpdateData(t, gamestate.itemsOnScene);

        Managers.Enemy.LoadUpdateData(gamestate.curEnemies);
        Managers.Mission.UpdateData(gamestate.curLevel);
        Managers.Mission.RestartCurrent();
    }
}
