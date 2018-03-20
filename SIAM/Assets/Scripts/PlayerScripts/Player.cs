using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    //Movement
    public float moveSpeed;
    public float drag;
    public float terminalRotationSpeed;
    public VirtualJoystick joystick;
    private float stunDuration;
    //public GameObject camera;

    //Health system
    private int maxHealth = 7;
    private int startHealth = 5;
    public int currHealth;

    public Image[] healthImages;

    //Scoring
    public int score;

    //Game objects
    public GameObject arrowSpawnPoint;
    public float waitTime;
    public GameObject arrow;
    public GameObject taichiObj;
  
    private Rigidbody controller;
    private GameObject taichiShield;

    private Button arrowButton;
    private Button taichiButton;

    private void Start()
    {
        controller = GetComponent<Rigidbody>();
        controller.maxAngularVelocity = terminalRotationSpeed;
        controller.drag = drag;
        taichiShield = taichiObj;
        currHealth = startHealth;
        UpdateUIHealth();
        stunDuration = 0;
        arrowButton = GameObject.FindWithTag("ArrowButton").GetComponent<Button>() as Button;
        taichiButton = GameObject.FindWithTag("TaichiButton").GetComponent<Button>() as Button;
    }

    // Update is called once per frame
   private void Update() {
       
        Vector3 dir = Vector3.zero;
        // for keyboard movement
        /*    dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");
            dir *= moveSpeed * Time.deltaTime;
            if (dir.magnitude > 1)
                dir.Normalize();*/
        if (currHealth <= 0)
        {
            Destroy(this.gameObject);
            // TODO: Change the game camera to view top down and see the whole map.
        }
        else
        {
            if (joystick.inputVector != Vector3.zero)
            {
                dir = joystick.inputVector;
                controller.MovePosition(transform.position + dir);
                transform.forward = dir;
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

                //if taichiShield is active
                if(taichiShield!=null)
                    taichiShield.transform.position = transform.position;
            }

            //shoot testing, can be removed later
            if (Input.GetKeyDown(KeyCode.Space))//Input.GetMouseButtonDown(0))
            {
                Shoot();
                Debug.Log("Shoot arrow succeed");
            }

        }
    }

    //Instantiate an arrow object to shoot at current direction character is facing
    public void Shoot()
    {
        arrowButton.interactable = false;
        Instantiate(arrow.transform, arrowSpawnPoint.transform.position,arrowSpawnPoint.transform.rotation);
    }

    //Instantiate a taichi shield around character to reflect all incoming projectiles for x seconds
    public void Taichi()
    {
        //spawn taichi shield for x seconds
        taichiShield = Instantiate(taichiObj,transform.position,transform.rotation);
        //uses animation created by cyrus.
        //makes character immune for x seconds (disable rigidbody collider maybe)
        //makes projectiles that collides with character to be sent to direction player is facing.
    }

    //Update UI to display current hp user has
    void UpdateUIHealth()
    {
        for (int i = 0; i < maxHealth; i++)
        {
            if (currHealth <= i)
            {
                healthImages[i].enabled = false;
            }
            else
            {
                healthImages[i].enabled = true;
            }
        }
    }

    //Add or deal damage to user, negative means damage
    public void AddHp(int amount)
    {
        currHealth += amount;
        currHealth = Mathf.Clamp(currHealth, 0, maxHealth);
        UpdateUIHealth();
        Debug.Log("Current health is " + currHealth + " after " + amount + " hp");
    }

    // if touch object that triggers effect
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SkillDrop"))
        {
            Debug.Log("Player touched skill crate");
            // get a random skill
            if (!arrowButton.IsInteractable())
            {
                arrowButton.interactable = true;
            }
            //AddHp(1);
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("Arrow"))
        {
            // taichi shield is active, reflect projectile towards direction player is facing
            if (taichiShield != null)
            {

            }
            // get damaged since taichi shield is not active
            else
            {

            }
            AddHp(-1);
            Destroy(other.gameObject);
        }
    }
    /* not using this function
    // if collide onto other objects
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BossProjectile"))
        {
            Debug.Log("Player hit by boss projectile");
            AddHp(-1);
            stunDuration += 1f;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Arrow"))
        {
            Debug.Log("Player hit by other player arrow");
            //set stun
            stunDuration += 1f;
            Destroy(other.gameObject);
        }

    } */
}
