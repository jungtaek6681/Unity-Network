using Photon.Pun;
using TMPro;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{
	private static string playerID = null;

	[SerializeField]
	private TMP_InputField idInputField;

	private void OnEnable()
	{
		idInputField.text = playerID ?? string.Format("Player {0}", Random.Range(1000, 10000));
	}

	public void OnLoginButtonClicked()
	{
		playerID = idInputField.text;

		if (playerID == "")
		{
			Debug.LogError("Invalid Player Name");
			return;
		}

		PhotonNetwork.LocalPlayer.NickName = playerID;
		PhotonNetwork.ConnectUsingSettings();
	}
}
