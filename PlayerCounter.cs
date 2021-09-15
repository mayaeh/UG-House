
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerCounter : UdonSharpBehaviour {
  [SerializeField] TMPro.TextMeshPro TMProPlayerCounter;

  private int playerCount;
  private bool shouldUpdate = false;
  private float waitTime = 2.0f; // 2sec
  private float timer = 0.0f;

  // OnPlayerLeft の時点では GetPlayerCount() しても減っていないため遅延をつける
  void Update() {
    if (shouldUpdate) {
      timer += Time.deltaTime;
      if (timer > waitTime) {
        UpdateCount();
        timer = 0.0f;
      }
    }
  }

  void Start() {
  }

  public override void OnPlayerJoined(VRCPlayerApi player) {
    DoUpdate();
  }

  public override void OnPlayerLeft(VRCPlayerApi player) {
    DoUpdate();
  }

  private void DoUpdate() {
    timer = 0.0f;
    shouldUpdate = true;
  }

  private void UpdateCount() {
    playerCount = VRCPlayerApi.GetPlayerCount();
    if (playerCount < 10) {
      TMProPlayerCounter.text = "0" + playerCount.ToString();
    } else {
      TMProPlayerCounter.text = playerCount.ToString();
    }
  }

}
