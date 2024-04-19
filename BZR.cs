using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BZR : MonoBehaviour
{
    public Transform startPoint;
    public Transform controlPoint1;
    public Transform controlPoint2;
    public Transform endPoint;

    [Range(0,1)]
    public float index;
    
    // 计算贝塞尔曲线上的点
    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        // 贝塞尔曲线公式
        Vector3 point = uuu * p0; // 第一项
        point += 3 * uu * t * p1; // 第二项
        point += 3 * u * tt * p2; // 第三项
        point += ttt * p3; // 第四项

        return point;
    }

    void Update()
    {
        // t是从0到1的参数，表示曲线上的位置
        float t = Mathf.Clamp01(Time.timeSinceLevelLoad / 5.0f); // 5秒内从0变化到1

        // 计算当前时间t对应的贝塞尔曲线上的点
        Vector3 position = CalculateBezierPoint(index, startPoint.position, controlPoint1.position, controlPoint2.position,
            endPoint.position);

        // 移动物体到贝塞尔曲线上的点
        transform.position = position;
    }
}
