using UnityEngine;
using System.Collections;

public class PhotonNetworkCharacter : Photon.MonoBehaviour {
	PhotonView photonView;
	CharacterController player;
	string myPosition;
	string movePosition;

	void Start () {
		photonView = GetComponent<PhotonView> ();
		player = GetComponent<CharacterController> ();
	}

	void OnGUI ()
	{
		GUILayout.BeginArea (new Rect (Screen.width - 250, 10, Screen.width, Screen.height));
//		GUILayout.Label("My :: " + myPosition, GUILayout.Width(100));
//		GUILayout.Label("Move :: " + movePosition, GUILayout.Width(100));
//		if (GUILayout.Button ("Emotion", GUILayout.Width (100))) {
//			photonCharacter.Emotion (CommonConfig.CHARACTER_EMOTION_HAPPY);
//		}

		GUILayout.EndArea ();
	}

	public void Move (int viewId, Vector3 position) {
		photonView.RPC ("RPCMove", PhotonTargets.All, viewId, position);
	}

	public void MyLocation (int viewId, Vector3 position) {
		myPosition = position.ToString ();
		photonView.RPC ("RPCMyPosition", PhotonTargets.All, viewId, position);		
	}

	public void UpdateScore1 (int score) {
		photonView.RPC ("RPCUpdateScore1", PhotonTargets.All, score);		
	}

	public void UpdateScore2 (int score) {
		photonView.RPC ("RPCUpdateScore2", PhotonTargets.All, score);		
	}

	public void UpdateScore3 (int score) {
		photonView.RPC ("RPCUpdateScore3", PhotonTargets.All, score);		
	}

	public void UpdateScore4 (int score) {
		photonView.RPC ("RPCUpdateScore4", PhotonTargets.All, score);		
	}

	[PunRPC]
	public void RPCUpdateScore1 (int score) {
		player.UpdateScore1 (score);
	}

	[PunRPC]
	public void RPCUpdateScore2 (int score) {
		player.UpdateScore2 (score);
	}

	[PunRPC]
	public void RPCUpdateScore3 (int score) {
		player.UpdateScore3 (score);
	}

	[PunRPC]
	public void RPCUpdateScore4 (int score) {
		player.UpdateScore4 (score);
	}

	[PunRPC]
	public void RPCMyPosition (int viewId, Vector3 position) {
		Debug.Log ("RPC My position :: " + position);
		player.UpdateLocation (viewId, position);
	}

	[PunRPC]
	public void RPCMove (int viewId, Vector3 position) {
		Debug.Log ("RPCMove :: " + viewId + ", " + position.ToString ());
		movePosition = position.ToString ();
		player.OnMove (position);
	}
}