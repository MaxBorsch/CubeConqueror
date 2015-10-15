using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject character;
	public GameObject beam;
	new public CameraController camera;
	public enum GameState {
		Menu,
		Building,
		Attacking,
		Paused
	};
	public GameState State;
	public Material[] WhiteMaterial;
	public Material[] BlackMaterial;
	public int chunksPerStage = 16;


	private GameObject currentBeam;
	private World world;
	private int enemies = 0;

	// Use this for initialization
	void Start () {
		State = GameState.Menu;
		world = gameObject.GetComponent<World>();
		Language.LoadLanguage("English");
		Language.RefreshLanguages();

		SaveManager.Load();

		GameObject chunk = world.generateChunk (0);
		GameObject chunk2 = (GameObject) Instantiate(chunk);
		chunk2.transform.position = new Vector3 (2 * (World.chunkWidth/2f), 0f, 1f);
		world.chunks.Add (chunk2);
		GameObject chunk3 = (GameObject) Instantiate(chunk);
		chunk3.transform.position = new Vector3 (4 * (World.chunkWidth/2f), 0f, 1f);
		world.chunks.Add (chunk3);

		float vertExtent = camera.gameObject.GetComponent<Camera>().orthographicSize;
		float horzExtent = vertExtent * Screen.width / Screen.height;
		camera.gameObject.transform.position = new Vector3(horzExtent, 4f, -1f);
	}

	public void stopGame() {
		Time.timeScale = 1;
		enemies = 0;
		Game.current = null;
		State = GameState.Menu;

		GameObject[] enems = GameObject.FindGameObjectsWithTag("Enemy");

		foreach (GameObject enemy in enems) {
			Destroy(enemy);
        }

		camera.target = null;
		camera.maxPosition = new Vector3 (68f, 25f, 0f);
		camera.transform.position = (new Vector3(10f, 4f, -1f));
		camera.calculateBounds();

		if (currentBeam) Destroy(currentBeam);

		world.clearChunks();
		GameObject chunk = world.generateChunk (0);
		GameObject chunk2 = (GameObject) Instantiate(chunk);
		chunk2.transform.position = new Vector3 (2 * (World.chunkWidth/2f), 0f, 1f);
		world.chunks.Add (chunk2);
		GameObject chunk3 = (GameObject) Instantiate(chunk);
		chunk3.transform.position = new Vector3 (4 * (World.chunkWidth/2f), 0f, 1f);
		world.chunks.Add (chunk3);
	}

	public void startGame(Game game) {

		Game.current = game;
		State = GameState.Building;

		camera.maxPosition = new Vector3 ((Game.current.Chunk)*34f, 25f, 0f);
		camera.calculateBounds();

		world.clearChunks();
		for (int i=0; i<Game.current.Chunk; i++) {
			GameObject ch = world.generateChunk (i);
			if (Game.current.Team == 1) {
				ch.GetComponent<MeshRenderer>().materials = WhiteMaterial;
			} else {
				ch.GetComponent<MeshRenderer>().materials = BlackMaterial;
			}
		}

		currentBeam = (GameObject) Instantiate(beam);

		GameObject c = (GameObject) Instantiate(character);
		c.transform.position = new Vector3 (1f, 20f, 0f);

		Grunt ai = c.GetComponent<Grunt>();
			ai.isHero = true;
			ai.team = Game.current.Team;
		EnemyController cont = c.GetComponent<EnemyController>();
			cont.maxSpeed = 2.5f;

		camera.target = c.transform;
	}

	void spawnEnemies() {
		for (int i=0; i<5; i++) {
			GameObject c = (GameObject) Instantiate(character);
				c.transform.position = new Vector3 ((Game.current.Chunk)*16f + (i * 1f) + 2f, 5f, 0f);
			Grunt ai = c.GetComponent<Grunt>();
				ai.team = -Game.current.Team;
			enemies++;
		}

		EnemyBase.targets = GameObject.FindGameObjectsWithTag ("Enemy");
	}

	public void spawnUnit(int unit) {

		EnemyBase.targets = GameObject.FindGameObjectsWithTag ("Enemy");
	}

	public void enemyKilled(GameObject e, int team) {
		if (Game.current != null && team != Game.current.Team) {
			enemies--;
			Game.current.Cubes++;
			if (enemies <= 0) {
				enemies = 0;
				Victory();
			}
		}
		Destroy(e);
		EnemyBase.targets = GameObject.FindGameObjectsWithTag ("Enemy");
	}

	public void Victory() {
		if (Game.current.Team == 1) {
			world.chunks[Game.current.Chunk].GetComponent<MeshRenderer>().materials = WhiteMaterial;
		} else {
			world.chunks[Game.current.Chunk].GetComponent<MeshRenderer>().materials = BlackMaterial;
		}
		State = GameState.Building;
		Game.current.Chunk++;
		SaveManager.Save();
	}

	public void Attack() {
		if (Game.current == null || State == GameState.Attacking) return;
		State = GameState.Attacking;
		GameObject ch = world.generateChunk (Game.current.Chunk);
		if (Game.current.Team == 1) {
			ch.GetComponent<MeshRenderer>().materials = BlackMaterial;
		} else {
			ch.GetComponent<MeshRenderer>().materials = WhiteMaterial;
		}

		spawnEnemies();

		camera.maxPosition = new Vector3 ((Game.current.Chunk + 1)*34f, 25f, 0f);
		camera.calculateBounds();
	}

	// Update is called once per frame
	void Update () {
		if (State == GameState.Menu){
			float vertExtent = camera.gameObject.GetComponent<Camera>().orthographicSize;
			float horzExtent = vertExtent * Screen.width / Screen.height;

			if (camera.gameObject.transform.position.x >= World.chunkWidth + (horzExtent))
				camera.gameObject.transform.position = new Vector3(horzExtent, 4f, -1f);

			camera.gameObject.transform.Translate(Vector3.right * Time.deltaTime);
			//camera.gameObject.transform.rotation = Quaternion.Euler((-1f + Mathf.Sin (Time.time)) * 100f * Vector3.forward * Time.deltaTime);
		}

		if (Game.current != null) {
			if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
			 {
				 if (Time.timeScale == 0)
				 {
					Time.timeScale = 1;
				 } else {
					Time.timeScale = 0;
				 }
			 }

			if (currentBeam) {
				currentBeam.transform.position = new Vector3 (Game.current.Chunk * World.chunkWidth, 0f, 1f);
			}
		}
	}
}
