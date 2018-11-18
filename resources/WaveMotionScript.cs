using UnityEngine;
using System.Collections;

public class WaveMotionScript : MonoBehaviour {

    //Clamping movement between postive and negative range of values
    public float xRange = 0.5f;
    public float yRange = 0.5f;

    public bool clockWise = true;

    //The speed of movement (a coefficient for time.deltatime)
    public float speedMod = 1.0f;

    float tempTime;

    Vector3 basePosition;

    //Angle offset
    public float offSet;

    public float rotationRange;
    public bool doesRotate;

	// Use this for initialization
	void Start () {

        //clockWise = true;

        //xRange = 0.5f;
        //yRange = 0.5f;

        //speedMod = 1.0f;

        //basePosition = transform.position;
        basePosition = transform.localPosition;

	}
	
    void FixedUpdate() {


        
        //tempTime = Mathf.Sin(Time.time * speedMod);
        if(clockWise)
        {
            if(GetComponent<Rigidbody2D>() != null)
            {
                GetComponent<Rigidbody2D>().MovePosition(new Vector2(basePosition.x + xRange * (Mathf.Sin(Time.time * speedMod + offSet)), basePosition.y + yRange * (Mathf.Cos(Time.time * speedMod + offSet))));
                if(doesRotate)
                {
                    GetComponent<Rigidbody2D>().MoveRotation(Mathf.Rad2Deg * Mathf.Cos(Time.time * speedMod + offSet) * rotationRange);
                }
            }
            else
            {
                transform.localPosition = new Vector3(basePosition.x + xRange * (Mathf.Sin(Time.time * speedMod + offSet)), basePosition.y + yRange * (Mathf.Cos(Time.time * speedMod + offSet)), basePosition.z);
                if (doesRotate)
                {
                    transform.localRotation = new Quaternion(0.0f, 0.0f, (Mathf.Cos(Time.time * speedMod + offSet) * rotationRange), 1f);
                }
            }
            
        }
        else
        {
            if(GetComponent<Rigidbody2D>() != null)
            {
                GetComponent<Rigidbody2D>().MovePosition(new Vector2(basePosition.x + xRange * (Mathf.Cos(Time.time * speedMod + offSet)), basePosition.y + yRange * (Mathf.Cos(Time.time * speedMod + offSet))));
                if (doesRotate)
                {
                    GetComponent<Rigidbody2D>().MoveRotation(Mathf.Rad2Deg * Mathf.Sin(Time.time * speedMod + offSet) * rotationRange);
                }
            }
            else
            {
                transform.localPosition = new Vector3(basePosition.x + xRange * (Mathf.Cos(Time.time * speedMod + offSet)), basePosition.y + yRange * (Mathf.Sin(Time.time * speedMod + offSet)), basePosition.z);
                if (doesRotate)
                {
                    transform.localRotation = new Quaternion(0.0f, 0.0f, (Mathf.Sin(Time.time * speedMod + offSet) * rotationRange), 1f);
                }
            }
            
        }
        

    }

	// Update is called once per frame
	void Update () {
	
	}
}
