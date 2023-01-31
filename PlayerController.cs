using UnityEngine;

// Include the namespace required to use Unity UI and Input System
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour {

	// Create public variables for player speed, and for the Text UI game objects
	public float speed;
	public TextMeshProUGUI countText;
	public GameObject winTextObject;
	public Vector3 jump;
  public float jumpForce = 2.0f;
	public int yPos;
  public bool isGrounded;

  private float movementX;
  private float movementY;

	private Rigidbody rb;
	private int count;
	private Vector3 spawn;
	private Vector3 player;

	// At the start of the game..
	void Start ()
	{
		spawn = new Vector3(0,1,0);
		rb = GetComponent<Rigidbody>();
		jump = new Vector3(0.0f, 2.0f, 0.0f);
		count = 0;

		SetCountText ();

    // Set the text property of the Win Text UI to an empty string, making the 'You Win' (game over message) blank
    winTextObject.SetActive(false);
	}

	void OnCollisionStay()
	{
  	isGrounded = true;
  }
  void OnCollisionExit()
	{
  	isGrounded = false;
  }

	void FixedUpdate ()
	{
		Vector3 movement = new Vector3 (movementX, 0.0f, movementY);

		rb.AddForce (movement * speed);

		if(Input.GetKey("space") && isGrounded){

    	rb.AddForce(jump * jumpForce * Time.deltaTime *2, ForceMode.Impulse);
      isGrounded = false;
		}

		if (yPos <= -100)
		{
			transform.position = spawn;
		}
	}
	void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.CompareTag ("kill"))
		{
			transform.position = spawn;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}

		if (other.gameObject.CompareTag ("checkpoint"))
		{
			spawn = new Vector3 (675,1,0);
			transform.position = spawn;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}

		if (other.gameObject.CompareTag ("finish"))
		{
			if (count < 11)
			{
				spawn = new Vector3 (0,1,0);
				transform.position = spawn;
				GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
		}

		if (other.gameObject.CompareTag ("Jump"))
		{
			rb.AddForce(Vector3.up * 850);
			isGrounded = false;
		}
		// ..and if the GameObject you intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag ("PickUp"))
		{
			other.gameObject.SetActive (false);

			// Add one to the score variable 'count'
			count = count + 1;

			// Run the 'SetCountText()' function (see below)
			SetCountText ();
		}
	}

  void OnMove(InputValue value)
  {
		Vector2 v = value.Get<Vector2>();

    movementX = v.x;
  	movementY = v.y;
  }

  void SetCountText()
	{
		countText.text = "Count: " + count.ToString();

		if (count >= 11)
		{
			winTextObject.SetActive(true);
			spawn = new Vector3 (861,1,-13);
			transform.position = spawn;
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}
}
