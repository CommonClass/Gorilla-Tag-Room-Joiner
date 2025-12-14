using BepInEx;
using GorillaLocomotion;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RoomJoiner
{
    [BepInPlugin("roomjoiner", "Room Joiner", "1.0.0")]
    public class RoomJoinerMain : BaseUnityPlugin
    {
        private string roomName = "";
        private bool isWASDEnabled = false;
        private Rect windowRect = new Rect(100, 100, 300, 130);

        private static float yaw;
        private static float pitch;

        private void OnGUI()
        {
            windowRect = GUILayout.Window(12345, windowRect, RenderWindow, "Room UI # discord.gg/7JBXPKz7yU");
        }

        private void RenderWindow(int windowID)
        {
            GUILayout.BeginVertical();
            roomName = GUILayout.TextField(roomName, 25, GUILayout.Width(280));
            if (GUILayout.Button("Join Room"))
                JoinRoom(roomName);
            if (GUILayout.Button("Disconnect"))
                Disconnect();
            isWASDEnabled = GUILayout.Toggle(isWASDEnabled, "Enable WASD");
            GUILayout.EndVertical();
            GUI.DragWindow();
        }

        private static void JoinRoom(string roomName)
        {
            if (!string.IsNullOrEmpty(roomName))
                GorillaComputer.instance.networkController.AttemptToJoinSpecificRoom(roomName, JoinType.Solo);
        }

        private static void HandleWASDFly()
        {
            /// <summary>
            /// shitty ass WASD but idc it works fine
            /// </summary>

            bool Forward = UnityInput.Current.GetKey(KeyCode.W);
            bool Left = UnityInput.Current.GetKey(KeyCode.A);
            bool Back = UnityInput.Current.GetKey(KeyCode.S);
            bool Right = UnityInput.Current.GetKey(KeyCode.D);
            bool Up = UnityInput.Current.GetKey(KeyCode.Space);
            bool Down = UnityInput.Current.GetKey(KeyCode.LeftControl);
            bool SpeedUp = UnityInput.Current.GetKey(KeyCode.LeftShift);
            bool SlowDown = UnityInput.Current.GetKey(KeyCode.LeftAlt);

            float moveSpeed = 5f;
            if (SpeedUp) moveSpeed *= 2f;
            else if (SlowDown) moveSpeed *= 0.5f;

            Transform transform = GTPlayer.Instance.GetControllerTransform(false).parent;

            if (Forward) transform.position += transform.forward * (Time.deltaTime * moveSpeed);
            if (Back) transform.position -= transform.forward * (Time.deltaTime * moveSpeed);
            if (Left) transform.position -= transform.right * (Time.deltaTime * moveSpeed);
            if (Right) transform.position += transform.right * (Time.deltaTime * moveSpeed);
            if (Up) transform.position += Vector3.up * (Time.deltaTime * moveSpeed);
            if (Down) transform.position -= Vector3.up * (Time.deltaTime * moveSpeed);

            if (Mouse.current.rightButton.isPressed)
            {
                Vector2 delta = Mouse.current.delta.ReadValue();
                yaw += delta.x * 100 * Time.deltaTime;
                pitch -= delta.y * 100 * Time.deltaTime;
                pitch = Mathf.Clamp(pitch, -89f, 89f);
                transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
            }
        }

        private static void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        private void Update()
        {
            if (isWASDEnabled)
            {
                //better than setting it to zero, which makes you slowly fall, take notes skids.
                GTPlayer.Instance.GetComponent<Rigidbody>().velocity = Vector3.up * Physics.gravity.magnitude * Time.deltaTime;
                HandleWASDFly();
            }
        }
    }
}
