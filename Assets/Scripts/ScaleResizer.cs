using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleResizer : MonoBehaviour
{
    public float referenceHeight = 1080f; // 기준 화면 높이 (픽셀)

    void Start() {
        ScaleToCamera();
    }

    void ScaleToCamera()
    {
        float currentHeight = Screen.height;
        float scaleRatio = Mathf.Min(currentHeight / referenceHeight, 1f);

        float xRatio = transform.localScale.x * scaleRatio;
        float yRatio = transform.localScale.y * scaleRatio;

        // 유닛의 스프라이트 크기 조정
        transform.localScale = new Vector2(xRatio, yRatio);
    }
}
