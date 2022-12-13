using UnityEngine;

public class CameraController : MonoBehaviour
{
    public RectTransform _imageRect;
    private float targetZoom = 1;
    private float zoom = 1;

    // Update is called once per frame
    void Update()
    {
        targetZoom += Input.mouseScrollDelta.y;
        targetZoom = Mathf.Clamp(targetZoom, 1f, 15f);

        if (Mathf.Abs(zoom - targetZoom) > 0.01f)
        {
            zoom = Mathf.Lerp(zoom, targetZoom, 5 * Time.deltaTime);
            var scale = new Vector3(zoom, zoom, 1);
            _imageRect.localScale = scale;
        }        
    }
}
