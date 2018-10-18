using System;
using UnityEngine;
using System.Collections;

[Serializable]
public struct Vector6Int
{
	public int largeRow;
	public int largeColumn;
	public int smallRow;
	public int smallColumn;
	public int wonBy;
	public int valid;

	public int magnitude
	{
		get { return Vector6Int.Magnitude(this); }
	}
	public int sqrMagnitude
	{
		get { return Vector6Int.SqrMagnitude(this); }
	}
	public Vector6Int(int U, int V, int W, int X, int Y, int Z)
	{
		this.largeRow = U;
		this.largeColumn = V;
		this.smallRow = W;
		this.smallColumn = X;
		this.wonBy = Y;
		this.valid = Z;
	}
	public Vector6Int(int V, int W, int X, int Y, int Z)
	{
		this.largeRow = 0;
		this.largeColumn = V;
		this.smallRow = W;
		this.smallColumn = X;
		this.wonBy = Y;
		this.valid = Z;
	}
	public Vector6Int(int W, int X, int Y, int Z)
	{
		this.largeRow = W;
		this.largeColumn = X;
		this.smallRow = Y;
		this.smallColumn = Z;
		this.wonBy = 0;
		this.valid = 0;
	}
	public Vector6Int(int X, int Y, int Z)
	{
		this.largeRow = 0;
		this.largeColumn = 0;
		this.smallRow = X;
		this.smallColumn = Y;
		this.wonBy = Z;
		this.valid = 0;
	}
	public Vector6Int(int X, int Y)
	{
		this.largeRow = 0;
		this.largeColumn = 0;
		this.smallRow = X;
		this.smallColumn = Y;
		this.wonBy = 0;
		this.valid = 0;
	}
	public static Vector6Int One = new Vector6Int(1, 1, 1, 1, 1, 1);
	public static Vector6Int Zero = new Vector6Int(0, 0, 0, 0, 0, 0);
	public static int Distance(Vector6Int a, Vector6Int b)
	{
		return (int)Math.Sqrt(((b.largeRow - a.largeRow) * (b.largeRow - a.largeRow)) +
			((b.largeColumn - a.largeColumn) * (b.largeColumn - a.largeColumn)) +
			((b.smallRow - a.smallRow) * (b.smallRow - a.smallRow)) +
			((b.smallColumn - a.smallColumn) * (b.smallColumn - a.smallColumn)) +
			((b.wonBy - a.wonBy) * (b.wonBy - a.wonBy)));
	}
	public static int Magnitude(Vector6Int a)
	{
		return (int)Mathf.Sqrt(
			Mathf.Pow(a.largeRow, 2) +
			Mathf.Pow(a.largeColumn, 2) +
			Mathf.Pow(a.smallRow, 2) +
			Mathf.Pow(a.smallColumn, 2) +
			Mathf.Pow(a.wonBy, 2) +
			Mathf.Pow(a.valid, 2));
	}
	public static int Dot(Vector6Int a, Vector6Int b)
	{
		return ((a.largeRow * b.largeRow) + (a.largeColumn * b.largeColumn) + (a.smallRow * b.smallRow) + (a.smallColumn * b.smallColumn) + (a.wonBy * b.wonBy) + (a.valid * b.valid));
	}
	public static int Angle(Vector6Int a, Vector6Int b)
	{
		Vector6Int A = a;
		Vector6Int B = b;
		A.largeRow = A.largeRow % 360;
		A.largeColumn = A.largeColumn % 360;
		A.smallRow = A.smallRow % 360;
		A.smallColumn = A.smallColumn % 360;
		A.wonBy = A.wonBy % 360;
		A.valid = A.valid % 360;
		B.largeRow = B.largeRow % 360;
		B.largeColumn = B.largeColumn % 360;
		B.smallRow = B.smallRow % 360;
		B.smallColumn = B.smallColumn % 360;
		B.wonBy = B.wonBy % 360;
		B.valid = B.valid % 360;
		if (A.largeRow > 180)
			A.largeRow = -(360 - A.largeRow);
		if (A.largeColumn > 180)
			A.largeColumn = -(360 - A.largeColumn);
		if (A.smallRow > 180)
			A.smallRow = -(360 - A.smallRow);
		if (A.smallColumn > 180)
			A.smallColumn = -(360 - A.smallColumn);
		if (A.wonBy > 180)
			A.wonBy = -(360 - A.wonBy);
		if (A.valid > 180)
			A.valid = -(360 - A.valid);
		if (B.largeRow > 180)
			B.largeRow = -(360 - B.largeRow);
		if (B.largeColumn > 180)
			B.largeColumn = -(360 - B.largeColumn);
		if (B.smallRow > 180)
			B.smallRow = -(360 - B.smallRow);
		if (B.smallColumn > 180)
			B.smallColumn = -(360 - B.smallColumn);
		if (B.wonBy > 180)
			B.wonBy = -(360 - B.wonBy);
		if (B.valid > 180)
			B.valid = -(360 - B.valid);
		return (int)Mathf.Sqrt(((B.largeRow - A.largeRow) * (B.largeRow - A.largeRow)) +
			((B.largeColumn - A.largeColumn) * (B.largeColumn - A.largeColumn)) +
			((B.smallRow - A.smallRow) * (B.smallRow - A.smallRow)) +
			((B.smallColumn - A.smallColumn) * (B.smallColumn - A.smallColumn)) +
			((B.wonBy - A.wonBy) * (B.wonBy - A.wonBy) * (A.valid - B.valid)));
	}
	public static int SqrMagnitude(Vector6Int a)
	{
		return (int)Mathf.Pow(Vector6Int.Magnitude(a), 2);
	}
	public static Vector6Int Normalize(Vector6Int a)
	{
		int Mag = Vector6Int.Magnitude(a);
		return new Vector6Int(
			a.largeRow / Mag,
			a.largeColumn / Mag,
			a.smallRow / Mag,
			a.smallColumn / Mag,
			a.wonBy / Mag,
			a.valid / Mag);
	}
	public override bool Equals(object obj)
	{
		if (obj is Vector6Int)
		{
			Vector6Int A = (Vector6Int)obj;
			if (A.largeRow != this.largeRow)
				return false;
			else if (A.largeColumn != this.largeColumn)
				return false;
			else if (A.smallRow != this.smallRow)
				return false;
			else if (A.smallColumn != this.smallColumn)
				return false;
			else if (A.wonBy != this.wonBy)
				return false;
			else if (A.valid != this.valid)
				return false;
			else return true;
		}
		else throw new InvalidCastException("The object 'obj' is not castable to the type 'Vector6'");
	}
	public override string ToString()
	{
		return "(" + this.largeRow.ToString("F1") +
			", " + this.largeColumn.ToString("F1") +
				", " + this.smallRow.ToString("F1") +
				", " + this.smallColumn.ToString("F1") +
				", " + this.wonBy.ToString("F1") +
				", " + this.valid.ToString("F1") + ")";
	}
	public string ToString(string Format)
	{
		return "(" + this.largeRow.ToString(Format) +
			", " + this.largeColumn.ToString(Format) +
				", " + this.smallRow.ToString(Format) +
				", " + this.smallColumn.ToString(Format) +
				", " + this.wonBy.ToString(Format) +
				", " + this.valid.ToString(Format) + ")";
	}
	public override int GetHashCode()
	{

		BitArray A = new BitArray(BitConverter.GetBytes(this.largeRow));
		A = A.Xor(new BitArray(BitConverter.GetBytes(this.largeColumn)));
		A = A.Xor(new BitArray(BitConverter.GetBytes(this.smallRow)));
		A = A.Xor(new BitArray(BitConverter.GetBytes(this.smallColumn)));
		A = A.Xor(new BitArray(BitConverter.GetBytes(this.wonBy)));
		A = A.Xor(new BitArray(BitConverter.GetBytes(this.valid)));
		byte[] B = new byte[4];
		A.CopyTo(B, 0);
		return BitConverter.ToInt32(B, 0);
	}
}
