using UnityEngine;
using System;

public static class CameraExtensions
{
    private static Rect _bounds;

    public static UnityEngine.Rect OrthographicBounds(this Camera camera)
    {

        return _bounds;
    }

    public static void UpdateOrthographicBounds(this Camera camera)
    {
        float height = 2.0f * camera.orthographicSize;
        float width = height * camera.aspect;
        float x = camera.transform.position.x;
        float y = camera.transform.position.y;

        _bounds = new UnityEngine.Rect(x - (width / 2.0f), y - (height / 2.0f), width, height);
    }

}
