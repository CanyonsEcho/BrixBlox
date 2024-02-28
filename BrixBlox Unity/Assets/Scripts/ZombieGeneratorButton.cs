using UnityEngine;

public class ZombieGeneratorButton : MonoBehaviour
{
    public Material green;
    public GameObject gen;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            meshRenderer.material = green;
            gen.SetActive(false);
        }
    }
}
