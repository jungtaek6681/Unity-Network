using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
	[SerializeField]
	private TMP_InputField inputField;
	[SerializeField]
	private RectTransform chatContent;
	[SerializeField]
	private TMP_Text chatText;
	[SerializeField]
	private ScrollRect chatScrollView;

	private void Update()
	{
		if (Input.GetButtonDown("Submit"))
		{
			if (inputField.IsActive() && inputField.text != "")
				AddChat(inputField.text);
			else
				inputField.ActivateInputField();
		}
	}

	public void AddMessage(string message)
	{
		if (chatScrollView == null)
			return;
		if (inputField == null)
			return;

		TMP_Text newMessage = Instantiate(chatText, chatContent.transform);
		newMessage.text = message;
		newMessage.fontSize = 32;
		newMessage.color = Color.red;
		chatScrollView.verticalScrollbar.value = 0;

		inputField.text = "";
		inputField.ActivateInputField();
	}

	public void AddChat(string chat)
	{
		if (chatScrollView == null)
			return;
		if (inputField == null)
			return;

		TMP_Text newMessage = Instantiate(chatText, chatContent.transform);
		newMessage.text = chat;
		newMessage.fontSize = 32;
		newMessage.color = Color.black;
		chatScrollView.verticalScrollbar.value = 0;

		inputField.text = "";
		inputField.ActivateInputField();
	}
}
