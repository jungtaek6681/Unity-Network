using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using TMPro;
using UnityEngine;
using System.Linq;

public class Server : MonoBehaviour
{
    [SerializeField] RectTransform logContent;
    [SerializeField] TMP_Text logTextPrefab;
    [SerializeField] TMP_InputField ipField;
    [SerializeField] TMP_InputField portField;

    public bool IsOpened { get; private set; } = false;

    private IPAddress ip;
    private int port;

    TcpListener listener;
    List<TcpClient> connectedClients;
    List<TcpClient> disConnectedClients;

    private void OnEnable()
    {
        ShowIPAddress();
    }

    private void Update()
    {
        if (!IsOpened)
            return;

        foreach (TcpClient client in connectedClients)
        {
            if (!ClientConnectCheck(client))
            {
                client.Close();
                disConnectedClients.Add(client);
                continue;
            }
            NetworkStream stream = client.GetStream();
            if (stream.DataAvailable)
            {
                StreamReader reader = new StreamReader(stream);
                string chat = reader.ReadLine();
                if (chat != null)
                    SendAll(chat);
            }
        }

        for (int i = 0; i < disConnectedClients.Count; i++)
        {
            connectedClients.Remove(disConnectedClients[i]);
        }
        disConnectedClients.Clear();
    }

    private void OnDisable()
    {
        if (IsOpened)
            Close();
    }

    public void Open()
    {
        if (IsOpened)
            return;

        AddLog("Try to Open");

        port = portField.text == "" ? 5555 : int.Parse(portField.text);

        connectedClients = new List<TcpClient>();
        disConnectedClients = new List<TcpClient>();

        try
        {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            AddLog("Server Opened");
            IsOpened = true;
            portField.interactable = false;
        }
        catch (Exception e)
        {
            AddLog("Server Open Fail : " + e.Message);
            Close();
        }

        listener.BeginAcceptTcpClient(AcceptCallback, listener);
    }

    public void Close()
    {
        listener?.Stop();
        listener = null;
        IsOpened = false;
        portField.interactable = true;

        AddLog("Server Closed");
    }

    public void SendAll(string chat)
    {
        AddLog("SendAll " + chat);
        foreach (TcpClient client in connectedClients)
        {
            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.WriteLine(chat);
            writer.Flush();
        }
    }

    private void ShowIPAddress()
    {
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        ip = host
            .AddressList
            .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        ipField.text = ip.ToString();
    }

    private void AcceptCallback(IAsyncResult ar)
    {
        Debug.Log("AcceptCallback");

        if (listener == null)
        {
            Debug.Log("Server Closed");
            return;
        }

        TcpClient client = listener.EndAcceptTcpClient(ar);
        connectedClients.Add(client);
        AddLog("Client connected");
        listener.BeginAcceptTcpClient(AcceptCallback, null);
    }

    private bool ClientConnectCheck(TcpClient client)
    {
        try
        {
            if (client != null && client.Client != null && client.Connected)
            {
                if (client.Client.Poll(0, SelectMode.SelectRead))
                    return !(client.Client.Receive(new byte[1], SocketFlags.Peek) == 0);

                return true;
            }
            else
                return false;
        }
        catch (Exception e)
        {
            AddLog("Connect Check Error");
            AddLog(e.Message);
            return false;
        }
    }

    private void AddLog(string message)
    {
        Debug.Log(string.Format("[Server] {0}", message));
        TMP_Text newLog = Instantiate(logTextPrefab, logContent);
        newLog.text = message;
    }
}
