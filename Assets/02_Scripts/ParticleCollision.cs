using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
   public ParticleSystem particleLauncher;
    public ParticleSystem splatter;
    public float speed=0.1f;
    List<ParticleCollisionEvent> collisionEvent;
    public Gradient particleGradient;
    private bool isMouseDown = false;
    public float rotatespeed = 10.0f;
    float XAngle;
    float YAngle;
    float XAngleTemp;
    float YAngleTemp;

    // Start is called before the first frame update
    void Start()
    {
        XAngle = 0;
        YAngle = 50;
        collisionEvent = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particleLauncher, other, collisionEvent);
        Debug.Log("파티클 충돌이 일어났습니다");
        for(int i = 0; i < collisionEvent.Count; i++) {
            EmitAtLocation(collisionEvent[i]);
        }
        

    }
    void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent)
    {
        splatter.transform.position = particleCollisionEvent.intersection;
        Debug.Log(splatter.transform.position);
        
        Debug.Log(splatter.transform.position);
        splatter.transform.rotation = Quaternion.LookRotation(particleCollisionEvent.normal);

        ParticleSystem.MainModule psMain = splatter.main;
        
        splatter.Emit(5);
    }
  

    private void Update()
    {
       if (Input.GetMouseButtonDown(0))
        {
            ParticleSystem.MainModule psmain = particleLauncher.main;
            particleLauncher.Emit(1);
            isMouseDown = true;
        }
       if(Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;

        }
       if(isMouseDown)
        {
            ParticleSystem.MainModule psmain = particleLauncher.main;
            particleLauncher.Emit(1);
            float temp_x_axis = Input.GetAxis("Mouse X") * rotatespeed * Time.deltaTime;
            float temp_y_axis = Input.GetAxis("Mouse Y") * rotatespeed * Time.deltaTime;
            particleLauncher.transform.Rotate(temp_y_axis, -temp_x_axis, 0, Space.World);

        }
     
        if (Input.GetKey(KeyCode.W))
        {
            particleLauncher.transform.Rotate(Vector3.forward * -rotatespeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {

            Debug.Log(particleLauncher.transform.rotation.x+","+ particleLauncher.transform.rotation.y+","+ particleLauncher.transform.rotation.z * -rotatespeed * Time.deltaTime);
            particleLauncher.transform.Rotate(Vector3.back * -rotatespeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            particleLauncher.transform.Rotate(Vector3.left * -rotatespeed * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.D))
        {
            particleLauncher.transform.Rotate(Vector3.right * -rotatespeed * Time.deltaTime);
        }



    }

}
