using System.Collections;
using UnityEngine;
using DG.Tweening;


public class MMGR : MonoBehaviour
{
    public GameObject Move; //主动移动
    public GameObject Tag_NO;
    public GameObject Tag_Line;
    public GameObject Tag_Three; //跟随移动

    public Camera Cam;

    public Vector3 Offset;

    public float UpdateTime = 10;
    private int record = 0;


    public Vector3 StartVelocity; //记录 跟随物体的速度 
    private float timeToReachEnd; // 到达终点的预计时间

    private Vector3 coord0, coord1, coord2, coord3;
    private float A, B, C, D, E, F, G, H, I, J, K, L;

    private CCTT MoveCCTT; //获取主动移动物体的速度与加速度 



    private Vector3 CalculateBezierPoint(float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        // 贝塞尔曲线公式
        Vector3 point = uuu * coord0; // 第一项
        point += 3 * uu * t * coord1; // 第二项
        point += 3 * u * tt * coord2; // 第三项
        point += ttt * coord3; // 第四项

        return point;
    }

    private void Awake()
    {
        MoveCCTT = Move.GetComponent<CCTT>();
        
        Application.targetFrameRate = 120;
    }


    void Update()
    {
        timeToReachEnd = UpdateTime / 120;
        
        //Cam.orthographicSize = MoveCCTT.Speed.magnitude;
        
        record++;
        if (record >= UpdateTime)
        {
            record = 0;
            SetTagNo(Move.transform.position);
            SetTagLine(Move.transform.position);
            StartCoroutine(nameof(SetTagThree));
        }
       
    }

    void SetTagNo(Vector3 pos)
    {
        Tag_NO.transform.position = pos + Offset;
    }

    void SetTagLine(Vector3 pos)
    {
        Tag_Line.transform.DOMove(pos + Offset * 2, Time.deltaTime * UpdateTime);
    }

    IEnumerator SetTagThree()
    {
        CalculateSplineCoefficients();
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / timeToReachEnd;
            Vector3 newPostion = CalculateBezierPoint(t);
            Tag_Three.transform.position = newPostion + Offset * 3;
            yield return null;
        }
    }

    public float Muti = 1;

    public float weight1;

    public float weight2;

    // 计算样条曲线的系数
    void CalculateSplineCoefficients()
    {
        // coord0 = Tag_Three.transform.position - Offset * 3;
        // coord1 = coord0 + StartVelocity * UpdateTime * Time.deltaTime /3f; //UpdateTime =60时/3 UpdateTime =30 时 /30
        //
        // coord3 = Move.transform.position;
        // coord2 = coord3 - MoveCCTT.Speed * UpdateTime * Time.deltaTime / 3f;


        //状态预测参数
         coord0 = Tag_Three.transform.position - Offset * 3;
         coord1 = coord0 + StartVelocity * UpdateTime * Time.deltaTime / 3f;
             ; //需要根据UpdateTime修改
        
         coord3 = Move.transform.position + MoveCCTT.Speed * UpdateTime * Time.deltaTime +
                  MoveCCTT.account * UpdateTime * Time.deltaTime * UpdateTime * Time.deltaTime / 2f;
        
         coord2 = coord3 - (MoveCCTT.Speed + MoveCCTT.account * UpdateTime * Time.deltaTime) * UpdateTime *
             Time.deltaTime / 3f;
        
        StartVelocity = MoveCCTT.Speed;
    }
    
}