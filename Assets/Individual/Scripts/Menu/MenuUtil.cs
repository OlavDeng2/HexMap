using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MenuUtil : MonoBehaviour {
	[Header("Connection")]
	public InputField joinIpField;
	public Button joinServerButton;
	public Button startServerButton;

	[Header("Settings")]
	public Dropdown qualityDropdown;
	public Dropdown resolutionDropdown;
	public Toggle fullscreenToggle;

	private NetworkManagerCustom networkManager;

	void Start() {
		networkManager = NetworkManagerCustom.GetInstance();

		if (networkManager) {
			networkManager.OnServerStopped += OnServerStopped;
			networkManager.OnClientStopped += OnClientStopped;
			networkManager.OnClientConnectionError += OnClientError;
		}

		// In case of disconnect
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		// Quality
		qualityDropdown.ClearOptions();
		qualityDropdown.AddOptions(new List<string>(QualitySettings.names));
		qualityDropdown.value = QualitySettings.GetQualityLevel();

		// Resolutions
		Resolution[] screenResolutions = Screen.resolutions;
		Resolution currentRes = Screen.currentResolution;

		resolutionDropdown.ClearOptions();
		List<string> resolutions = new List<string>(screenResolutions.Length);
		int currentResolution = 0;

		for (int i = 0; i < screenResolutions.Length; i++) {
			string resolutionString = String.Format("{0} x {1}", screenResolutions[i].width, screenResolutions[i].height);

			if (resolutions.Contains(resolutionString)) 
				continue;

			resolutions.Add(resolutionString);

			if (currentRes.Equals(screenResolutions[i]))
				currentResolution = resolutions.Count - 1;
		}

		resolutionDropdown.AddOptions(resolutions);
		resolutionDropdown.value = currentResolution;

		// Fullscreen
		fullscreenToggle.isOn = Screen.fullScreen;
	}

	void OnDestroy() {
		if (networkManager) {
			networkManager.OnServerStopped -= OnServerStopped;
			networkManager.OnClientStopped -= OnClientStopped;
			networkManager.OnClientConnectionError -= OnClientError;
		}
	}

	public void OnQualitySelect(int index) {
		QualitySettings.SetQualityLevel(index);
	}

	public void OnResolutionSelect(int index) {
		string[] resolutionStrings = resolutionDropdown.options[index].text.Split(new [] {" x "}, StringSplitOptions.RemoveEmptyEntries);

		Screen.SetResolution(int.Parse(resolutionStrings[0]), int.Parse(resolutionStrings[1]), Screen.fullScreen);
	}

	public void OnFullscreenToggle(bool value) {
		Screen.fullScreen = value;
	}

	public void StartServer() {
		startServerButton.interactable = false;

		networkManager.StartHost();
	}

	public void JoinServer() {
		if (string.IsNullOrEmpty(joinIpField.text)) {
			return;
		}

		joinServerButton.interactable = false;

		int port;
		string adress;

		ParseIP(joinIpField.text, out adress, out port);

		networkManager.networkPort = port;
		networkManager.networkAddress = adress;

		networkManager.StartClient();
	}

	void OnClientError(NetworkError error) {
		joinServerButton.interactable = true;
	}

	void OnClientStopped() {
		joinServerButton.interactable = true;
	}

	void OnServerStopped() {
		startServerButton.interactable = true;
	}

	public void Disconnect() {
		networkManager.StopHost();
	}

    public void Exit() {
		#if UNITY_EDITOR
			Debug.Break();
		#else
			Application.Quit();
		#endif
    }

	public void ParseIP(string input, out string adress, out int port, int defaultPort = 7777) {
		string[] ip = input.Split(':');
		adress = ip[0];

		if (!(ip.Length > 1 && int.TryParse(ip[1], out port))) {
			port = defaultPort;
		}
	}
}
