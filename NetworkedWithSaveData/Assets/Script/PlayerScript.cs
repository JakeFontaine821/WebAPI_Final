using Mirror;
using UnityEngine;
using TMPro;

namespace QuickStart
{
    public class PlayerScript : NetworkBehaviour
    {
        public DataClass playerAccount;
        public TextMeshPro playerInfo;

        [SyncVar(hook = nameof(OnNameChanged))]
        public string playerName;
        void OnNameChanged(string _Old, string _New)
        {
            playerInfo.text = playerName + " | " + playerScore;
        }

        [SyncVar(hook = nameof(OnScoreChanged))]
        public int playerScore;
        void OnScoreChanged(int _Old, int _New)
        {
            playerInfo.text = playerName + " | " + playerScore;
        }

        public override void OnStartLocalPlayer()
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 0, 0);

            playerAccount = AccountManager.Instance.Account;

            CmdSetupPlayer(playerAccount.username, playerAccount.savedScore);
        }

        [Command]
        public void CmdSetupPlayer(string _name, int _score)
        {
            // player info sent to server, then server updates sync vars which handles it on all clients
            playerName = _name;
            playerScore = _score;
        }

        [Command]
        public void AddToScore(int _score)
        {
            playerScore += _score;
        }

        void Update()
        {
            if (!isLocalPlayer) { return; }

            float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
            float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

            transform.Rotate(0, moveX, 0);
            transform.Translate(0, 0, moveZ);

            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                AddToScore(1);
            }
        }
    }
}
