using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{
	private Vector3 endPoint;
	public float duration = 0.1f;
	private float yAxis;
	private Quaternion rotateTo;

	public int score = 0;
	public bool isStunt = false;

	public PhotonView photonView;
	public PhotonNetworkCharacter photonCharacter;
	public Vector3 destinationPosition;
	public Vector3 currentPosition;
	public bool moveFlag = false;
	public Animator animator;

	void Start ()
	{
		// save the y axis value of gameobject
		yAxis = gameObject.transform.position.y;
		photonView = GetComponent<PhotonView> ();
		photonCharacter = GetComponent<PhotonNetworkCharacter> ();
		animator = GetComponent<Animator> ();
		Prepare ();

		Debug.Log ("1. Prepare :: " + photonView.viewID);
	}

	void FaceOff (Vector3 direction)
	{
		Vector3 relativePos = direction - transform.position;
		rotateTo = Quaternion.LookRotation (relativePos);
	}

	void Prepare ()
	{
		//Update character when joined game
	}

	// Update is called once per frame
	void Update ()
	{

		// check if the screen is touched / clicked   
		if ((Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began) || (Input.GetMouseButtonDown (0))) {
			// declare a variable of RaycastHit struct
			RaycastHit hit;
			// Create a Ray on the tapped / clicked position
			Ray ray;
			// for unity editor
			#if UNITY_EDITOR
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			// for touch device
			#elif (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8)
			ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			#else
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			#endif

			// Check if the ray hits any collider
			if (Physics.Raycast (ray, out hit)) {
				// set a flag to indicate to move the gameobject
//				moveFlag = true;
				// save the click / tap position
				endPoint = hit.point;
				// as we do not want to change the y axis value based on touch position, reset it to original y axis value
				endPoint.y = yAxis;
				
				Debug.Log ("Hit :: " + hit.collider.name);
				Debug.Log ("Direction: " + endPoint);
				Debug.Log ("Score is: " + score);
				Debug.Log ("Stunt is: " + isStunt);
				
//				destinationPosition = endPoint;
				if (photonView.isMine) {
					photonCharacter.Move (photonView.viewID, endPoint);
				}
			}

		}

		transform.rotation = Quaternion.Slerp (transform.rotation, rotateTo, Time.deltaTime * 20);

		if (moveFlag) {
			Debug.Log ("Start Moving :: " + moveFlag);
			MovePosition ();
		} else {
			return;
		}

	}

	void MovePosition ()
	{
		FaceOff (destinationPosition);

		if (!isStunt && !Mathf.Approximately (gameObject.transform.position.magnitude, destinationPosition.magnitude)) {
			gameObject.transform.position = Vector3.Lerp (
				gameObject.transform.position, 
				destinationPosition, 
				1 / (duration * (Vector3.Distance (gameObject.transform.position, destinationPosition)))
			);
		} else if (Mathf.Approximately (gameObject.transform.position.magnitude, destinationPosition.magnitude)) {
			animator.SetBool ("isRunning", false);
			moveFlag = false;
			Debug.Log ("Moved Here!");
		}
	}

	public void UpdateLocation (int id, Vector3 position)
	{
		currentPosition = position;
	}

	public void OnMove (Vector3 futureTransform)
	{
		Debug.Log ("Position :: " + futureTransform);
		Debug.Log ("Stunt :: " + isStunt);
		animator.SetBool ("isRunning", true);
		destinationPosition = futureTransform;
		photonCharacter.MyLocation (photonView.viewID, futureTransform);

		moveFlag = true;
	}

	public void OnEmotion ()
	{
	}

	void OnTriggerEnter (Collider other)
	{
		Debug.Log ("OnTriggerEnter");
		if (other.gameObject.CompareTag ("Coin")) {
			other.gameObject.SetActive (false);
			Destroy (other.gameObject);
			score = score + 1;
			animator.SetTrigger ("isTake");
		} else if (other.gameObject.CompareTag ("Bomb")) {
			StartCoroutine (spawnExplosion ());
			animator.SetTrigger ("isBomb");
			other.gameObject.SetActive (false);
			Destroy (other.gameObject);
			if (!isStunt) {
				isStunt = true;
				StartCoroutine (removeStuntAfterTime (1.5f));
			}
		}
	}


	IEnumerator removeStuntAfterTime (float time)
	{
		yield return new WaitForSeconds (time);
		isStunt = false;
	}
	
	IEnumerator spawnExplosion ()
	{
		GameObject item = Instantiate (Resources.Load ("Prefabs/Explosion"), transform.position, Quaternion.identity) as GameObject;
		yield return new WaitForSeconds (0.2f);
		Destroy (item);
	}

		
}
