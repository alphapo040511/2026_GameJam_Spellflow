using UnityEngine;

public class Flame : MonoBehaviour
{
    public int tickDamage = 2;
    public float interval = 0.3f;


    float lastDamageTime;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Time.time < lastDamageTime + interval) return;

            lastDamageTime = Time.time;



            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
                damageable.ApplyDamage(tickDamage);
        }

    }
}
