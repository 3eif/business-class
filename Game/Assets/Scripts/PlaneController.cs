using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlaneController : MonoBehaviour {


	[Header("Movement")]
	public static float speed = 0f;
	private float rotationSpeed = 75f;
	private float zRot = 0;
	public float maxSpeed = 99f;
	public float minSpeed = 50f;
	public float minBreakSpeed = 20f;
	public float acceleration = 25;
	private float decceleration = -10;

	[Header("Crash")]
	private bool crashed = false;
	private bool updatedCrash = false;
	private int crashedNum = 0;
	private int respawnTime = 0;
	public static Vector3 respawnPosition;
	public static Vector3 respawnRotation;
	private bool inWater = false;

	[Header("Objects")]
	private GameObject airplane;
	private GameObject crashCam;
	public Rigidbody rb;
	private GameObject spin;
	private ParticleSystem smoke;
	private ParticleSystem fire;
	private Light fireLight;
	public static GameObject tempQuestion;

	[Header("Fuel")]
	public static float fuel;
	public float startFuel;
	private Image fuelBar;

	[Header("Misc")]
	public static bool questionCol;
	public static bool lost = false;
	public static bool won = false;

    private bool hasCrashedAlready = false;

	//This function runs right when the scene starts. It finds all the game objects in the game and assigns them to variables. 
	//This function will also set each planes' fuel
	private void Awake(){
        string planeStr = ShopSystem.planes[ShopSystem.selectedIndex];
			
        switch (ShopSystem.selectedIndex) {
            case 0:
                startFuel = 100f;
                break;
            case 1:
                startFuel = 150f;
                break;
            case 2:
                startFuel = 200f;
                break;
            case 3:
                startFuel = 250f;
                break;
            case 4:
                startFuel = 10000f;
                break;
        }

        airplane = GameObject.Find("/" + planeStr);
        if (ShopSystem.selectedIndex == 0) {
            spin = GameObject.Find("/" + planeStr + "/spin");
        }
		
        crashCam = GameObject.Find("/cameras/CM vcam1");
		fireLight = GameObject.Find("/" + planeStr + "/fireSmoke/light").GetComponent<Light>();
		fuelBar = GameObject.Find("/canvas/hud/fuelPanel/fuelBar").GetComponent<Image>();
		fire = GameObject.Find("/" + planeStr + "/fireSmoke/fire").GetComponent<ParticleSystem>();
		smoke = GameObject.Find("/" + planeStr + "/fireSmoke/smoke").GetComponent<ParticleSystem>();
		
		var smokeEmission = smoke.emission;
		var fireEmission = fire.emission;
		smokeEmission.enabled = false;
		fireEmission.enabled = false;
		fireLight.GetComponent<Light>().enabled = false;

	}

	void Start(){
		//Sets the fuel to the st
		fuel = startFuel;
		rb = GetComponent<Rigidbody>();
        hasCrashedAlready = false;
	}

	//This function checks every frame whether the plane has crashed or not. If it has crashed, it will run the crash function, if not it will keep moving.
	//This function also makes sure the fuel doesn't go above the max fuel.
	void Update(){
		if(crashed) {
            if (!hasCrashedAlready) {
            	crash();
                hasCrashedAlready = true;
            }
		} else {
			move();
		}

		if(fuel > startFuel){
			fuel = startFuel;
		}
	}

	//This function will run when the plane collides with the environment. When the plane collides with the environment, it will call the crash function.
    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "environment" && !won){
            crashed = true;
			updatedCrash = true;
        } else {
			updatedCrash = false;
		}
    }

	//This funcion will run when the plane goes in a question cube or water. When it goes in the water it will crash, when it 
	//goes into a question cube, the question cube will be destroyed and the quiz panel pops up.
	void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "question cube"){

			if(!GameManager.isEndless){
				Destroy(col.gameObject);
			} else {
				tempQuestion = col.gameObject;
				tempQuestion.SetActive(false);
			}

			questionCol = true;
		}

		if(col.gameObject.tag == "water" && !won){
			inWater = true;
            crashed = true;
			updatedCrash = true;
		} else {
			inWater = false;
			updatedCrash = false;
		}
    }

	//This funcion creates the fire and smoke particles, and makes sure the plane can't move. It also shows up the game over screen.
	void crash(){
		crashCam.SetActive(false);
		speed = 0;
		rb.isKinematic = false;
		rb.useGravity = true;
		rb.mass = 5000;
		var smokeEmission = smoke.emission;
		var fireEmission = fire.emission;
		if(!inWater){
			smokeEmission.enabled = true;
			fireEmission.enabled = true;
		} else {
			smokeEmission.enabled = false;
			fireEmission.enabled = false;
		}
		fireLight.GetComponent<Light>().enabled = true;
		lost = true;
	}

	void move(){
		//Make sure speed doesn't go over maxspeed, and doesn't go less than minspeed.
		if(speed > minSpeed){
			speed -= transform.forward.y-transform.forward.y/2;
		}
		if(speed > maxSpeed){
			speed = maxSpeed;
		}

		//Slows down the plane when space bar is pressed.
        if (Input.GetKey("space")){
			if(speed > minBreakSpeed){
				speed += decceleration*Time.deltaTime;
			} else {
				speed += acceleration*Time.deltaTime;
			}
		//Accelerates or deccelarates the plane
        } else {
			if(speed < minSpeed && fuel > 0) {
				speed += acceleration*Time.deltaTime;
			}
			if(fuel < 0 && speed > 0){
				speed += decceleration/2*Time.deltaTime;
			}
		}

		//moves the plane by an amount every frame
		transform.position += transform.forward * Time.deltaTime * speed;

        float dz = 3.0f;
		//rotates the plane based on left and right arrow keys
        if (System.Math.Abs(Input.GetAxis("Horizontal")) > 0.0f) {
            if (Input.GetAxis("Horizontal") > 0.0f) {
                zRot -= dz;
                if (zRot < -40.0f) {
                    zRot = -40.0f;
                }
            } else {
                zRot += dz;
                if (zRot > 40.0f) {
                    zRot = 40.0f;
                }
            }
        } else {
            if (zRot < 0.0f) {
                zRot += dz;
                if (zRot > 0.0f) {
                    zRot = 0.0f;
                }
            } else {
                zRot -= dz;
                if (zRot < 0.0f) {
                    zRot = 0.0f;
                }
            }
        }

        Vector3 v = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(v.x, v.y, zRot);

        transform.Rotate(Vector3.right, Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.up, 0.5f * Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime, Space.World);

        v = transform.rotation.eulerAngles;
        float threshold = 80.0f;

        float currX = (v.x > 180) ? v.x - 360.0f : v.x;

        if (currX < -threshold) {
            transform.rotation = Quaternion.Euler(-threshold, v.y, v.z);
        }
        if (currX > threshold) {
            transform.rotation = Quaternion.Euler(threshold, v.y, v.z);
        }

		//Subtracts fuel relative to how much the speed is
        if (ShopSystem.selectedIndex != 4) fuel -= (speed/35)*Time.deltaTime;
		fuelBar.fillAmount = fuel/startFuel;
		if(fuel < 0){
			rb.useGravity = true;
		}

		//Rotates the plane's propeller
        if (ShopSystem.selectedIndex == 0) {
            spin.transform.Rotate(0, 0, (speed * 40) * Time.deltaTime);
        }
	}

	IEnumerator respawn(){
		//waits 4 seconds, then respawns with new values(new positions, rotations, speed, bools, etc);
        yield return new WaitForSeconds(4);
		if(updatedCrash && crashedNum == 0){
			crashedNum++;
			fuel = startFuel;
			transform.position = new Vector3(respawnPosition.x+6, respawnPosition.y+2, respawnPosition.z);
			transform.rotation = Quaternion.Euler(respawnRotation.x, respawnRotation.y, respawnRotation.z);
			rb.velocity = new Vector3(0f,0f,0f);
			rb.angularVelocity = new Vector3(0f,0f,0f);
			speed = 0;
			crashed = false;
			updatedCrash = false;
			rb.useGravity = false;
			rb.mass = 1;
		}
    }

	public static IEnumerator respawnQuestion(){
		yield return new WaitForSeconds(3);
		tempQuestion.SetActive(true);
	}
}

