using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
	private Rigidbody2D rb;
	public UIDocument uiDocument;
	private ProgressBar EnergyBar;
	private ProgressBar HungerBar;

	// Sounds
	/*
	[field: SerializeField]
	public UnityEvent onEat { get; set; }
	[field: SerializeField]
	public UnityEvent onCollect { get; set; }
	[field: SerializeField]
	public UnityEvent onDeath { get; set; }
	*/

	[field: SerializeField]
	public float speed { get; set; } = 10.0f;
	[field: SerializeField]
	public int energy { get; set; } = 100; 
	[field: SerializeField]
	public int hunger { get; set; } = 100;
	

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		// Alternatively GetCompenentInParent if the Rigidbody is in a child object
		// GetComponent must be placed on the Monster folder
		rb = GetComponent<Rigidbody2D>();
		EnergyBar = uiDocument.rootVisualElement.Q<ProgressBar>("EnergyBar");
		HungerBar = uiDocument.rootVisualElement.Q<ProgressBar>("HungerBar");
	}

	void Update()
	{
		Vector2 move = new Vector2();

		if (Input.GetKey(KeyCode.W))
		{
			move.y += speed;
		}
		if (Input.GetKey(KeyCode.A))
		{
			move.x -= speed;
		}
		if (Input.GetKey(KeyCode.S))
		{
			move.y -= speed;
		}
		if (Input.GetKey(KeyCode.D))
		{
			move.x += speed;
		}

		rb.MovePosition(rb.position + (move * Time.deltaTime));
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		//Debug.Log("Collision detected with: " + collision.gameObject.name);
		if (collision.gameObject.CompareTag("Food"))
		{
			//onEat?.Invoke();
			hunger += 1; // Increase hunger by 5 when colliding with food
			Debug.Log("Player hunger: " + hunger);
			HungerBar.value = hunger; // Update the hunger bar UI
			Destroy(collision.gameObject); // Remove the food from the scene
		}
		else if (collision.gameObject.CompareTag("Item"))
		{ // Needs to be rewritten to support an inventory system for collectible items
			//onCollect?.Invoke();
			Destroy(collision.gameObject); // Remove the treasure from the scene
		}
		else if (collision.gameObject.CompareTag("Checkpoint"))
		{ // Checkpoints save progress and trigger sleep to restore energy
			//onCollect?.Invoke();
			energy = 100; // Fully restores energy
			if (hunger < 20)
			{ // If hunger is above 20 pts, decreases hunger bar by 10 pts when resting at a checkpoint
				hunger -= 10; 
			}
			Debug.Log("Player energy: " + energy);
			Debug.Log("Player hunger: " + hunger);
			EnergyBar.value = energy; // Update the energy bar UI
			HungerBar.value = hunger; // Update the hunger bar UI
		}
		else if (collision.gameObject.CompareTag("Enemy"))
		{ // Implement respawn at last checkpoint
			//onDeath?.Invoke();
			Debug.Log("Player has been knocked out");
			// "You narrowly managed to escape to your last safe resting spot"
		}
	}
}
