using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour {

	public int health = 100;
	public int maxHealth = 100;
	public int damage = 5;
	public GameObject target;
	public int team = 1;
	public bool isHero = false;

	public static GameObject[] targets;

	void Start () {
	}

	void Update () {
	}

	public GameObject getGroundTarget(int team) {
		GameObject closestTarget = null;
		float closestX = 100f;

		if (team == Game.current.Team) {
			foreach (GameObject target in targets) {
				EnemyBase e = target.GetComponent<EnemyBase> ();
				if (e && e.health > 0 && e.team != team) {
					if (target.transform.position.x < closestX && target.transform.position.x > gameObject.transform.position.x) {
						closestX = target.transform.position.x;
						closestTarget = target;
					}
				}
			}
		} else {
			closestX = -100f;
			foreach (GameObject target in targets) {
				EnemyBase e = target.GetComponent<EnemyBase> ();
				if (e && e.health > 0 && e.team != team) {
					if (target.transform.position.x > closestX && target.transform.position.x < gameObject.transform.position.x) {
						closestX = target.transform.position.x;
						closestTarget = target;
					}
				}
			}
		}
		return closestTarget;
	}

	public GameObject getGroundAlly(int team) {
		GameObject closestTarget = null;
		float closestX = 100f;

		if (team == Game.current.Team) {
			foreach (GameObject target in targets) {
				EnemyBase e = target.GetComponent<EnemyBase> ();
				if (e && e.health > 0 && e.team == team) {
					if (target.transform.position.x < closestX && target.transform.position.x > gameObject.transform.position.x) {
						closestX = target.transform.position.x;
						closestTarget = target;
					}
				}
			}
		} else {
			closestX = -100f;
			foreach (GameObject target in targets) {
				EnemyBase e = target.GetComponent<EnemyBase> ();
				if (e && e.health > 0 && e.team == team) {
					if (target.transform.position.x > closestX && target.transform.position.x < gameObject.transform.position.x) {
						closestX = target.transform.position.x;
						closestTarget = target;
					}
				}
			}
		}
		return closestTarget;
	}
}
