using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Builder : MonoBehaviour {

	[SerializeField]
	private int width = 400;
	public int Width {
		get { return this.width; }
	}

	[SerializeField]
	private int height = 300;
	public int Height {
		get { return this.height; }
	}

	public Tile[,] map;

	void Awake () {
		this.map = new Tile[this.width, this.height];
		for (int x = 0; x < this.width; x++) {
			for (int y = 0; y < this.height; y++) {
				this.map[x, y] = new Tile();
			}
		}

		this.StartCoroutine(this.BuildCoroutine());
	}
	
	private IEnumerator BuildCoroutine() {
        this.GenerateMainland();
        for (int x = 0; x < this.width; x++) {
            for (int y = 0; y < this.height; y++) {
                // this.map[x, y].type = (Random.Range(0, 2) == 0 ? Tile.Type.LAND : Tile.Type.OCEAN);
            }
            yield return null;
        }
        yield return null;
	}

    private void GenerateMainland() {
        const int sections = 80;
        const int landSections = 35;
        int secHeight = Mathf.RoundToInt(Mathf.Sqrt(sections / (this.width / (float)this.height)));
        int secWidth = Mathf.RoundToInt(Mathf.Sqrt(sections / (this.height / (float)this.width)));
        Debug.Log(secWidth * secHeight); // << THIS IS VERY WRONG
        bool[,] isLand = new bool[secWidth, secHeight];
        
        // 80 = w * h
        // a = w / h
        // w = a * h
        // 80 = (a * h) * h
        // 80 = a * h^2
        // sqrt(80 / a) = h

        int count = 0;
        while (count < landSections) {
            int x = Random.Range(1, secWidth - 1);
            int y = Random.Range(1, secHeight - 1);
            if (!isLand[x, y]) {
                count++;
                isLand[x, y] = true;
            }
        }

        float sectionSize = (float)this.height / (Mathf.Sqrt(sections / (this.width / (float)this.height)));

        for (int x = 0; x < secWidth; x++) {
            for (int y = 0; y < secHeight; y++) {
                if (isLand[x, y]) {
                    this.SetTypeCircle(
                        Mathf.RoundToInt(x * sectionSize + sectionSize / 2),
                        Mathf.RoundToInt(y * sectionSize + sectionSize / 2),
                        Tile.Type.LAND, sectionSize * 0.4f);
                }
            }
        }
    }

    private void SetTypeCircle(int centerX, int centerY, Tile.Type type, float radius) {
        for (int x = Mathf.FloorToInt(centerX - radius); x < Mathf.CeilToInt(centerX + radius); x++) {
            for (int y = Mathf.FloorToInt(centerY - radius); y < Mathf.CeilToInt(centerY + radius); y++) {
                if (Mathf.Sqrt(Mathf.Pow(x-centerX, 2) + Mathf.Pow(y-centerY, 2)) <= radius) {
                    this.map[x, y].type = type;
                }
            }
        }
    }
}
