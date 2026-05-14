using UnityEngine;

public class TopDown : MonoBehaviour
{
    public Collider skillCollider;
    public int tickDamage = 5;
    public float interval = 1f;

    private float lifeTime = 7f;

    float delay = 1;

    float lastDamageTime;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        delay -= Time.deltaTime;
        if (delay <= 0)
        {
            skillCollider.enabled = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (Time.time < lastDamageTime + interval) return;

            lastDamageTime = Time.time;



            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.ApplyDamage(tickDamage);


            Vector3 hitPoint = other.ClosestPoint(transform.position);
            DamagePopupManager.Instance.ShowDamage(hitPoint, tickDamage);
        }
        
    }
}
