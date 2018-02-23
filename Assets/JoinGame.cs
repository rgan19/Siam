using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
using UnityEngine;


public class JoinGame : MonoBehaviour {

	List<GameObject> roomList = new List<GameObject>();

	[SerializeField]
	private Text status;

	[SerializeField]
	private GameObject roomListItemPrefab;

	[SerializeField]
	private Transform roomListParent; //reference to scroll view

	private NetworkManager networkManager;

	void Start(){
		//to initialise and enable matchmaker
		networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null) {
			networkManager.StartMatchMaker();
		}

		RefreshRoomList();

	}

	//public so that UI button can access the refresh
	public void RefreshRoomList(){
		ClearRoomList();
		networkManager.matchMaker.ListMatches(0, 20,"", false, 0, 0, OnMatchList);
		status.text = "Loading....";
	}

	//old API uses ListMatchResponse
	public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList){
		status.text = "";
		if (matchList == null) {
			status.text = "Unable to get room list."; //usually due to connectivity issues if error
			return;
		}
	
		ClearRoomList ();

		//at first its matchlist.matches?
		foreach (MatchInfoSnapshot match in matchList) {
			GameObject roomListItemGO = Instantiate(roomListItemPrefab);
			roomListItemGO.transform.SetParent(roomListParent);

			//This is a component sit on the gameobjecet that will take care of setting up the name / amount of users
			RoomListItem roomListItem = roomListItemGO.GetComponent<RoomListItem> ();
			if (roomListItem != null) {
				roomListItem.Setup(match, JoinRoom);
			}

			//set up callback function that will join the game

			roomList.Add (roomListItemGO);
		}
			
		if (roomList.Count == 0) {
			status.text = "No rooms at the moment";
		}

	}

	void ClearRoomList(){
		for (int i = 0; i < roomList.Count; i++) {
			Destroy (roomList [i]);
		}
		roomList.Clear();
	}

	public void JoinRoom(MatchInfoSnapshot _match){
		
		networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "",0, 0, networkManager.OnMatchJoined);
		ClearRoomList ();
		status.text = "Joining...";

	}

}
