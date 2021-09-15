using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

// https://fabniworld.com/ja/vrchat-udon-sync-mirror/
// https://docs.unity3d.com/ScriptReference/Time-deltaTime.html

public class DoorKnock : UdonSharpBehaviour {
  [SerializeField] private GameObject knockAudio_mainroom;
  [SerializeField] private GameObject knockAudio_bedroom;
//  [SerializeField] private float waitSeconds;

  private float waitTime = 4.0f; // 4sec
  private float timer = 0.0f;

  void start() {
//    if (waitSeconds) waittime = waitSeconds;
  }

  private void Update() {
    // オーナーでのみ実行
    if (Networking.IsOwner(Networking.LocalPlayer, this.gameObject)) {
      if (knockAudio_mainroom.activeSelf) {
        timer += Time.deltaTime;
        if (timer > waitTime) {
          SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "toggleDisable");
          timer = 0.0f;
        }
      }
    }
  }

  public override void Interact() {
    //スイッチを押した人を「オーナ」にする命令
    if (!Networking.IsOwner(Networking.LocalPlayer, this.gameObject)) Networking.SetOwner(Networking.LocalPlayer, this.gameObject);

    SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "toggleEnable");
  }

  public void toggleEnable() {
    knockAudio_mainroom.SetActive(true);
    knockAudio_bedroom.SetActive(true);
  }
  public void toggleDisable() {
    knockAudio_mainroom.SetActive(false);
    knockAudio_bedroom.SetActive(false);
  }

}
