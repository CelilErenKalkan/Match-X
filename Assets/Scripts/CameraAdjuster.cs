using UnityEngine;

public static class CameraAdjuster
{
    public static void FitCameraToRenderers(Camera cam, Renderer[] renderers, float margin = 1.02f)
    {
        if (renderers.Length == 0 || cam == null || cam.orthographic) return;

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
            bounds.Encapsulate(renderers[i].bounds);

        bounds.size *= margin;
        Vector3 center = bounds.center;

        float fov = cam.fieldOfView;
        float aspect = cam.aspect;

        float vFOV = Mathf.Deg2Rad * fov / 2;
        float hFOV = Mathf.Atan(Mathf.Tan(vFOV) * aspect);

        float distV = (bounds.size.y / 2) / Mathf.Tan(vFOV);
        float distH = (bounds.size.x / 2) / Mathf.Tan(hFOV);

        cam.transform.position = new Vector3(center.x, center.y, center.z - Mathf.Max(distV, distH));
    }
}