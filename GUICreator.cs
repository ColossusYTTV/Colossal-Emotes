using UnityEngine;
using UnityEngine.UI;

namespace Colossal
{
    /*
     * I know even if I say dont skid this people are still going to skid it (dont skid this plz), Yes this is the same thing CCMV2 uses.
     * All I can ask is if you are going to skid this to please credit me.
     * - ColossusYTTV
    */
    internal class GUICreator : MonoBehaviour
    {
        private static Material mat = new Material(Shader.Find("GUI/Text Shader"));

        public static (GameObject, Text) CreateTextGUI(string text, string name, TextAnchor alignment, Vector3 loctrans)
        {
            GameObject HUDObj = new GameObject();
            HUDObj.name = name;

            Canvas canvas = HUDObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            HUDObj.AddComponent<CanvasScaler>();
            HUDObj.AddComponent<GraphicRaycaster>();

            RectTransform rectTransform = HUDObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(5, 5);
            HUDObj.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);

            GameObject menuTextObj = new GameObject();
            menuTextObj.transform.SetParent(HUDObj.transform);
            Text MenuText = menuTextObj.AddComponent<Text>();
            MenuText.text = text;
            MenuText.fontSize = 10;
            MenuText.font = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/UI/-- PhysicalComputer UI --/keyboard/Buttons/Text/u").GetComponent<Text>().font;
            MenuText.rectTransform.sizeDelta = new Vector2(260, 180);
            MenuText.rectTransform.localScale = new Vector3(0.01f, 0.01f, 1f);
            MenuText.rectTransform.localPosition = loctrans;
            MenuText.material = mat;
            MenuText.alignment = alignment;

            // Set the parent and adjust for camera position
            HUDObj.transform.SetParent(Camera.main.transform);
            HUDObj.transform.position = Camera.main.transform.position;
            HUDObj.transform.rotation = Camera.main.transform.rotation;

            return (HUDObj, MenuText);
        }
    }
}