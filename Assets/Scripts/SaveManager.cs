using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveManager {

	public static List<Game> savedGames = new List<Game>();

	public static void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/saves.dat");
		bf.Serialize(file, SaveManager.savedGames);
		file.Close();
	}

	public static void Load() {
		if(File.Exists(Application.persistentDataPath + "/saves.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/saves.dat", FileMode.Open);
			SaveManager.savedGames = (List<Game>)bf.Deserialize(file);
			file.Close();
		}
	}

	public static void Delete(Game game) {
		savedGames.Remove (game);
		Save();
	}

}
