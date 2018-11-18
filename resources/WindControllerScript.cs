using UnityEngine;
using System.Collections;


//This class should both store current wind information,
//Receive input from the player class
//Modify the rotation and appearance of the compass object
//and possibly the appearance of other cosmetic environmental effects
//Let objects update themselves with physics.

public class WindControllerScript : MonoBehaviour {

    public Vector2 windVector;

    //In degrees, from 0-359
    static float windDirection;

    static float windReduction = 0.2F;

    //Scalar to be applied to wind forces acting on rigid bodies.
    public static float windScalar = 150.0F;

    //The maximum possible magnitude of the windVector
    public static float maxMagnitude = 5.0F;

    //Temporary magnitude used in calculations for wind reduction update functions
    float tempMag;

    float getDirFromVect()
    {
        if(windVector.magnitude > 0)
        {
            return (Mathf.Atan2(windVector.y, windVector.x) * Mathf.Rad2Deg) % 360;
        }
        else
        {
            return 0.0F;
        }
    }

    void setCompassRose()
    {
        //Only change the direction if there actually is wind.
        if(windVector.magnitude > 0)
        {
            //Sets compass rose z rotation to current wind angle degrees
            GameObject.FindGameObjectWithTag("CompassRose").transform.rotation = Quaternion.AngleAxis(getDirFromVect(), Vector3.forward);
        }
        
    }

    //Sets the windVector object based on the current joystick position.
    void setWindVector()
    {
        windVector.x = maxMagnitude * Input.GetAxisRaw("Horizontal");
        windVector.y = maxMagnitude * Input.GetAxisRaw("Vertical");

        //This line is important when reducing the wind strength without needing to do Atan conversion every time back and forth.
        windDirection = getDirFromVect();
    }

	// Use this for initialization
	void Start ()
    { 
        //No wind to start
        windVector = Vector2.zero;
        windDirection = 0.0F;

        Debug.Log("Angle should be: " + Mathf.Atan2(windVector.y, windVector.x) * Mathf.Rad2Deg);

        setCompassRose();

	}
	
	// Update is called once per frame
	void Update ()
    { 
        //Wind needs to be reduced.
        if(windVector.magnitude != 0.0f)
        {
            //Get the magnitude now so that modifying the x doesn't affect y calcs
            tempMag = windVector.magnitude;

            //X component update
            if (Mathf.Abs(windVector.x) > 0.1)
            {
                windVector.x -= (windVector.x/tempMag) * windReduction * Time.deltaTime;
            }
            else
            {
                windVector.x = 0.0f;
            }
            
            //Y component update
            if (Mathf.Abs(windVector.y) > 0.1)
            {
                windVector.y -= (windVector.y/tempMag) * windReduction * Time.deltaTime;
            }
            else
            {
                windVector.y = 0.0f;
            }
        }
        else
        {
            //Wind is 0 or has been reduced to 0.
            GameObject.FindGameObjectWithTag("GameController").SendMessage("changeWindMessage", "Wind Inactive.");
        }
        

    }
}
