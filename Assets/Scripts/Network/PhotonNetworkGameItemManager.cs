using UnityEngine;
using System.Collections;

public class PhotonNetworkGameItemManager : MonoBehaviour {
	PhotonView photonView;
	GameItemManager gameItemManager;

	void Start () {
		photonView = GetComponent<PhotonView> ();
		gameItemManager = GetComponent<GameItemManager> ();
	}

	public void SpawnCoin (Vector3 position) {
		photonView.RPC ("RCPSpawnCoin", PhotonTargets.All, position);
	}

	public void SpawnBomb (Vector3 position) {
		photonView.RPC ("RCPSpawnBomb", PhotonTargets.All, position);
	}

	[PunRPC]
	public void RCPSpawnCoin (Vector3 position) {
		Debug.Log ("SpawnCoin at " + position.ToString());
		gameItemManager.SpawnCoin (position);
	}

	[PunRPC]
	public void RCPSpawnBomb (Vector3 position) {
		Debug.Log ("SpawnBomb at " + position.ToString());
		gameItemManager.SpawnBomb (position);
	}
}
