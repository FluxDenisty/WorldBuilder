using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class MapRenderer : MonoBehaviour {

    [SerializeField]
    Builder builder = null;

    Texture2D texture = null;

    Color[] colours = null;

    SpriteRenderer sprite = null;

    void Awake() {
        this.sprite = this.GetComponent<SpriteRenderer>();
    }

	void Start () {
        this.texture = new Texture2D(this.builder.Width, this.builder.Height);
        this.colours = new Color[this.builder.Width * this.builder.Height];

        this.sprite.sprite = Sprite.Create(this.texture, new Rect(0, 0, this.builder.Width, this.builder.Height), new Vector2(0.5f, 0.5f));

        float height = 10f / (this.texture.height / 100f);
        this.sprite.transform.localScale = new Vector3(height, height, 1);
//	}
//
//	void Update () {
        for (int x = 0; x < this.builder.Width; x++) {
            for (int y = 0; y < this.builder.Height; y++) {
				this.colours[x + y * this.builder.Width] = Color.white * Mathf.PerlinNoise(x / (float)this.builder.Width * 10f, y / (float)this.builder.Height * 10f);// this.builder.map[x, y].Colour;
            }
        }

        this.texture.SetPixels(this.colours);
        this.texture.Apply();
    }
}
