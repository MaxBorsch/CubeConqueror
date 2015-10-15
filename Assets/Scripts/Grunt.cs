using UnityEngine;
using System.Collections;

public class Grunt : EnemyBase {

	private SpriteRenderer sprite;
	private EnemyController controller;

	public HealthBarGUI healthGUI;

	void Start () {
		sprite = gameObject.GetComponent<SpriteRenderer> ();
		controller = gameObject.GetComponent<EnemyController> ();
		healthGUI = gameObject.GetComponent<HealthBarGUI> ();

		if (team == 1) {
			sprite.color = Color.white;
		}else if (team == -1) {
			sprite.color = Color.black;
		}

		InvokeRepeating("Attack", 1, 1F);
	}

	void Attack() {
		if (target != null) {
			EnemyBase ai = target.GetComponent<EnemyBase> ();
			if (ai) {
				ai.health = ai.health - damage;
			}
		}
	}

	void Update () {
		if (health <= 0) {
			GameObject.FindWithTag ("GameController").GetComponent<GameManager>().enemyKilled(gameObject, team);
		} else {
			healthGUI.healthPercent = (float) health / maxHealth;
		}

		if (gameObject == null) return;

		if (isHero == false) {
			GameObject a = getGroundAlly (team);
			GameObject t = getGroundTarget (team);
			if (a == null) {
				if (t) {
					if (Vector3.Distance(transform.position, t.transform.position) > 0.5f) {
						target = null;
						controller.moveDirection = (team == Game.current.Team ? 1f : -1f);
					} else {
						controller.moveDirection = 0;
						target = t;
					}
				} else {
					target = null;
				}
			} else {
				if (Vector3.Distance(transform.position, a.transform.position) > 1.5f) {
					controller.moveDirection = (team == Game.current.Team ? 1f : -1f);
				} else {
					controller.moveDirection = 0;
				}

				if (t) {
					if (Vector3.Distance(transform.position, t.transform.position) > 0.5f) {
						target = null;
					} else {
						target = t;
						controller.moveDirection = 0;
					}
				}
			}
		} else {
			// If the jump button is pressed and the player is grounded then the player should jump.
			if(Input.GetButtonDown("Jump") && controller.grounded)
				controller.jump = true;

			// Cache the horizontal input.
			float h = Input.GetAxis("Horizontal");
			controller.moveDirection = h;
		}
	}
}
