using Photon.Pun;
using TMPro;
using UnityEngine;

public class StatePanel : MonoBehaviour
{
	public static StatePanel Instance { get; private set; }

	[SerializeField]
	private RectTransform content;
	[SerializeField]
	private TMP_Text textPrefab;

	private Photon.Realtime.ClientState state;

	private void Awake()
	{
		Instance = this;
	}

	void Update()
	{
		if (state == PhotonNetwork.NetworkClientState)
			return;

		state = PhotonNetwork.NetworkClientState;

		TMP_Text instance = Instantiate(textPrefab, content);
		instance.text = string.Format("[Photon NetworkState] {0} : {1}", System.DateTime.Now.ToString("HH:mm:ss.ff"), state.ToString());
		Debug.Log(string.Format("[Photon NetworkState] {0}", state.ToString()));
	}

	public void AddMessage(string message)
	{
		TMP_Text instance = Instantiate(textPrefab, content);
		instance.text = string.Format("[Photon] {0} : {1}", System.DateTime.Now.ToString("HH:mm:ss.ff"), message);
		Debug.Log(string.Format("[Photon] {0}", message));
	}
}
