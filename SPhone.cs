
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SPhone : UdonSharpBehaviour {
  [SerializeField] private GameObject sphoneCamera;

  private bool shouldUpdate = false;
  private bool toCameraEnable = false;

  void Start() {
  }

  private void Update() {
    if (shouldUpdate) {
      // オーナーでのみ実行
      if (Networking.IsOwner(Networking.LocalPlayer, this.gameObject)) {
        if(toCameraEnable) {
          //すべてのプレイヤーに実行させる（同期させる）命令
          SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetCameraEnable");
        } else {
          SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SetCameraDisable");
        }
      }
      shouldUpdate = false;
    }
  }

  public override void OnPickupUseDown() {
    //スイッチを押した人を「オーナ」にする命令
    if (!Networking.IsOwner(Networking.LocalPlayer, this.gameObject)) {
      Networking.SetOwner(Networking.LocalPlayer, this.gameObject);
    }

    if (sphoneCamera != null) {
      toCameraEnable = !sphoneCamera.activeSelf;
    }
    shouldUpdate = true;
  }

  public override void OnPlayerJoined(VRCPlayerApi player) {
    shouldUpdate = true;
  }

  public void SetCameraEnable() {
    if (sphoneCamera != null) {
      sphoneCamera.SetActive(true);
    }
  }

  public void SetCameraDisable() {
    if (sphoneCamera != null) {
      sphoneCamera.SetActive(false);
    }
  }

}
