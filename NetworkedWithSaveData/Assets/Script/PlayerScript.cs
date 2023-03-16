using Mirror;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

namespace QuickStart
{
    public class PlayerScript : NetworkBehaviour
    {
        #region Server Stuff
        public DataClass playerAccount;
        public TextMeshPro playerInfo;
        public GameObject weapon;
        
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
            Camera.main.transform.localPosition = new Vector3(0, .75f, 0);
            Camera.main.gameObject.AddComponent<FPMouseLook>();
            Camera.main.gameObject.GetComponent<FPMouseLook>().playerObj = transform;

            weapon.transform.SetParent(Camera.main.transform);

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
        #endregion

        [Header("General Player Stuff")]
        public CharacterController playerController;
        public float speed;
        public float gravity = -9.8f;
        public float jumpHeight = 5.0f;
        Vector3 vel;
        public Transform GC;
        public float groundDist;
        public LayerMask groundMask;
        bool isGrounded;
        Vector3 newScale;

        [Header("GunStuff")]
        float shotTimer = 0;
        public GameObject bullet;
        public Transform bulletStart;
        float mass = 3f; // defines the character mass
        Vector3 impact = Vector3.zero;

        public float SaveTimer = 20f;

        void Update()
        {
            if (!isLocalPlayer) { return; }

            isGrounded = Physics.CheckSphere(GC.position, groundDist, groundMask);

            if (isGrounded && vel.y < 0)
            {
                vel.y = -2;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            playerController.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                vel.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            }

            vel.y += gravity * Time.deltaTime;
            playerController.Move(vel * Time.deltaTime);

            // apply the impact force:
            if (impact.magnitude > 0.2) playerController.Move(impact * Time.deltaTime);
            // consumes the impact energy each cycle:
            impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);

            //shots
            if (shotTimer > 0)
            {
                shotTimer -= Time.deltaTime;
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                shotTimer = .05f;
                CmdShootRay();
            }

            if((playerScore * .1f) + 1 > 0)
            {
                newScale = new Vector3((playerScore * .1f) + 1, (playerScore * .1f) + 1, (playerScore * .1f) + 1);
            }
            else
            {
                newScale = new Vector3(.1f, .1f, .1f);
            }
            
            //Set Player scale
            transform.localScale = newScale;

            if(SaveTimer >= 0)
            {
                SaveTimer -= Time.deltaTime;
            }
            else
            {
                SaveTimer = 20f;
                playerAccount.savedScore = playerScore;
                string account = JsonUtility.ToJson(playerAccount);
                StartCoroutine(AddAccount(account));
            }
        }

        #region More Server Stuff
        [Command]
        void CmdShootRay()
        {
            RpcFireWeapon();
        }

        [ClientRpc]
        void RpcFireWeapon()
        {
            GameObject newBullet = Instantiate(bullet, bulletStart.position, bulletStart.rotation);
            newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.forward * 10f;
            newBullet.GetComponent<BulletScript>().shotby = gameObject;
        }
        #endregion

        public void AddImpact(Vector3 dir, float force)
        {
            dir.Normalize();
            if (dir.y < 0) dir.y = -dir.y;
            impact += dir.normalized * force / mass;
        }

        #region Save Account Data
        IEnumerator AddAccount(string json)
        {
            using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:3000/saveAccount", json))
            {
                request.SetRequestHeader("content-type", "application/json");
                request.uploadHandler.contentType = "application/json";
                request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    Debug.Log("DataObj Posted");
                }
                request.uploadHandler.Dispose();
            }
        }
        #endregion
    }
}
