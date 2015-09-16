using UnityEngine;
using System.Collections;

public class Tile {

	public enum Type {
		OCEAN,
		LAND
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
}
