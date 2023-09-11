using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MenuFade : MonoBehaviour {
    private float timer = 0.0f;
    private RectTransform rect;

    private void OnEnable() {
        rect = GetComponent<RectTransform>();

        rect.localScale = new Vector3(0, 0, 0);
    }

    private void OnDisable() {
        timer = 0.0f;
    }

    private void Update() {
        timer += Time.unscaledDeltaTime;

        if(timer < 0.25f) {
            rect.localScale = new Vector3(timer * 4, timer * 4, timer * 4);
        } else if(timer > 0.25f) {
            rect.localScale = new Vector3(1, 1, 1);
        }
    }
}