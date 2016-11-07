using UnityEngine;
using System.Collections;

public class GameItemManager : MonoBehaviour {
	public int maxPosition = 2;
	public int items = 0;

	public float counter = 3f;

	public PhotonNetworkGameItemManager photonGameItemManager;

	// Use this for initialization
	void Start () {
		photonGameItemManager = GetComponent<PhotonNetworkGameItemManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (counter <= 0) {
			counter = 3f;
			float dropRate = Random.Range (0, 100);
			if (dropRate < 10) {
				SetBomb ();
				SetCoin ();
			} else {
				SetCoin ();
				SetCoin ();
			}
		}	

		if (counter > 0) {
			counter -= Time.deltaTime;
		}
	}

	public void SetCoin () {
		StartCoroutine (CoinSpawn ());
	}

	public void SpawnCoin (Vector3 position) {
		items += 1;
		GameObject item = Instantiate (Resources.Load ("Prefabs/Items/coin"), position, Quaternion.identity) as GameObject;
	}

	IEnumerator CoinSpawn() {
		yield return new WaitForSeconds (1f);
		int rndStart = Random.Range (-2, 3);
		Vector3 spawnPosition = new Vector3 (Random.Range (-2, 3), 0, Random.Range (-2, 3));
		if (PhotonNetwork.isMasterClient) {
			photonGameItemManager.SpawnCoin (spawnPosition);
		}
	}

	public void SetBomb () {
		StartCoroutine (BombSpawn ());
	}

	public void SpawnBomb (Vector3 position) {
		items += 1;
		GameObject item = Instantiate (Resources.Load ("Prefabs/Items/bomb"), position, Quaternion.identity) as GameObject;
	}

	IEnumerator BombSpawn() {
		yield return new WaitForSeconds (1f);
		int rndStart = Random.Range (-2, 3);
		Vector3 spawnPosition = new Vector3 (Random.Range (-2, 3), 0, Random.Range (-2, 3));
		if (PhotonNetwork.isMasterClient) {
			photonGameItemManager.SpawnBomb (spawnPosition);
		}
	}

}
