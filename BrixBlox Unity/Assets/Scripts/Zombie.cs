using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float rotationSpeed = 2f;
    public bool allowedToMove = true;
    public GameObject sword;

    private Transform playerTransform;
    private Rigidbody botRigidbody;
    private MeshRenderer swordMeshRenderer;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player not found!");
        }

        sword = GameObject.FindGameObjectWithTag("Sword");
        swordMeshRenderer = sword.GetComponent<MeshRenderer>();

        if (sword == null)
        {
            Debug.LogError("Sword not found!");
        }

        botRigidbody = GetComponent<Rigidbody>();
        if (botRigidbody == null)
        {
            Debug.LogError("Rigidbody not found on the zombie object!");
        }
    }

    void Update()
    {
        if (playerTransform != null && allowedToMove)
        {
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            directionToPlayer.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            botRigidbody.velocity = directionToPlayer.normalized * moveSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sword") && Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject);
        }
    }
}
