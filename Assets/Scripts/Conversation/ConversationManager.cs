﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI; 

public class ConversationManager : MonoBehaviour 
{
	//!EventSystem.current.IsPointerOverGameObject()
	private bool activeConversation = false;
	private GameObject conversationPanel;
	private Image panelImage;

	private GameObject close;
	private Image closeImage;
	private Text closeText;

	public List<GameObject> fields = new List<GameObject>();
	List<string[]> conversations = new List<string[]>();
	List<int[]> indexes = new List<int[]>();
	List<int> newIndexes = new List<int>();

	TypeWriter typeWriter;
	private int counter = 0;
	private int index;

	// Use this for initialization
	void Start () 
	{
		close = GameObject.Find ("Close");
		closeImage = close.GetComponent<Image>();
		Text[] closeArr = close.GetComponentsInChildren<Text>();
		closeText = closeArr[0];

		conversationPanel = GameObject.Find ("ConversationPanel");
		panelImage = conversationPanel.GetComponent<Image>();

		typeWriter = gameObject.GetComponent<TypeWriter>();
	}

	public void InputAnswer(int index)
	{
		Debug.Log (index);
		this.index = index;
		counter = 0;
		UnloadPanel ();
		PrepareText();

		if(indexes[index].Length == 1)
		{
			closeImage.enabled = true;
			closeText.enabled = true;
		}
	}

	public void DoneTyping()
	{
		if (counter < conversations [index].Length) 
		{
			PrepareText ();
		} 
	}

	private void PrepareText()
	{
		if (counter == 0) 
		{
			Text text = fields [counter].GetComponent<Text> ();
			typeWriter.WriteText (conversations [index] [counter], text);
			counter++;
			return;
		} 
		else if(counter < fields.Count-1)
		{
			Button button = fields[counter].GetComponent<Button>();
			Text[] answerArr = button.GetComponentsInChildren<Text>();
			Text answer = answerArr[0];
			answer.enabled = true;
			typeWriter.WriteText(conversations[index][counter], answer);
			int newIndex = indexes[index][counter];
			button.onClick.AddListener(
				delegate
				{
				InputAnswer(newIndex);
			}
			);
			counter++;
		}
	}

	private void UnloadPanel()
	{
		for (int i = 1; i < fields.Count; i++) 
		{
			Button button = fields[i].GetComponent<Button>();
			button.onClick.RemoveAllListeners();
			Text[] textArr = fields[i].GetComponentsInChildren<Text>();
			textArr[0].enabled = false;
		}
	}

	public void StartConversation(int conversationID)
	{
		if (!activeConversation) 
		{
			panelImage.enabled = true;
			activeConversation = true;

			SetLists (conversationID);
			InputAnswer (0);
		}
	}

	public void CloseConversation()
	{
		activeConversation = false;
		closeImage.enabled = false;
		closeText.enabled = false;
		panelImage.enabled = false;
		Text text = fields[0].GetComponent<Text>();
		text.text = "";
		UnloadPanel ();
	}

	private void SetLists(int conversationID)
	{
		switch(conversationID)
		{
			case 1:
			{
				FatherSon fatherSon = gameObject.GetComponent<FatherSon>();
				conversations = fatherSon.GetConversationList();
				indexes = fatherSon.GetindexesList();
				break;
			}
			case 2:
			{
				FatherWife fatherWife = gameObject.GetComponent<FatherWife>();
				conversations = fatherWife.GetConversationList();
				indexes = fatherWife.GetindexesList();
				break;
			}
		}
	}
}
