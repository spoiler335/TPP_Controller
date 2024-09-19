using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private BulletController bulletPrefab;
    [SerializeField] private Transform barreltransform;
    [SerializeField] private Transform bulletParent;
    private float bulletMaxAirDistance = 25f;
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (DI.di.input.isFireClicked) Shoot();
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, barreltransform.position, Quaternion.identity, bulletParent);

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.tag == "Player")
            {
                Destroy(bullet);
                return;
            }
            Debug.Log($"Fired At {hit.collider.name}");
            bullet.target = hit.point;
            bullet.hit = true;
        }
        else
        {
            bullet.target = cameraTransform.position + cameraTransform.forward * bulletMaxAirDistance;
            bullet.hit = false;
        }

    }
}