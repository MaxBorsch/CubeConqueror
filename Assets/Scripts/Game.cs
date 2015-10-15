using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game {

	public static Game current;

	public string Name;
	public int Cubes;
	public int Stage;
	public int Chunk;
	public int Team;
	public Color Color;
	public int GrayCubes;

    public Game () {
		this.Name = "World";
		this.Cubes = 10;
		this.Stage = 1;
		this.Chunk = 1;
		this.Team = 1;
		this.GrayCubes = 0;
		this.Color = Color.white;
    }

}
