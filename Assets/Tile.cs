using UnityEngine;
using System.Collections;

public class Tile {

	public enum Type {
		OCEAN,
		LAND
	}

    public int X {
        private set;
        get;
    }

    public int Y {
        private set;
        get;
    }

    public Type type = Type.OCEAN;

    public Color Colour {
        get { 
            switch (this.type)
            {
                case Type.LAND:
                    return Color.green;
                case Type.OCEAN:
                    return Color.blue;
                default:
                    return Color.black;
            }
        }
    }

    public Tile(int x, int y) {
        this.X = x;
        this.Y = y;
    }
}
