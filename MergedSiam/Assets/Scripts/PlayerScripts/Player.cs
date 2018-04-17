using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using Prototype.NetworkLobby;

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

	[SyncVar]
    public int currHealth;

    public Image[] healthImages;

	[HideInInspector]
	private GameObject life, life2, life3, life4, life5;

    //Scoring
    public int score;

	public string state{get;set;}

    //Game objects
	public Transform arrowSpawnPoint;
    public float waitTime;
    public GameObject arrow;
	public GameObject arrowShot;
    public GameObject taichiObj;

    public GameObject laser;
    public GameObject bow;
    public GameObject explosion;

    //SFX
    private AudioSource audioSource;
    public AudioClip bowClip;
    public AudioClip pickupClip;

	//UI END GAME
	public GameObject endGame;
	private Text endStatusText;
	private GameObject gameOverOverlay;

    private Rigidbody controller;
    private GameObject taichiShield;
    
    private Button arrowButton;
    private Button taichiButton;

	Timer timer;

	bool isGameEnd;


    //Animation
    private Animator anim;
    public float delay;
    private float move;


    private void Start()
    {
        controller = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        controller.maxAngularVelocity = terminalRotationSpeed;
        controller.drag = drag;
        taichiShield = null;
        currHealth = startHealth;
        UpdateUIHealth();
        bow.SetActive(false);

        // Destroys laser if not local player
        if (!isLocalPlayer)
        {
            Destroy(laser);
        }

        // Spawns on random location in arena
        this.transform.position = new Vector3(Random.Range(10, -10), 3, Random.Range(10, -10));

        // Get Timer
		GameObject timerObj = GameObject.FindGameObjectWithTag ("Timer");
		timer = timerObj.GetComponent<Timer> ();
		timer.resetTimer ();
		GameManager.ClearPlayerList();
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
		arrowButton.interactable = true;
		arrowButton.onClick.AddListener (Shoot);

		taichiButton = (Button) GameObject.FindGameObjectWithTag ("TaichiButton").GetComponent<Button> ();
        taichiButton.onClick.AddListener(Taichi);

		healthImages[0] = (Image) GameObject.FindGameObjectWithTag ("Life").GetComponent(typeof(Image));
		healthImages[1] = (Image) GameObject.FindGameObjectWithTag ("Life2").GetComponent(typeof(Image));
		healthImages[2] = (Image) GameObject.FindGameObjectWithTag ("Life3").GetComponent(typeof(Image));
		healthImages[3] = (Image) GameObject.FindGameObjectWithTag ("Life4").GetComponent(typeof(Image));
		healthImages[4] = (Image) GameObject.FindGameObjectWithTag ("Life5").GetComponent(typeof(Image));

		gameOverOverlay = GameObject.FindGameObjectWithTag("GameOverOverlay");
		gameOverOverlay.SetActive (false);


	}
    

    // Update is called once per frame
   private void Update() {

		if (!isLocalPlayer) {
			return;
		}
		if (timer.isTimeUp) {
			ShowTimeUp ();
		
		}
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
        Vector3 dir = Vector3.zero;

        move = Mathf.Sqrt(Mathf.Pow(joystickBehavior.Horizontal(),2) + Mathf.Pow(joystickBehavior.Vertical(),2));
        anim.SetFloat("Speed", move);
        
        // for keyboard movement
        /*    dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");
            dir *= moveSpeed * Time.deltaTime;
            if (dir.magnitude > 1)
                dir.Normalize();*/
		if (currHealth <= 0)
        {
			if (!isGameEnd) 
			{
				isGameEnd = true;
				anim.SetTrigger("Death");
				Death();
			}
            

			//CmdDestroyObject (this.gameObject);
			//Destroy (this.gameObject);
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
            else if (anim.GetBool("Taichi"))
                anim.SetBool("Taichi", false);
        }
    }

    private void FixedUpdate()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("ArrowAnim"))
        {
            bow.SetActive(true);
        }
        else bow.SetActive(false);
    }

    public void Shoot(){
		arrowButton.interactable = false;
		CmdShoot ();
        audioSource.PlayOneShot(bowClip, 1f);
    }
    //Instantiate an arrow object to shoot at current direction character is facing
    [Command]
    public void CmdShoot()
    {
        //arrowButton.interactable = false;
        anim.SetTrigger("Arrow");
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
        anim.SetTrigger("TaichiTrigger");
        if (!anim.GetBool("Taichi"))
        {
            anim.SetBool("Taichi", true);
        }
        Quaternion shieldRotate = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x + 270, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        taichiShield = Instantiate(taichiObj, transform.position, transform.rotation);
        //taichiShield.transform.parent = transform;
        NetworkServer.Spawn(taichiShield);
        RpcTaichi(taichiShield);
        Debug.Log("Taichi called");
        //uses animation created by cyrus.
        //makes character immune for x seconds (disable rigidbody collider maybe)
        //makes projectiles that collides with character to be sent to direction player is facing.
    }
		
    [ClientRpc]
    public void RpcTaichi(GameObject taichi)
    {
        taichi.transform.parent = transform;
    }
		
    //Update UI to display current hp user has
    void UpdateUIHealth()
    {
		if (!isLocalPlayer) {
			return;
		}
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
                audioSource.PlayOneShot(pickupClip, 0.7f);
                arrowButton.interactable = true;
			} 
			CmdDestroyObject (other.gameObject);
			Destroy (other.gameObject);
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
                CmdAddHp(-1);
                Instantiate(explosion, this.transform.position, this.transform.rotation);
                CmdDestroyObject(other.gameObject);
				Destroy (other.gameObject);
            }

        }
    }
		
	//destroys objects in server
	[Command]
	public void CmdDestroyObject(GameObject other){
		if (isLocalPlayer) {
			return;
		}
		NetworkServer.Destroy(other.gameObject);
	}

	//Player Death
	void Death(){
		CmdReducePlayerCount (); //reduce player count in GameManager
        //gameOverOverlay.SetActive (true);
		CmdDestroyObject (this.gameObject); //destroy on all clients
		Destroy(this.gameObject); //destroy on local player
		Debug.Log ("Going to End Player Game");
		EndPlayerGame ();
		Debug.Log("Death");
	}


	[Command]
	void CmdReducePlayerCount(){
		string _netID = GetComponent<NetworkIdentity> ().netId.ToString();
		string playerNameID = "Player " + _netID;
		GameManager.DeregisterPlayer (playerNameID);
		Debug.Log("Number of players alive: "+ GameManager.GetNumberOfPlayersAlive());
		if (GameManager.GetNumberOfPlayersAlive () <= 1) {
			CmdShowEndGame ();
		} 
	}


	[Command]
	void CmdShowEndGame(){
		//if player is alive should be tagged
		foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")){
			player.GetComponent<Player>().RpcEndGame ();
			Debug.Log ("In Show End Game");
		}
	}

	[ClientRpc]
	void RpcEndGame(){
		if (!isLocalPlayer)
			return;
		EndGame ();
	}

	//End whole game. Find the only player alive and set win screen. 
	void EndGame(){
		string _netID = GetComponent<NetworkIdentity> ().netId.ToString();
		string playerNameID = "Player " + _netID;
		Debug.Log ("Player ID: " + playerNameID);
		Dictionary<string, Player> listOfPlayers = GameManager.players;
		if (listOfPlayers.ContainsKey(playerNameID)){
			//Player p = listOfPlayers [playerNameID];
			Debug.Log (playerNameID + " win");
			GameObject endSplash;
			endSplash = (GameObject)Instantiate (endGame);
			//gameOverOverlay = GameObject.FindGameObjectWithTag("GameOverOverlay");
			//p.GameOverOverlay.SetActive(true);
			gameOverOverlay.SetActive (true);
			endSplash.GetComponent<Text> ().text = "You win!";
			//endSplash.transform.SetParent (gameOverOverlay.GetComponent<RectTransform> (), false);
			GameManager.players.Clear();
			GameManager.DeregisterPlayer (playerNameID);
			Debug.Log ("player list size: " + listOfPlayers.Count);
			CmdDestroyObject(this.gameObject);
			Destroy (this.gameObject);
			//StartCoroutine (CountEndScene ());
		} else {
			Debug.Log ("Reached into end game check winner");
		}

	}



	//Each player result when they lose
	void EndPlayerGame(){
		gameOverOverlay.SetActive (true);
		GameObject endSplash;
		endSplash = (GameObject)Instantiate (endGame);
		endSplash.GetComponent<Text> ().text = "You lose!";
		//endSplash.transform.SetParent (gameOverOverlay.GetComponent<RectTransform>(), false);
		//StartCoroutine (CountEndScene());

	}

	void EndGameWhenTimesUp() {
		GameObject endSplash;
		endSplash = (GameObject)Instantiate (endGame);
		endSplash.GetComponent<Text> ().text = "You have survived today";
	}

	void ShowTimeUp() {
		foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")){
			player.GetComponent<Player>().EndGameWhenTimesUp ();
			CmdDestroyObject (this.gameObject); //destroy on all clients
			Destroy(this.gameObject); //destroy on local player
			GameManager.DeregisterPlayer("Player " + this.netId);
		}
	}

	//Incomplete: To be deleted
	IEnumerator CountEndScene(){
		Debug.Log ("Going back to lobby");
		yield return new WaitForSeconds (3);
		GameObject.FindGameObjectWithTag ("LobbyManager").GetComponent<LobbyManager> ().OnStopHost ();
		SceneManager.LoadScene("Lobby");
	}

}
