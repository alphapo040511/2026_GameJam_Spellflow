using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float damage = 5;

    private float lifeTime = 10f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }
}
