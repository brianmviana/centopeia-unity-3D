using UnityEngine;
using UnityEngine.Networking.Match;
using TMPro;

public class RoomListItem : MonoBehaviour {

    public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);
    public JoinRoomDelegate joinRoomCallback;

    [SerializeField]
    private TextMeshProUGUI roomNameText;

    private MatchInfoSnapshot match;

    public void Setup(MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallback) {
        match = _match;
        joinRoomCallback = _joinRoomCallback;
        roomNameText.text = match.name + "(" + match.currentSize +  " / " + match.maxSize + ")";
    }

    public void JoinGame() {
        joinRoomCallback.Invoke(match);
    }

}
