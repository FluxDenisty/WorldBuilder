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
				this.map[x, y] = new Tile(x, y);
			}
		}

		this.StartCoroutine(this.BuildCoroutine());
	}
	
	private IEnumerator BuildCoroutine() {
        yield return this.StartCoroutine(this.GeneratePerlinLand());
        for (int x = 0; x < this.width; x++) {
            for (int y = 0; y < this.height; y++) {
                // this.map[x, y].type = (Random.Range(0, 1) == 0 ? Tile.Type.LAND : Tile.Type.OCEAN);
            }
            yield return null;
        }
        yield return null;

		Debug.Log("DONE!");
	}

	private IEnumerator GeneratePerlinLand() {
		float seed = Random.Range(0f, 1f);

		int yieldCounter = 0;

		for (int x = Mathf.RoundToInt(this.width * 0.05f); x < Mathf.RoundToInt(this.width * 0.95f); x++) {
			for (int y = Mathf.RoundToInt(this.height * 0.05f); y < Mathf.RoundToInt(this.height * 0.95f); y++) {
//				Debug.Log(Mathf.PerlinNoise(x / (float)this.width + seed, y / (float)this.height + seed));
				if (Mathf.PerlinNoise(x / (float)this.width + seed, y / (float)this.height + seed) > 0.5f) {
					this.map[x, y].type = Tile.Type.LAND;
				}
			}
			if (yieldCounter++ >= 500) {
                yieldCounter = 0;
                yield return null;
            }
		}
	}

    private IEnumerator GenerateMainland() {
        const int SECTIONS = 500;
        const int LAND_SECTIONS = 200;
        int secHeight = Mathf.RoundToInt(Mathf.Sqrt(SECTIONS / (this.width / (float)this.height)));
        int secWidth = Mathf.RoundToInt(Mathf.Sqrt(SECTIONS / (this.height / (float)this.width)));
        bool[,] isLand = new bool[secWidth, secHeight];

        int yieldCounter = 0;

        // 80 = w * h
        // a = w / h
        // w = a * h
        // 80 = (a * h) * h
        // 80 = a * h^2
        // sqrt(80 / a) = h

        int count = 0;
        while (count < LAND_SECTIONS) {
            int x = Random.Range(1, secWidth - 1);
            int y = Random.Range(1, secHeight - 1);
            if (!isLand[x, y]) {
                count++;
                isLand[x, y] = true;
            }
        }

        float sectionSize = (float)this.height / (Mathf.Sqrt(SECTIONS / (this.width / (float)this.height)));

		// Place dots on grid.
//        int circleSize = 0;
//        for (int x = 0; x < secWidth; x++) {
//            for (int y = 0; y < secHeight; y++) {
//                if (isLand[x, y]) {
//                    circleSize = this.SetTypeCircle(
//                        Mathf.RoundToInt(x * sectionSize + sectionSize / 2),
//                        Mathf.RoundToInt(y * sectionSize + sectionSize / 2),
//                        Tile.Type.LAND, sectionSize * 0.2f);
//
//                    if (yieldCounter++ >= 5) {
//                        yieldCounter = 0;
//                        yield return null;
//                    }
//                }
//            }
//        }

		// Place dots randomly.
		const float PERCENT_LAND = 0.4f;
		const float PERCENT_DOTS = 0.5f;
		int leftToFill = Mathf.RoundToInt(this.width * this.height * PERCENT_LAND);
		while (leftToFill > Mathf.RoundToInt(this.width * this.height * PERCENT_LAND * (1f - PERCENT_DOTS))) {
			leftToFill -= this.SetTypeCircle(
				Mathf.RoundToInt(Random.Range(this.width * 0.05f, this.width * 0.95f)),
				Mathf.RoundToInt(Random.Range(this.height * 0.05f, this.height * 0.95f)),
				Tile.Type.LAND, Random.Range(5, 10));
				
            if (yieldCounter++ >= 5) {
                yieldCounter = 0;
                yield return null;
            }
		}

		// Complile a list of possible land placements
        List<Tile> possible = new List<Tile>();
        for (int x = 0; x < this.width; x++) {
            for (int y = 0; y < this.height; y++) {
                Tile tile = this.map[x, y];
                if (tile.type == Tile.Type.OCEAN && this.Adjacent(tile).Exists(t => t.type == Tile.Type.LAND)) {
                    possible.Add(tile);
                }

                if (yieldCounter++ >= (this.width * this.height) / 100) {
                    yieldCounter = 0;
                    yield return null;
                }
            }
        }

		// Randomly place the remaining land tiles.
//        int totalToFill = (Mathf.RoundToInt(sectionSize * sectionSize) - circleSize) * LAND_SECTIONS * 3;
//        int leftToFill = totalToFill;
        while (leftToFill > 0) {
            int i = Random.Range(0, possible.Count - 1);
            Tile tile = possible[i];
            tile.type = Tile.Type.LAND;
            possible.RemoveAt(i);
            possible.AddRange(this.Adjacent(tile).FindAll(t => t.type == Tile.Type.OCEAN));
            leftToFill--;

            if (yieldCounter++ >= this.width * this.height * PERCENT_LAND / 200f) {
                yieldCounter = 0;
                yield return null;
            }
        }
    }

    private int SetTypeCircle(int centerX, int centerY, Tile.Type type, float radius) {
        int count = 0;
        for (int x = Mathf.Max(0, Mathf.FloorToInt(centerX - radius)); x < Mathf.CeilToInt(centerX + radius) && x < this.width; x++) {
            for (int y = Mathf.Max(0, Mathf.FloorToInt(centerY - radius)); y < Mathf.CeilToInt(centerY + radius) && y < this.height; y++) {
                if (Mathf.Sqrt(Mathf.Pow(x-centerX, 2) + Mathf.Pow(y-centerY, 2)) <= radius) {
					if (this.map[x, y].type != type) {
	                    this.map[x, y].type = type;
	                    count++;
					}
                }
            }
        }

        return count;
    }

    private List<Tile> Adjacent(Tile tile) {
        List<Tile> ret = new List<Tile>();
        if (tile.X > 0) {
            ret.Add(this.map[tile.X - 1, tile.Y]);
        }
        if (tile.Y > 0) {
            ret.Add(this.map[tile.X, tile.Y - 1]);
        }
        if (tile.X < this.width - 1) {
            ret.Add(this.map[tile.X + 1, tile.Y]);
        }
        if (tile.Y < this.height - 1) {
            ret.Add(this.map[tile.X, tile.Y + 1]);
        }

        return ret;
    }

    
}

static class MyExtensions {
    public static void Shuffle<T>(this IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = Random.Range(0, n +1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
