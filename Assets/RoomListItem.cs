using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour {

	private MatchInfoSnapshot match;
	public delegate void JoinRoomDelegate(MatchInfoSnapshot _match); //points to a few function, search the meaning
	private JoinRoomDelegate joinRoomCallback;


	[SerializeField]
	private Text roomNameText;

	public void Setup(MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallback){
		match = _match;
		joinRoomCallback = _joinRoomCallback;
		roomNameText.text = match.name + "(" + match.currentSize + "/" + match.maxSize + ")";

	}

	public void JoinRoom(){
		joinRoomCallback.Invoke(match);	
	}
}
