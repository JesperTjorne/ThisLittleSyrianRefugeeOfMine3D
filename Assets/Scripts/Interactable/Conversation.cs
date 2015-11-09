﻿using UnityEngine;
using System.Collections;
/// <summary>
/// This script is an extension of the interactable script. 
/// This specific script is used to handle conversation with NPCs.
/// </summary>
public class Conversation : Interactable 
{
	private GameObject conversationManagement;
	private ConversationManager conversationManager;
	public int conversationId;
	
	// Use this for initialization
	void Start () 
	{
        SetIcon((Texture2D)Resources.Load("Textures/conversation_icon"));
    	
		conversationManagement = GameObject.Find ("ConversationManagement");
		conversationManager = conversationManagement.GetComponent<ConversationManager>();
	}
	    
	public override void Interact ()
	{
		//conversationManager.StartConversation (conversationId);
		base.Interact ();
		StartCoroutine (CoroutineCalls ());
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.K)) {
			isInteractionActive = false;
			
		}
	}

	public IEnumerator CoroutineCalls()
	{
		Vector3 xPos = new Vector3 (interactionPos.transform.position.x, pm.transform.position.y, 0.0F); 			// find x position
		
		
		yield return StartCoroutine(MoveToPos(xPos));																// move to x position
		yield return StartCoroutine(MoveToPos(interactionPos.transform.position));									// move to interaction position
		yield return StartCoroutine(WaitForInteraction()); 														// while interacting
		yield return StartCoroutine(MoveToPos(new Vector3(pm.transform.position.x, pm.transform.position.y, 0.0F)));
		
	}
	
	public IEnumerator MoveToPos(Vector3 pos) {
		
		Debug.Log ("MoveToPos method called");
		
		pm.SetTarget	(pos);
		while (Vector3.Distance(pos, pm.transform.position) > 1.5F)
		{
			yield return null;
		}
		Debug.Log ("Done with moveToPos method");
	}

	
	private IEnumerator WaitForInteraction() 
	{
		isInteractionActive = true;
		conversationManager.StartConversation (this);
		while (isInteractionActive) {
			Debug.Log ("Interaction is active!");

			yield return null;
		}
	}
}
