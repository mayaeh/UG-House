using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

// https://fabniworld.com/ja/vrchat-udon-sync-mirror/
// https://note.com/shoninvrc/n/nddf2bf676b9b

[AddComponentMenu("Udon Sharp/Utilities/Interact Toggle")]
public class Teppan_Switch : UdonSharpBehaviour {
  [SerializeField] private GameObject toggleTarget;
  [SerializeField] private GameObject buttonOn;
  [SerializeField] private GameObject buttonOff;

  private bool isEnable = false;
  private int syncWaitCount = 0;

  void start() {
  }

  // 実際の実行までに wait を入れる
  private void Update() {
    // オーナーでのみ実行
    if (Networking.IsOwner(Networking.LocalPlayer, this.gameObject)) {
      if(syncWaitCount != 0) {
        syncWaitCount --;
        if(syncWaitCount <= 0) {
          //すべてのプレイヤーに「toggleEnable() or toggleDisable()」を実行させる（同期させる）命令
          if (isEnable) {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "toggleEnable");
          }
          else {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "toggleDisable");
          }
          syncWaitCount = 0;
        }
      }
    }
  }

  public override void OnPlayerJoined(VRCPlayerApi player) {
    // オーナーでのみ実行
    if (Networking.IsOwner(Networking.LocalPlayer, this.gameObject)) {
      if (syncWaitCount == 0) {
        syncWaitCount = 10;
      }
    }
  }

  public override void Interact() {
    //スイッチを押した人を「オーナ」にする命令
    if (!Networking.IsOwner(Networking.LocalPlayer, this.gameObject)) Networking.SetOwner(Networking.LocalPlayer, this.gameObject);

    if (!isEnable) {
      isEnable = true;
    }
    else {
      isEnable = false;
    }
    syncWaitCount = 10;
  }

　//同期させる関数は「public」
  public void toggleEnable() {
    if (!toggleTarget.activeSelf) {
      toggleTarget.SetActive(true);
      buttonOn.SetActive(true);
      buttonOff.SetActive(false);
    }
  }
  public void toggleDisable() {
    if (toggleTarget.activeSelf) {
      toggleTarget.SetActive(false);
      buttonOn.SetActive(false);
      buttonOff.SetActive(true);
    }
  }
}