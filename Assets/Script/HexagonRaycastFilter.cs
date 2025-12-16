using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HexagonRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
{
    [Header("Hexagon Settings")]
    [SerializeField] bool _pointyTop = false;

    RectTransform _buttonRectTransform;

    void Awake()
    {
        _buttonRectTransform = GetComponent<RectTransform>();
    }
    
    public bool IsRaycastLocationValid(Vector2 screenPoint, Camera camera)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_buttonRectTransform, screenPoint, camera, out localPoint);

        return IsPointInsideHexagon(localPoint);
    }

    bool IsPointInsideHexagon(Vector2 localPoint)
    {
        Vector2[] hexLocalPoints = GetHexLocalPoints(_buttonRectTransform.rect);

        bool isInside = false;
        for (int i = 0, j = hexLocalPoints.Length - 1; i < hexLocalPoints.Length; j = i++)
        {
            if (((hexLocalPoints[i].y > localPoint.y) != (hexLocalPoints[j].y > localPoint.y)) && (localPoint.x < (hexLocalPoints[j].x - hexLocalPoints[i].x) * (localPoint.y - hexLocalPoints[i].y) / (hexLocalPoints[j].y - hexLocalPoints[i].y) + hexLocalPoints[i].x))
            {
                isInside = !isInside;
            }
        }
        return isInside;
    }
    
    Vector2[] GetHexLocalPoints(Rect rectTransform)
    {
        float width = rectTransform.width * 0.5f;
        float height = rectTransform.height * 0.5f;

        if (_pointyTop)
        {
            return new Vector2[]
            {
                new Vector2( 0,  height),
                new Vector2( width * 0.8660254f,  height * 0.5f),
                new Vector2( width * 0.8660254f, -height * 0.5f),
                new Vector2( 0, -height),
                new Vector2(-width * 0.8660254f, -height * 0.5f),
                new Vector2(-width * 0.8660254f,  height * 0.5f),
            };
        }
        else
        {
            return new Vector2[]
            {
                new Vector2( width,  0),
                new Vector2( width * 0.5f,  height * 0.8660254f),
                new Vector2(-width * 0.5f,  height * 0.8660254f),
                new Vector2(-width,  0),
                new Vector2(-width * 0.5f, -height * 0.8660254f),
                new Vector2( width * 0.5f, -height * 0.8660254f),
            };
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (!rectTransform) return;

        Gizmos.color = Color.cyan;

        Vector2[] localPoints = GetHexLocalPoints(rectTransform.rect);
        Vector3[] worldPoints = new Vector3[localPoints.Length];

        for (int i = 0; i < localPoints.Length; i++)
            worldPoints[i] = rectTransform.TransformPoint(localPoints[i]);

        for (int i = 0; i < worldPoints.Length; i++)
        {
            Gizmos.DrawLine(
                worldPoints[i],
                worldPoints[(i + 1) % worldPoints.Length]
            );
        }
    }
#endif
}
