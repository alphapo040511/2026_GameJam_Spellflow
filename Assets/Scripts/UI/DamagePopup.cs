using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float moveUpSpeed = 50f;
    public float duration = 1f;

    RectTransform rect;
    CanvasGroup canvasGroup;

    Vector3 moveDir = new Vector3(0, 1, 0);

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void Setup(float damage)
    {
        text.text = damage.ToString();
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            rect.anchoredPosition += (Vector2)(moveDir * moveUpSpeed * Time.deltaTime);

            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / duration);

            yield return null;
        }

        Destroy(gameObject);
    }
}