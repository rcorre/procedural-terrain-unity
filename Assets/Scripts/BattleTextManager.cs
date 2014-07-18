using UnityEngine;
using System.Collections;

public class BattleTextManager : MonoBehaviour {
    public enum TextType {
	Damage,
	Healing
    }

    public Transform DamageText;
    public Transform HealingText;

    public void SpawnText(string text, TextType type, Vector3 worldPosition) {
        var position = Camera.main.WorldToViewportPoint(worldPosition);
        Transform textPrefab = DamageText;
        switch (type) {
            case TextType.Damage:
                textPrefab = DamageText;
                break;
            case TextType.Healing:
                textPrefab = HealingText;
                break;
            default:
                break;
        }
        var popup = (Transform)GameObject.Instantiate(textPrefab, position, Quaternion.identity);
        popup.guiText.text = text;
    }

    public void SpawnText(int value, TextType type, Vector3 worldPosition) {
        SpawnText(value.ToString(), type, worldPosition);
    }
}
