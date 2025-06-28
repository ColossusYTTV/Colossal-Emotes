// How come I get harassed on discord about fixing ts but never actually see anyone ingame using it 😭, I got like 100 ppl telling me to fix it the day it broke

using BepInEx;
using Colossal.Patches;
using Photon.Pun;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;

namespace Colossal
{
    public class MenuOption
    {
        public string Name;
        public bool submenu;
    }
    public class Plugin : MonoBehaviour
    {
        public static bool oculus = false;
        public static GameObject ScriptHolder;
        private float X = -1;

        public static GameObject Menu;
        public static Text menuText;
        public static MenuOption[] CurrentViewingMenu = null;
        public static MenuOption[][] Pages = new MenuOption[8][];  // An array to hold all pages
        public static MenuOption[] Page1 = null;
        public static MenuOption[] Page2 = null;
        public static MenuOption[] Page3 = null;
        public static MenuOption[] Page4 = null;
        public static MenuOption[] Page5 = null;
        public static MenuOption[] Page6 = null;
        public static int SelectedOptionIndex = 0;
        public static int currentPage = 0;

        private static int previousSerializationRate = -1;
        private static Vector3 previousPos;
        private static Animator animator;

        public static bool emoting = false;
        private bool coolDown = false;
        private bool imToLazy = false;
        private bool wasRightTriggerPressed = false;

        #region Start and Update
        public void Start()
        {
            HarmonyPatches.ApplyHarmonyPatches();


            // Enables support for Rift
            string[] oculusDlls = Directory.GetFiles(Environment.CurrentDirectory, "OculusXRPlugin.dll", SearchOption.AllDirectories);
            if (oculusDlls.Length > 0)
            {
                oculus = true;
            }


            // Making holder for scripts
            if (ScriptHolder == null)
            {
                ScriptHolder = new GameObject();
                ScriptHolder.name = "ColossalEmotes (ScriptHolder)";

                ScriptHolder.AddComponent<AssetBundleLoader>();// Loading asset bundles
            }


            (Menu, menuText) = GUICreator.CreateTextGUI("", "ColossalEmotes", TextAnchor.MiddleCenter, new Vector3(0, 0f, 2)); // Please refer to what I commented at the top of GUICreator.CreateTextGUI
            if (Menu != null)
                Menu.SetActive(false);


            // Adding options to the menu
            Page1 = new MenuOption[14];
            Page1[0] = new MenuOption { Name = "--->", submenu = true };
            Page1[1] = new MenuOption { Name = "All I Want Is You" }; // This is totally a fortnite emote
            Page1[2] = new MenuOption { Name = "Griddy" };
            Page1[3] = new MenuOption { Name = "Lucid Dreams" };
            Page1[4] = new MenuOption { Name = "Empty Out Your Pockets" };
            Page1[5] = new MenuOption { Name = "Scenario" };
            Page1[6] = new MenuOption { Name = "Caffeinated" };
            Page1[7] = new MenuOption { Name = "Miss The Rage" };
            Page1[8] = new MenuOption { Name = "Tek It" };
            Page1[9] = new MenuOption { Name = "California Gurls" };
            Page1[10] = new MenuOption { Name = "Miku Miku Beam" };
            Page1[11] = new MenuOption { Name = "Miku Live" };
            Page1[12] = new MenuOption { Name = "Vegetable Juice" };
            Page1[13] = new MenuOption { Name = "Caramelldansen" };

            Page2 = new MenuOption[13];
            Page2[0] = new MenuOption { Name = "--->", submenu = true };
            Page2[1] = new MenuOption { Name = "<---", submenu = true };
            Page2[2] = new MenuOption { Name = "Gangnam Style" };
            Page2[3] = new MenuOption { Name = "Never Gonna" };
            Page2[4] = new MenuOption { Name = "Feel Like God" };
            Page2[5] = new MenuOption { Name = "Macarena" };
            Page2[6] = new MenuOption { Name = "Cupids Arrow" };
            Page2[7] = new MenuOption { Name = "Paws And Claws" };
            Page2[8] = new MenuOption { Name = "Jabba Switch Way" };
            Page2[9] = new MenuOption { Name = "Renegade" };
            Page2[10] = new MenuOption { Name = "Evil Plan" };
            Page2[11] = new MenuOption { Name = "Smooth Moves" };
            Page2[12] = new MenuOption { Name = "Rat Dance" };

            Page3 = new MenuOption[12];
            Page3[0] = new MenuOption { Name = "--->", submenu = true };
            Page3[1] = new MenuOption { Name = "<---", submenu = true };
            Page3[2] = new MenuOption { Name = "Its Dynamite" };
            Page3[3] = new MenuOption { Name = "My World" };
            Page3[4] = new MenuOption { Name = "Last Forever" };
            Page3[5] = new MenuOption { Name = "Savage" };
            Page3[6] = new MenuOption { Name = "Say So" };
            Page3[7] = new MenuOption { Name = "Rollie" };
            Page3[8] = new MenuOption { Name = "Out West" };
            Page3[9] = new MenuOption { Name = "Toosie Slide" };
            Page3[10] = new MenuOption { Name = "Marsh Walk" };
            Page3[11] = new MenuOption { Name = "Boogie Bomb" };

            Page4 = new MenuOption[12];
            Page4[0] = new MenuOption { Name = "--->", submenu = true };
            Page4[1] = new MenuOption { Name = "<---", submenu = true };
            Page4[2] = new MenuOption { Name = "Popular Vibe" };
            Page4[3] = new MenuOption { Name = "Best Mates" };
            Page4[4] = new MenuOption { Name = "Take The L" };
            Page4[5] = new MenuOption { Name = "Orange Justice" };
            Page4[6] = new MenuOption { Name = "Electro Swing" };
            Page4[7] = new MenuOption { Name = "Fresh" };
            Page4[8] = new MenuOption { Name = "Crabby" };
            Page4[9] = new MenuOption { Name = "Boogie Down" };
            Page4[10] = new MenuOption { Name = "Zany" };
            Page4[11] = new MenuOption { Name = "Flapper" };

            Page5 = new MenuOption[12];
            Page5[0] = new MenuOption { Name = "--->", submenu = true };
            Page5[1] = new MenuOption { Name = "<---", submenu = true };
            Page5[2] = new MenuOption { Name = "Electro Shuffle" };
            Page5[3] = new MenuOption { Name = "Dance Moves" };
            Page5[4] = new MenuOption { Name = "Break Neck" };
            Page5[5] = new MenuOption { Name = "Breakin" };
            Page5[6] = new MenuOption { Name = "Crack Down" };
            Page5[7] = new MenuOption { Name = "Groove Jam" };
            Page5[8] = new MenuOption { Name = "Robot" };
            Page5[9] = new MenuOption { Name = "Disco Fever" };
            Page5[10] = new MenuOption { Name = "Boneless" };
            Page5[11] = new MenuOption { Name = "Back Stroke" };

            Page6 = new MenuOption[13];
            Page6[0] = new MenuOption { Name = "<---", submenu = true };
            Page6[1] = new MenuOption { Name = "Clean Groove" };
            Page6[2] = new MenuOption { Name = "Blinding Lights" };
            Page6[3] = new MenuOption { Name = "Distraction" };
            Page6[4] = new MenuOption { Name = "Chicken Wing" };
            Page6[5] = new MenuOption { Name = "Break Down" };
            Page6[6] = new MenuOption { Name = "Poki" };
            Page6[7] = new MenuOption { Name = "Pull Up" };
            Page6[8] = new MenuOption { Name = "JumpStyle" };
            Page6[9] = new MenuOption { Name = "Billy Bounce" };
            Page6[10] = new MenuOption { Name = "Star Power" };
            Page6[11] = new MenuOption { Name = "Floss" };
            Page6[12] = new MenuOption { Name = "Day Dream" };


            Pages[0] = Page1;
            Pages[1] = Page2;
            Pages[2] = Page3;
            Pages[3] = Page4;
            Pages[4] = Page5;
            Pages[5] = Page6;
            CurrentViewingMenu = Pages[0]; // Starting on the first page
        }
        public void Update()
        {
            if (ScriptHolder != null && Menu != null && menuText != null)
            {
                if (ScriptHolder.GetComponent<AssetBundleLoader>()) // Just making sure everything is loaded, there is not reason for AssetBundleLoader to not be added but better safe than sorry 😭
                {
                    if (AssetBundleLoader.KyleRobot != null)
                    {
                        if (AssetBundleLoader.KyleRobot.transform.GetChild(0).gameObject.GetComponent<Renderer>().renderingLayerMask != 0)
                            AssetBundleLoader.KyleRobot.transform.GetChild(0).gameObject.GetComponent<Renderer>().renderingLayerMask = 0; // Making kyle invisible

                        if (emoting) // Only run during a emote
                        {
                            // Making you float
                            GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity = Vector3.zero;
                            GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(-Physics.gravity, ForceMode.Acceleration);


                            // Enables freecam
                            Fly();


                            // Setting positions and rotations for the actual emotes
                            VRRig.LocalRig.transform.position = AssetBundleLoader.KyleRobot.transform.Find("ROOT/Hips/Spine1/Spine2").transform.position - (AssetBundleLoader.KyleRobot.transform.Find("ROOT/Hips/Spine1/Spine2").transform.right / 2.5f);
                            VRRig.LocalRig.transform.rotation = Quaternion.Euler(new Vector3(0f, AssetBundleLoader.KyleRobot.transform.Find("ROOT/Hips/Spine1/Spine2").transform.rotation.eulerAngles.y, 0f));

                            VRRig.LocalRig.leftHand.rigTarget.transform.position = AssetBundleLoader.KyleRobot.transform.Find("ROOT/Hips/Spine1/Spine2/LeftShoulder/LeftUpperArm/LeftArm/LeftHand").transform.position;
                            VRRig.LocalRig.rightHand.rigTarget.transform.position = AssetBundleLoader.KyleRobot.transform.Find("ROOT/Hips/Spine1/Spine2/RightShoulder/RightUpperArm/RightArm/RightHand").transform.position;

                            VRRig.LocalRig.leftHand.rigTarget.transform.rotation = AssetBundleLoader.KyleRobot.transform.Find("ROOT/Hips/Spine1/Spine2/LeftShoulder/LeftUpperArm/LeftArm/LeftHand").transform.rotation * Quaternion.Euler(0, 0, 75);
                            VRRig.LocalRig.rightHand.rigTarget.transform.rotation = AssetBundleLoader.KyleRobot.transform.Find("ROOT/Hips/Spine1/Spine2/RightShoulder/RightUpperArm/RightArm/RightHand").transform.rotation * Quaternion.Euler(180, 0, -75);

                            VRRig.LocalRig.head.rigTarget.transform.rotation = AssetBundleLoader.KyleRobot.transform.Find("ROOT/Hips/Spine1/Spine2/Neck/Head").transform.rotation * Quaternion.Euler(0f, 0f, 90f);
                        }

                        EmoteSelect();
                    }
                    else
                    {
                        Debug.Log("[EMOTE] KyleRobot is null");
                    }
                }
                else
                {
                    Debug.Log("[EMOTE] ScriptHolder doesnt have AssetBundleLoader");
                }
            }
            else
            {
                Debug.Log("[EMOTE] ScriptHolder or Menu is null");
            }
        }
        #endregion

        #region Emote Handling
        public void Emote(string emoteName)
        {
            if (AssetBundleLoader.KyleRobot == null)
            {
                Debug.Log("[EMOTE] KyleRobot is null");
                return;
            }


            if (VRRig.LocalRig.enabled) // Enabling the ability to move/do stuff with the rig
                VRRig.LocalRig.enabled = false;


            previousSerializationRate = PhotonNetwork.SerializationRate; // More movement show up server sided
            PhotonNetwork.SerializationRate *= 3;


            previousPos = GorillaTagger.Instance.transform.position; // Saving position to return to
            Camera.main.transform.rotation = Quaternion.Euler(0f, 180f, 0f); // Flipping around to look at yourself


            // Moving Kyle to your position
            AssetBundleLoader.KyleRobot.transform.position = VRRig.LocalRig.transform.Find("RigAnchor/rig/body").position - new Vector3(0f, 1.15f, 0f);
            AssetBundleLoader.KyleRobot.transform.rotation = VRRig.LocalRig.transform.Find("RigAnchor/rig/body").rotation;


            //DisableCosmetics();


            if (animator == null)
            {
                if (AssetBundleLoader.KyleRobot.GetComponentInChildren<Animator>() != null)
                    animator = AssetBundleLoader.KyleRobot.GetComponentInChildren<Animator>(); // Declaring the animator
                else
                    Debug.Log("[EMOTE] Could not find animator");
            }
            if (animator != null)
            {
                animator.Play(emoteName); // Plays the animation/emote

                // Audio
                if (AssetBundleLoader.audioSource != null)
                    AssetBundleLoader.PlayAudioByName(emoteName);
                emoting = true;
            }
        }
        public void StopEmote()
        {
            if (emoting)
            {
                emoting = false;


                if (!VRRig.LocalRig.enabled) // Disabling the ability to move/do stuff with the rig
                    VRRig.LocalRig.enabled = true;


                if (PhotonNetwork.InRoom)
                    PhotonNetwork.SerializationRate = previousSerializationRate; // Setting your movements back to normal


                GorillaTagger.Instance.transform.position = previousPos; // Setting your position back to normal
                Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, 0f); // Putting you back inside your own body


                //EnableCosmetics();


                if (previousSerializationRate > 0)
                    PhotonNetwork.SerializationRate = previousSerializationRate;


                animator.Play("idle");

                AssetBundleLoader.audioSource.Stop();
                if (PhotonNetwork.InRoom)
                {
                    GorillaTagger.Instance.myRecorder.SourceType = Photon.Voice.Unity.Recorder.InputSourceType.Microphone;
                    GorillaTagger.Instance.myRecorder.RestartRecording();
                }
            }
        }

        public void EmoteSelect()
        {
            if (Menu == null || menuText == null) return; // This shouldnt happen but just incase it gets destroyed

            // PC Controls
            bool bKeyHeld = UnityInput.Current.GetKey(KeyCode.B);
            if (bKeyHeld) // Start selection when holding B
            {
                if (!Menu.activeSelf)
                    Menu.SetActive(true); // Toggle the menu visibility

                float scrollInput = UnityInput.Current.mouseScrollDelta.y;
                if (scrollInput > 0)
                {
                    if (SelectedOptionIndex == 0)
                        SelectedOptionIndex = CurrentViewingMenu.Length - 1;
                    else
                        SelectedOptionIndex--;
                }
                else if (scrollInput < 0)
                {
                    if (SelectedOptionIndex + 1 == CurrentViewingMenu.Length)
                        SelectedOptionIndex = 0;
                    else
                        SelectedOptionIndex++;
                }


                MenuDisplay(); // Updates the displayed menu text
            }
            if (UnityInput.Current.GetKeyUp(KeyCode.B) && !coolDown) // If B key is released
            {
                if (Menu.activeSelf)
                    Menu.SetActive(false); // Toggle the menu visibility

                if (!CurrentViewingMenu[SelectedOptionIndex].submenu)
                    Emote(CurrentViewingMenu[SelectedOptionIndex].Name.Replace(" ", "").ToLower()); // Dynamically play emotes based off the name shown on the menu. If you want to add more emotes make sure the animation state name is lowercase and is the same as shown on the menu
                else
                {
                    if (CurrentViewingMenu[SelectedOptionIndex].Name.Contains(">"))
                    {
                        if (currentPage < Pages.Length - 1)
                        {
                            currentPage++;  // Increment to the next page
                            CurrentViewingMenu = Pages[currentPage];
                            SelectedOptionIndex = 0;  // Optionally reset the selected index when changing pages
                        }
                    }
                    else if (CurrentViewingMenu[SelectedOptionIndex].Name.Contains("<"))
                    {
                        if (currentPage > 0)
                        {
                            currentPage--;  // Decrement to the previous page
                            CurrentViewingMenu = Pages[currentPage];
                            SelectedOptionIndex = 0;  // Optionally reset the selected index when changing pages
                        }
                    }
                }

                coolDown = true;
            }

            if (UnityInput.Current.GetKey(KeyCode.V) && !coolDown) // Stop Emote
            {
                StopEmote();
                coolDown = true;
            }


            // VR Controls
            float inputAxis = Controls.RightJoystickAxis().y;
            if (XRSettings.isDeviceActive)
            {
                if (Controls.RightTrigger())  // If Right Trigger is pressed
                {
                    if (!Menu.activeSelf)
                        Menu.SetActive(true); // Toggle the menu visibility

                    // Update the selected option based on joystick input
                    if (inputAxis > 0 && !imToLazy)
                    {
                        if (SelectedOptionIndex == 0)
                            SelectedOptionIndex = CurrentViewingMenu.Length - 1;
                        else
                            SelectedOptionIndex--;

                        imToLazy = true;
                    }
                    else if (inputAxis < 0 && !imToLazy)
                    {
                        if (SelectedOptionIndex + 1 == CurrentViewingMenu.Length)
                            SelectedOptionIndex = 0;
                        else
                            SelectedOptionIndex++;

                        imToLazy = true;
                    }

                    MenuDisplay(); // Update the displayed menu text
                    wasRightTriggerPressed = true;
                }
                else if (wasRightTriggerPressed && !coolDown)  // If Right Trigger was just released and no cooldown
                {
                    if (Menu.activeSelf)
                        Menu.SetActive(false);  // Toggle the menu visibility


                    if (!CurrentViewingMenu[SelectedOptionIndex].submenu)
                        Emote(CurrentViewingMenu[SelectedOptionIndex].Name.Replace(" ", "").ToLower()); // Dynamically play emotes based off the name shown on the menu. If you want to add more emotes make sure the animation state name is lowercase and is the same as shown on the menu
                    else
                    {
                        if (CurrentViewingMenu[SelectedOptionIndex].Name.Contains(">"))
                        {
                            if (currentPage < Pages.Length - 1)
                            {
                                currentPage++;  // Increment to the next page
                                CurrentViewingMenu = Pages[currentPage];
                                SelectedOptionIndex = 0;  // Optionally reset the selected index when changing pages
                            }
                        }
                        else if (CurrentViewingMenu[SelectedOptionIndex].Name.Contains("<"))
                        {
                            if (currentPage > 0)
                            {
                                currentPage--;  // Decrement to the previous page
                                CurrentViewingMenu = Pages[currentPage];
                                SelectedOptionIndex = 0;  // Optionally reset the selected index when changing pages
                            }
                        }
                    }


                    coolDown = true;
                    wasRightTriggerPressed = false;
                }

                if (Controls.LeftTrigger() && !coolDown) // Stop Emote
                {
                    StopEmote();
                    coolDown = true;
                }
            }


            // Cooldown
            if (inputAxis == 0)
                imToLazy = false;
            if (!UnityInput.Current.GetKey(KeyCode.B) && !UnityInput.Current.GetKey(KeyCode.V) && !Controls.RightTrigger() && !Controls.LeftTrigger())
            {
                coolDown = false;
            }
        }
        #endregion

        #region Menu
        public void MenuDisplay()
        {
            if (Menu == null || menuText == null) return; // This shouldnt happen but just incase it gets destroyed

            string ToDraw = $"<color=magenta>EMOTES : Page {currentPage + 1}</color>\n"; // Doing +1 because it will show 0
            int i = 0;
            if (CurrentViewingMenu != null)
            {
                foreach (MenuOption opt in CurrentViewingMenu)
                {
                    if (SelectedOptionIndex == i)
                        ToDraw = ToDraw + "> ";
                    ToDraw = ToDraw + opt.Name;
                    ToDraw = ToDraw + "\n";
                    i++;
                }
                menuText.text = ToDraw;
            }
            else
            {
                Debug.Log("[EMOTE] CurrentViewingMenu Null");
            }
        }
        #endregion

        #region Cosmetics (Being disabled because im to lazy to update it)
        // Sourced from IIDK emote mod
        //private List<GameObject> portedCosmetics = new List<GameObject> { };
        //public void DisableCosmetics()
        //{
        //    VRRig.LocalRig.transform.Find("RigAnchor/rig/body/head/gorillaface").gameObject.layer = LayerMask.NameToLayer("Default");
        //    foreach (GameObject Cosmetic in VRRig.LocalRig.cosmetics)
        //    {
        //        if (Cosmetic.activeSelf && Cosmetic.transform.parent == VRRig.LocalRig.mainCamera.transform)
        //        {
        //            portedCosmetics.Add(Cosmetic);
        //            Cosmetic.transform.SetParent(VRRig.LocalRig.headMesh.transform, false);
        //            Cosmetic.transform.localPosition += new Vector3(0f, 0.1333f, 0.1f);
        //        }
        //    }
        //}

        //public void EnableCosmetics()
        //{
        //    VRRig.LocalRig.transform.Find("RigAnchor/rig/body/head/gorillaface").gameObject.layer = LayerMask.NameToLayer("MirrorOnly");
        //    foreach (GameObject Cosmetic in portedCosmetics)
        //    {
        //        Cosmetic.transform.SetParent(VRRig.LocalRig.mainCamera.transform, false);
        //        Cosmetic.transform.localPosition -= new Vector3(0f, 0.1333f, 0.1f);
        //    }
        //    portedCosmetics.Clear();
        //}
        #endregion

        #region Flying
        public void Fly()
        {
            Vector3 movement = Vector3.zero;

            // Handle WASD movement (keyboard controls)
            if (UnityInput.Current.GetKey(KeyCode.W)) movement += Camera.main.transform.forward;  // Forward
            if (UnityInput.Current.GetKey(KeyCode.S)) movement -= Camera.main.transform.forward;  // Backward
            if (UnityInput.Current.GetKey(KeyCode.A)) movement -= Camera.main.transform.right;  // Left
            if (UnityInput.Current.GetKey(KeyCode.D)) movement += Camera.main.transform.right;  // Right
            if (UnityInput.Current.GetKey(KeyCode.Space)) movement += Camera.main.transform.up;  // Up
            if (UnityInput.Current.GetKey(KeyCode.LeftControl)) movement -= Camera.main.transform.up;  // Down
            if (Mouse.current.rightButton.isPressed)
            {
                Vector3 eulerAngles = Camera.main.transform.rotation.eulerAngles;
                if (X < 0f) X = eulerAngles.y;

                eulerAngles = new Vector3(eulerAngles.x, X + (Mouse.current.position.ReadValue().x / (float)Screen.width - 5) * 360f * 1.33f, eulerAngles.z);
                Camera.main.transform.rotation = Quaternion.Euler(eulerAngles);
            }
            else
                X = -1f;

            if (XRSettings.isDeviceActive)
            {
                // Handle Left Joystick movement for X (right/left) and Z (forward/backward)
                float leftJoystickX = Controls.LeftJoystickAxis().x;  // Left joystick X-axis (right/left movement)
                float leftJoystickY = Controls.LeftJoystickAxis().y;  // Left joystick Y-axis (forward/backward movement)

                movement += Camera.main.transform.right * leftJoystickX;  // Move right/left relative to the camera
                movement += Camera.main.transform.forward * leftJoystickY;  // Move forward/backward relative to the camera

                // Handle Right Joystick movement for Y (up/down) movement
                float rightJoystickY = Controls.RightJoystickAxis().y;  // Right joystick Y-axis (up/down movement)
                movement += Camera.main.transform.up * rightJoystickY;  // Move up/down relative to the camera
            }

            // Apply the movement to the character's rigidbody
            GorillaTagger.Instance.rigidbody.transform.position += movement * Time.deltaTime * 5;
        }
        #endregion
    }
}
