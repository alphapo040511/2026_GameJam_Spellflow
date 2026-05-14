using UnityEngine;

public class DamagePopupManager : MonoBehaviour
{
    public static DamagePopupManager Instance;

    public Canvas canvas;
    public Camera mainCamera;
    public DamagePopup popupPrefab;

    void Awake()
    {
        Instance = this;
    }

    public void ShowDamage(Vector3 worldPos, float damage)
    {
        Vector2 screenPos = mainCamera.WorldToScreenPoint(worldPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
            out Vector2 localPos
        );

        DamagePopup popup = Instantiate(popupPrefab, canvas.transform);
        popup.GetComponent<RectTransform>().anchoredPosition = localPos;

        popup.Setup(damage);
    }
}