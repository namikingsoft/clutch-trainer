using System.Collections;
using UnityEngine;

public class TransformShaker
{
    private Transform transform;
    private bool isShaking = false;

    public TransformShaker(Transform transform)
    {
        this.transform = transform;
    }

    public IEnumerator Do(float duration, float magnitude = 1)
    {
        if (isShaking) yield break;
        isShaking = true;

        // refs. http://baba-s.hatenablog.com/entry/2018/03/14/170400
        Vector3 pos = transform.localPosition;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, pos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = pos;

        isShaking = false;
    }
}
