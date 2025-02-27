using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    private Transform enemy;
    private RectTransform rectTransform;
    private Image hpFillImage; 

    public void Initialize(Transform enemyTransform)
    {
        enemy = enemyTransform;
        rectTransform = GetComponentInChildren<RectTransform>();
        hpFillImage = transform.Find("HP").GetComponentInChildren<Image>(); 

        transform.SetParent(enemy);
        transform.localPosition = new Vector3(0, 0.3f, 0);
    }

    public void UpdateHP(float currentHP, float maxHP)
    {
        if (hpFillImage != null)
        {
            hpFillImage.fillAmount = Mathf.Clamp(currentHP / maxHP, 0, 1);
        }
    }
}
