using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject bulletDecal;

    private float spped = 50f;
    private float lifeTime = 3f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    private void OnEnable() => Destroy(gameObject, lifeTime);

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, spped * Time.deltaTime);
        if (!hit && Vector3.Distance(transform.position, target) < 0.01f)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player")) return;
        var contact = other.GetContact(0);
        Instantiate(bulletDecal, contact.point + contact.normal * 0.0001f, Quaternion.LookRotation(contact.normal));
        Destroy(gameObject);
    }
}