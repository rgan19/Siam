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

    public GameObject camera;

    //Health system
    private int maxHealth = 7;
    public int startHealth = 5;
    public int currHealth;

    public Image[] healthImages;

    //Scoring
    public int score;

    //Game objects
    public GameObject bulletSpawnPoint;
    public float waitTime;
    public GameObject bullet;
    public GameObject taichiObj;
    public GameObject playerObj;

    private Rigidbody controller;
    private GameObject taichiShield;

    private void Start()
    {
        controller = GetComponent<Rigidbody>();
        controller.maxAngularVelocity = terminalRotationSpeed;
        controller.drag = drag;
        taichiShield = taichiObj;
        currHealth = startHealth;
        updateUIHealth();
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
            Destroy(this.gameObject);
        else
        {
            if (joystick.inputVector != Vector3.zero)
            {
                dir = joystick.inputVector;
                controller.MovePosition(transform.position + dir);
                transform.forward = dir;
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                taichiShield.transform.position = transform.position;
            }

            //TODO: detect projectiles using triggers in order to know what to do
            //boss projectile: - hp, player projectile, stun? pickups: do smth


            //shoot testing, can be removed later
            if (Input.GetKeyDown(KeyCode.Space))//Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

    }

    //Instantiate an arrow object to shoot at current direction character is facing
    public void Shoot()
    {
        Instantiate(bullet.transform, bulletSpawnPoint.transform.position,bulletSpawnPoint.transform.rotation);
    }

    //Instantiate a taichi shield around character to reflect all incoming projectiles for x seconds
    public void Taichi()
    {
        //spawn taichi shield for x seconds
        taichiShield = Instantiate(taichiObj,transform.position,transform.rotation);
    }

    //Update UI to display current hp user has
    void updateUIHealth()
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
        updateUIHealth();
    }
}
