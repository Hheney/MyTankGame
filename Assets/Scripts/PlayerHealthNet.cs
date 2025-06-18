using UnityEngine;
using Unity.Netcode;
using Unity.Collections;

public class PlayerHealthNet : NetworkBehaviour
{
    public NetworkVariable<int> health = new NetworkVariable<int>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<FixedString128Bytes> userId = new NetworkVariable<FixedString128Bytes>("player", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            health.Value = UiManagerTank.Instance.maxHealth; // write
            health.OnValueChanged += OnHealthChanged; // 이벤트 핸들러 등록
            userId.Value = GameManager.Instance.UserId;
            Debug.Log($"health = {health.Value}");
        }
        base.OnNetworkSpawn();
    }

    void OnHealthChanged(int prevValue, int newValue)
    {
        // newValue가 health.Value가 됨
        UiManagerTank.Instance.health = newValue;
        UiManagerTank.Instance.updateTextUserInfo();
        UiManagerTank.Instance.updateSliderHealth();
        //Debug.Log($"health = {UiManagerTank.Instance.health}");
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            health.OnValueChanged -= OnHealthChanged; // 이벤트 핸들러 해제
        }
        base.OnNetworkDespawn();
    }

    public void decHealth()
    {
        int curValue = health.Value - 1;
        if (curValue < 0) curValue = 0;
        health.Value = curValue;
        Debug.Log($"health = {health.Value}");
    }

    [Rpc(SendTo.Owner)]
    public void decHealthRpc() // 확실하게 RPC로 NetworkVariable 갱신
    {
        decHealth();
    }

    private void OnGUI() // GUI를 처리하는 이벤트 처리기
    {
        Vector3 offset = new Vector3(-3.0f, 4.0f, 0.0f);
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position + offset);
        Rect rect = new Rect(0, 0, 100, 50);
        rect.x = pos.x;
        rect.y = Screen.height - pos.y;
        // GUI style 사용
        GUIStyle style = new GUIStyle();
        style.richText = true; // HTML 태그 사용을 허용
        string text = $"<color=blue>{userId.Value}</color>: <color=red>{health.Value}</color>";
        GUI.Label(rect, text);
    }
}
