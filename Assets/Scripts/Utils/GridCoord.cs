using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GridCoord {
	public static readonly GridCoord[] DIRECTIONS = {
		new GridCoord( 1, -1),
		new GridCoord( 0, -1),
		new GridCoord(-1,  0),
		new GridCoord(-1,  1),
		new GridCoord( 0,  1),
		new GridCoord( 1,  0)
	};
	
	public int x;
	public int y;
	
	public GridCoord(int x = 0, int y = 0) {
		this.x = x;
		this.y = y;
	}
	
	/// <summary>
	/// Add 2 CubicHexCoords together and return the result.
	/// </summary>
	/// <param name="lhs">The GridCoord on the left-hand side of the + sign.</param>
	/// <param name="rhs">The GridCoord on the right-hand side of the + sign.</param>
	/// <returns>A new GridCoord representing the sum of the inputs.</returns>
	public static GridCoord operator +( GridCoord lhs, GridCoord rhs ) 
	{
		int x = lhs.x + rhs.x;
		int y = lhs.y + rhs.y;

		return new GridCoord(x, y);
	}

	
	/// <summary>
	/// Subtract 1 GridCoord from another and return the result.
	/// </summary>
	/// <param name="lhs">The GridCoord on the left-hand side of the - sign.</param>
	/// <param name="rhs">The GridCoord on the right-hand side of the - sign.</param>
	/// <returns>A new GridCoord representing the difference of the inputs.</returns>
	public static GridCoord operator -( GridCoord lhs, GridCoord rhs ) 
	{
		int x = lhs.x - rhs.x;
		int y = lhs.y - rhs.y;

		return new GridCoord(x, y);
	}

	
	/// <summary>
	/// Check if 2 CubicHexCoords represent the same hex on the grid.
	/// </summary>
	/// <param name="lhs">The GridCoord on the left-hand side of the == sign.</param>
	/// <param name="rhs">The GridCoord on the right-hand side of the == sign.</param>
	/// <returns>A bool representing whether or not the CubicHexCoords are equal.</returns>
	public static bool operator ==( GridCoord lhs, GridCoord rhs ) 
	{
		return ( lhs.x == rhs.x ) && ( lhs.y == rhs.y );
	}

	
	/// <summary>
	/// Check if 2 CubicHexCoords represent the different hexes on the grid.
	/// </summary>
	/// <param name="lhs">The GridCoord on the left-hand side of the != sign.</param>
	/// <param name="rhs">The GridCoord on the right-hand side of the != sign.</param>
	/// <returns>A bool representing whether or not the CubicHexCoords are unequal.</returns>
	public static bool operator !=( GridCoord lhs, GridCoord rhs ) 
	{
		return ( lhs.x != rhs.x ) || ( lhs.y != rhs.y );
	}


	/// <summary>
	/// Get a hash reflecting the contents of the GridCoord.
	/// </summary>
	/// <returns>An integer hash code reflecting the contents of the GridCoord.</returns>
	public override int GetHashCode()
	{
		// See http://stackoverflow.com/questions/7813687/right-way-to-implement-gethashcode-for-this-struct
		unchecked
		{
			int hash = 17;
			hash = hash * 23 + x.GetHashCode();
			hash = hash * 23 + y.GetHashCode();
			return hash;
		}
	}

	
	/// <summary>
	/// Check if this GridCoord is equal to an arbitrary object.
	/// </summary>
	/// <returns>Whether or not this GridCoord and the given object are equal.</returns>
	public override bool Equals( object obj )
	{
		if ( obj == null )
		{
			return false;
		}            

		if ( GetType() != obj.GetType() )
		{
			return false;
		}  
		
		GridCoord other = (GridCoord)obj;

		return this == other;
	}
}
