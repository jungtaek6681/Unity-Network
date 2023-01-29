using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class Client : MonoBehaviour
{
	[SerializeField]
	private ChatUI chatUI;
	[SerializeField]
	private TMP_InputField nameField;
	[SerializeField]
	private TMP_InputField ipField;
	[SerializeField]
	private TMP_InputField portField;

	private string clientName;
	private string ip;
	private int port;

	public bool IsConnect { get;  private set; } = false;

	private TcpClient client;
	private NetworkStream stream;
	private StreamWriter writer;
	private StreamReader reader;

	private void Update()
	{
		if (IsConnect && stream.DataAvailable)
		{
			string chat = reader.ReadLine();
			if (chat != null)
				ReceiveChat(chat);
		}
	}

	private void OnDisable()
	{
		DisConnect();
	}

	public void Connect()
	{
		if (IsConnect)
			return;

		AddMessage("Try to Connect ");

		clientName = nameField.text == "" ? "NickName" : nameField.text;
		ip = ipField.text == "" ? "127.0.0.1" : ipField.text;
		port = portField.text == "" ? 5555 : int.Parse(portField.text);

		try
		{
			client = new TcpClient(ip, port);
			stream = client.GetStream();
			writer = new StreamWriter(stream);
			reader = new StreamReader(stream);

			AddMessage("Connect Success");
			IsConnect = true;
			nameField.interactable = false;
			ipField.interactable = false;
			portField.interactable = false;
		}
		catch (Exception e)
		{
			AddMessage("Connect Fail : " + e.Message);
			DisConnect();
		}
	}

	public void DisConnect()
	{
		writer?.Close();
		writer = null;
		reader?.Close();
		reader = null;
		stream?.Close();
		stream = null;
		client?.Close();
		client = null;
		IsConnect = false;
		nameField.interactable = true;
		ipField.interactable = true;
		portField.interactable = true;
		AddMessage("DisConnect");
	}

	public void SendChat(string chat)
	{
		if (!IsConnect)
		{
			AddMessage("Client is not connected");
			return;
		}

		try
		{
			writer.WriteLine(string.Format("{0} : {1}", clientName, chat));
			writer.Flush();
		}
		catch (Exception e)
		{
			AddMessage("Send chat Fail " + e.Message);
		}

	}

	public void ReceiveChat(string chat)
	{
		Debug.Log(chat);
		chatUI.AddChat(chat);
	}

	private void AddMessage(string message)
	{
		Debug.Log(string.Format("[Client] {0}", message));
		chatUI.AddMessage(string.Format("[Client] {0}", message));
	}
}
