using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject VFX;
    public float moveSpeed = 20f;
    public int damage = 5;

    private float lifeTime = 10f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.ApplyDamage(damage);

            Vector3 hitPoint = other.ClosestPoint(transform.position);
            DamagePopupManager.Instance.ShowDamage(hitPoint, damage);

            VFX.SetActive(false);
        }
    }
}
