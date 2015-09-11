﻿using UnityEngine;
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

	void Start () {
		this.map = new Tile[this.width, this.height];
		for (int x = 0; x < this.width; x++) {
			for (int y = 0; y < this.height; y++) {
				this.map[x, y] = new Tile();
			}
		}

		this.StartCoroutine(this.BuildCoroutine());
	}
	
	private IEnumerator BuildCoroutine() {
		yield return null;
	}
}