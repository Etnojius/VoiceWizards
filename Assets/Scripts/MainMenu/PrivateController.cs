using MLAPI;
using MLAPI.Configuration;
using MLAPI.Transports.UNET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrivateController : MonoBehaviour
{
    public string ip;
    public string port;
    public int writeTo;
    public bool host;
    public GameObject button;
    public GameObject hostButton;
    public GameObject joinButton;
    public UNetTransport uNetTransport;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Debugger.LogError("Program started");
    }

    // Update is called once per frame
    void Update()
    {
        if (writeTo == 3)
        {
            Destroy(button);
            writeTo++;
        }
        if (writeTo == 4)
        {
            hostButton.SetActive(true);
            joinButton.SetActive(true);
            writeTo++;
        }
    }
    public void StartAsHost()
    {
        SceneManager.LoadScene("Game");
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            StartCoroutine(LoadThenHost());
        }
    }

    public void StartAsClient()
    {
        SceneManager.LoadScene("Game");
        if (SceneManager.GetActiveScene().buildIndex != 1)
        {
            StartCoroutine(LoadThenJoin());
        }
    }

    IEnumerator LoadThenHost()
    {
        while (SceneManager.GetActiveScene().buildIndex != 1)
        {
            yield return null;
        }

        uNetTransport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        uNetTransport.ServerListenPort = int.Parse(port);
        NetworkManager.Singleton.StartHost();
    }

    IEnumerator LoadThenJoin()
    {
        while (SceneManager.GetActiveScene().buildIndex != 1)
        {
            yield return null;
        }
        uNetTransport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        uNetTransport.ConnectAddress = ip;
        uNetTransport.ConnectPort = int.Parse(port);
        NetworkManager.Singleton.StartClient();
    }
}
