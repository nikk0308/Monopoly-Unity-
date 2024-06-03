using System;
using UnityEngine;
using UnityEngine.UI;

public class CubeRollScript : MonoBehaviour {
    [SerializeField] private Image cubeImage;

    public void RollCube(int numResult) {
        string cubesFolder = "CubeImages/";
        string cubeNameSample = "dice_";
        Texture2D texture = Resources.Load<Texture2D>(cubesFolder + cubeNameSample + Convert.ToString(numResult));
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //rollAnimation.Play("CubeRolling", 0, 0.0f);
        cubeImage.sprite = sprite;
    }
}
