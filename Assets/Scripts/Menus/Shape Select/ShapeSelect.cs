using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeSelect : MonoBehaviour
{
    public struct ShapeSpriteInfo
    {
        public string name;
        public Sprite sprite;

        public ShapeSpriteInfo(string _name)
        {
            name = _name;
            sprite = Resources.Load<Sprite>($"Sprites/Shapes/{name}");
            if (sprite == null)
            {
                sprite = Resources.Load<Sprite>($"Sprites/placeholder");
            }
        }
    }

    protected float startTime;
    protected static GameObject tick1, tick2;

    protected ShapeSpriteInfo[] shapes;

    List<ShapeSpriteInfo> shapesList = new List<ShapeSpriteInfo>();

    //shape names are now stored in the Shapes (Scripts/Data/Shapes.cs) class (Shapes.names)!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //ok

    // Start is called before the first frame update
    protected void _Start()
    {
        tick1 = GameObject.Find("Tick 1");
        tick2 = GameObject.Find("Tick 2");
        tick1.SetActive(false);
        tick2.SetActive(false);

        GameObject.Find("P1 Score").GetComponent<Text>().text = GameInfo.p1Score.ToString();
        GameObject.Find("P2 Score").GetComponent<Text>().text = GameInfo.p2Score.ToString();
        startTime = Time.time;
    }

    protected void AddShapeSpriteInfo(ShapeSpriteInfo shape)
    {
        shapesList.Add(shape);
    }

    protected void ShapesToArray()
    {
        shapes = shapesList.ToArray();
        print(shapesList.Count);
    }
}
