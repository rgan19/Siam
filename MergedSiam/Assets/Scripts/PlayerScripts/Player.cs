using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    //Movement
    public float moveSpeed;
    public float drag;
    public float terminalRotationSpeed;
    public VirtualJoystick joystickBehavior;
	public GameObject joystick;

    //public GameObject camera;

    //Health system
    private int maxHealth = 5;
    private int startHealth = 5;

	//[SyncVar]
    public int currHealth;

    public Image[] healthImages;
	private GameObject life;
	private GameObject life2;
	private GameObject life3;
	private GameObject life4;
	private GameObject life5;

    //Scoring
    public int score;

    //Game objects
	public Transform arrowSpawnPoint;
    public float waitTime;
    public GameObject arrow;
	public GameObject arrowShot;
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
        taichiShield = null;
        currHealth = startHealth;
        UpdateUIHealth();

    }

	public override void OnStartClient(){
		base.OnStartClient ();
		string _netID = GetComponent<NetworkIdentity> ().netId.ToString();
		Player _player = GetComponent<Player> ();
		GameManager.RegisterPlayer (_netID, _player);
	}

	public override void OnStartLocalPlayer(){
		GetComponent<MeshRenderer> ().material.color = Color.green;
		Camera.main.GetComponent<PlayerCamera> ().setTarget (gameObject.transform);
		this.joystick = GameObject.FindGameObjectWithTag ("Joystick");
		joystickBehavior = (VirtualJoystick)joystick.GetComponent (typeof(VirtualJoystick));

		arrowButton = (Button) GameObject.FindGameObjectWithTag ("ArrowButton").GetComponent<Button> ();
		arrowButton.interactable = false;
		arrowButton.onClick.AddListener (Shoot);


		taichiButton = (Button) GameObject.FindGameObjectWithTag ("TaichiButton").GetComponent<Button> ();
        taichiButton.onClick.AddListener(Taichi);

		healthImages[0] = (Image) GameObject.FindGameObjectWithTag ("Life").GetComponent(typeof(Image));
		healthImages[1] = (Image) GameObject.FindGameObjectWithTag ("Life2").GetComponent(typeof(Image));
		healthImages[2] = (Image) GameObject.FindGameObjectWithTag ("Life3").GetComponent(typeof(Image));
		healthImages[3] = (Image) GameObject.FindGameObjectWithTag ("Life4").GetComponent(typeof(Image));
		healthImages[4] = (Image) GameObject.FindGameObjectWithTag ("Life5").GetComponent(typeof(Image));

		Debug.Log ("Current health is "+healthImages.Length);
	}
    

    // Update is called once per frame
   private void Update() {

		if (!isLocalPlayer) {
			return;
		}
       
        Vector3 dir = Vector3.zero;
        // for keyboard movement
        /*    dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");
            dir *= moveSpeed * Time.deltaTime;
            if (dir.magnitude > 1)
                dir.Normalize();*/
        if (currHealth <= 0)
        {
            NetworkServer.Destroy(this.gameObject);
			Debug.Log ("Dead");
            // TODO: Change the game camera to view top down and see the whole map.
        }
        else
        {
            if (joystickBehavior.inputVector != Vector3.zero)
            {
                dir = joystickBehavior.inputVector;
                controller.MovePosition(transform.position + dir);
                transform.forward = dir;
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }

            //if taichiShield is active
            if (taichiShield != null)
                taichiShield.transform.position = transform.position;
            
        }
    }
		
	public void Shoot(){
		arrowButton.interactable = false;
		CmdShoot ();
	}
    //Instantiate an arrow object to shoot at current direction character is facing
    [Command]
    public void CmdShoot()
    {
        //arrowButton.interactable = false;
        arrowShot = (GameObject)Instantiate(arrow, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        NetworkServer.Spawn(arrowShot); //spawns on all clients through server
        Debug.Log ("Shoot");
	}


    //Instantiate a taichi shield around character to reflect all incoming projectiles for x seconds
	public void Taichi(){
		CmdTaichi ();
	}

	[Command]
    public void CmdTaichi()
    {
        //spawn taichi shield for x seconds
        taichiShield = Instantiate(taichiObj, transform.position, transform.rotation);
        NetworkServer.Spawn(taichiShield);
        Debug.Log("Taichi called");
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
	[ClientRpc]
	public void RpcAddHp(int amount){
		
		currHealth += amount;
		currHealth = Mathf.Clamp(currHealth, 0, maxHealth);
		UpdateUIHealth();
		Debug.Log("Current health is " + currHealth + " after " + amount + " hp");

	}

	[Command]
	public void CmdAddHp(int amount){
		RpcAddHp (amount);
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
            NetworkServer.Destroy(other.gameObject); //removes it on all clients
			Debug.Log("Destroy");
        }
        //if object is arrow
        if (other.gameObject.CompareTag("Arrow"))
        {
            Debug.Log("Player hit by arrow");
            // taichi shield is active, reflect projectile 
            if (taichiShield != null)
            {

            }
            // get damaged since taichi shield is not active
            else
            {
                //TODO: Fix HP, null pointer causes the arrow to not be destroyed in the next line
                CmdAddHp(-1);
                CmdDestroyArrow(other.gameObject);
				Destroy (other.gameObject);
                Debug.Log("Network Destroy arrow");

            }

        }
    }
		
	[Command]
	public void CmdDestroyArrow(GameObject other){
		NetworkServer.Destroy(other.gameObject);
		Debug.Log ("Command Destroy");
	}

}
