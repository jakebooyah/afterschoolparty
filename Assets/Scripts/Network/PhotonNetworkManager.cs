using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PhotonNetworkManager : Photon.PunBehaviour {
	public PhotonView myPhotonView;
	public Transform[] spawns;
	public GameObject panel;
	public GameObject scorePanel;
	public GameObject[] players;

	void Start () {
	}

	void SetActive (bool status) {
		panel.SetActive (!status);
		scorePanel.SetActive (status);
	}

	void OnGUI () {
		GUILayout.BeginArea (new Rect (10, 10, Screen.width / 2, Screen.height / 2));
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());

		GUILayout.EndArea ();
	}

	public void ConnectToPhotonNetwork ()
	{
		Debug.Log ("ConnectToPhotonNetwork");

		PhotonNetwork.ConnectUsingSettings("0.1");
		PhotonNetwork.autoJoinLobby = true;
	}

	public override void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby");
		PhotonNetwork.JoinRandomRoom();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("OnConnectedToMaster");
		// when AutoJoinLobby is off, this method gets called when PUN finished the connection (instead of OnJoinedLobby())
		PhotonNetwork.JoinRandomRoom();
	}

	public void OnPhotonRandomJoinFailed()
	{
		Debug.Log("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom("LevelUp");
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom :: " + PhotonNetwork.connected);
		SetActive (PhotonNetwork.connected);
			
		GameObject character = PhotonNetwork.Instantiate ("Prefabs/Character/Kids-" + Random.Range(1, 5), spawns[0].position, Quaternion.identity, 0);
		myPhotonView = character.GetComponent<PhotonView>();

		//		monster.GetComponent<myThirdPersonController>().isControllable = true;

	}
}
