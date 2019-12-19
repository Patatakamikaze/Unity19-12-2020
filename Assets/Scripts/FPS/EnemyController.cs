using UnityEngine;

public class EnemyController : MonoBehaviour {

    private CharacterHealth characterHealth;
    public int points;

	// Use this for initialization
	void Start () {
        characterHealth = GetComponent<CharacterHealth>();
	}
	
	// Update is called once per frame
	void Update () {
        if (characterHealth.isDead) {
            GameController.score+=points;
            Destroy(gameObject, 0.2f);
            enabled = false;
        }
	}
}
