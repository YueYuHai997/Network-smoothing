using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;



public class CCTT : MonoBehaviour
{
    private Rigidbody rg;

    public Vector3 Speed;
    private Vector3 oldSpeed;
    public Vector3 account;

    public float Force;

    public bool auto = true; // 自动模式
    
    
    public float angularSpeed = 30f; // 角速度
    public Vector3 centerPoint = Vector3.zero; // 圆周运动的中心点
    
    private Vector3 velocity = new Vector3(0,0,20); // 物体的初始速度

    async void Start()
    {
        rg = this.GetComponent<Rigidbody>();


        if (auto)
        {
            // 计算初始速度，使其垂直于从中心点到物体的向量
            Vector3 toCenter = (transform.position - centerPoint) ;
            velocity = Quaternion.Euler(0, 90, 0) * toCenter.normalized * angularSpeed;
            rg.velocity = velocity;

            //await Task.Delay(3000);
            //auto = false;
        }
    }
    
    private float weight;

    void FixedUpdate()
    {
        if (auto)
        {
            // 每个物理更新，计算新的速度方向，保持速度大小不变
            Vector3 toCenter = transform.position - centerPoint;
            velocity = Quaternion.Euler(0, 90, 0) * toCenter.normalized * angularSpeed;
            rg.velocity = velocity;
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            weight = 2;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            weight = 1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            rg.AddForce(new Vector3(0, 0, 1) * (Force * weight), ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.S))
        {
            rg.AddForce(new Vector3(0, 0, -1) * (Force * weight), ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.A))
        {
            rg.AddForce(new Vector3(-1, 0, 0) * (Force * weight), ForceMode.Acceleration);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rg.AddForce(new Vector3(1, 0, 0) * (Force * weight), ForceMode.Acceleration);
        }

        Debug.DrawLine(this.transform.position, this.transform.position + Speed, Color.green);
        Debug.DrawLine(this.transform.position, this.transform.position + account, Color.red);


        Speed = rg.velocity;
        account = (Speed - oldSpeed) / Time.fixedDeltaTime;
        oldSpeed = Speed;
    }

    // Update is called once per frame
    void Update()
    {
    }
}