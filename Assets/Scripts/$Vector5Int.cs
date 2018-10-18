using System;
using UnityEngine;
using System.Collections;

[Serializable]
public struct Vector5Int
{
	public int unused;
	public int largeRow;
	public int largeColumn;
	public int wonBy;
	public int valid;

	public int magnitude
	{
		get { return (int)Vector5Int.Magnitude(this); }
	}
	public int sqrMagnitude
	{
		get { return (int)Vector5Int.SqrMagnitude(this); }
	}
	public Vector5Int(int V, int W, int X, int Y, int Z)
	{
		this.unused = V;
		this.largeRow = W;
		this.largeColumn = X;
		this.wonBy = Y;
		this.valid = Z;
	}
	public Vector5Int(int W, int X, int Y, int Z)
	{
		this.unused = 0;
		this.largeRow = W;
		this.largeColumn = X;
		this.wonBy = Y;
		this.valid = Z;
	}
	public Vector5Int(int X, int Y, int Z)
	{
		this.unused = 0;
		this.largeRow = 0;
		this.largeColumn = X;
		this.wonBy = Y;
		this.valid = Z;
	}
	public Vector5Int(int X, int Y)
	{
		this.unused = 0;
		this.largeRow = 0;
		this.largeColumn = X;
		this.wonBy = Y;
		this.valid = 0;
	}
	public static Vector5Int One = new Vector5Int(1, 1, 1, 1, 1);
	public static Vector5Int Zero = new Vector5Int(0, 0, 0, 0, 0);
	public static int Distance(Vector5Int a, Vector5Int b)
	{
		return (int)Math.Sqrt(((b.unused - a.unused) * (b.unused - a.unused)) +
			((b.largeRow - a.largeRow) * (b.largeRow - a.largeRow)) +
			((b.largeColumn - a.largeColumn) * (b.largeColumn - a.largeColumn)) +
			((b.wonBy - a.wonBy) * (b.wonBy - a.wonBy)) +
			((b.valid - a.valid) * (b.valid - a.valid)));
	}
	public static int Magnitude(Vector5Int a)
	{
		return (int)Mathf.Sqrt(
			Mathf.Pow(a.unused, 2) +
			Mathf.Pow(a.largeRow, 2) +
			Mathf.Pow(a.largeColumn, 2) +
			Mathf.Pow(a.wonBy, 2) +
			Mathf.Pow(a.valid, 2));
	}
	public static int Dot(Vector5Int a, Vector5Int b)
	{
		return ((a.unused * b.unused) + (a.largeRow * b.largeRow) + (a.largeColumn * b.largeColumn) + (a.wonBy * b.wonBy) + (a.valid * b.valid));
	}
	public static int Angle(Vector5Int a, Vector5Int b)
	{
		Vector5Int A = a;
		Vector5Int B = b;
		A.unused = A.unused % 360;
		A.largeRow = A.largeRow % 360;
		A.largeColumn = A.largeColumn % 360;
		A.wonBy = A.wonBy % 360;
		A.valid = A.valid % 360;
		B.unused = B.unused % 360;
		B.largeRow = B.largeRow % 360;
		B.largeColumn = B.largeColumn % 360;
		B.wonBy = B.wonBy % 360;
		B.valid = B.valid % 360;
		if (A.unused > 180)
			A.unused = -(360 - A.unused);
		if (A.largeRow > 180)
			A.largeRow = -(360 - A.largeRow);
		if (A.largeColumn > 180)
			A.largeColumn = -(360 - A.largeColumn);
		if (A.wonBy > 180)
			A.wonBy = -(360 - A.wonBy);
		if (A.valid > 180)
			A.valid = -(360 - A.valid);
		if (B.unused > 180)
			B.unused = -(360 - B.unused);
		if (B.largeRow > 180)
			B.largeRow = -(360 - B.largeRow);
		if (B.largeColumn > 180)
			B.largeColumn = -(360 - B.largeColumn);
		if (B.wonBy > 180)
			B.wonBy = -(360 - B.wonBy);
		if (B.valid > 180)
			B.valid = -(360 - B.valid);
		return (int)Mathf.Sqrt(((B.unused - A.unused) * (B.unused - A.unused)) +
			((B.largeRow - A.largeRow) * (B.largeRow - A.largeRow)) +
			((B.largeColumn - A.largeColumn) * (B.largeColumn - A.largeColumn)) +
			((B.wonBy - A.wonBy) * (B.wonBy - A.wonBy)) +
			((B.valid - A.valid) * (B.valid - A.valid)));
	}
	public static int SqrMagnitude(Vector5Int a)
	{
		return (int)Mathf.Pow(Vector5Int.Magnitude(a), 2);
	}
	public static Vector5Int Normalize(Vector5Int a)
	{
		int Mag = Vector5Int.Magnitude(a);
		return new Vector5Int(
			a.unused / Mag,
			a.largeRow / Mag,
			a.largeColumn / Mag,
			a.wonBy / Mag,
			a.valid / Mag);
	}
	public override bool Equals(object obj)
	{
		if (obj is Vector5Int)
		{
			Vector5Int A = (Vector5Int)obj;
			if (A.unused != this.unused)
				return false;
			else if (A.largeRow != this.largeRow)
				return false;
			else if (A.largeColumn != this.largeColumn)
				return false;
			else if (A.wonBy != this.wonBy)
				return false;
			else if (A.valid != this.valid)
				return false;
			else return true;
		}
		else throw new InvalidCastException("The object 'obj' is not castable to the type 'Vector5'");
	}
	public override string ToString()
	{
		return "(" + this.unused.ToString("F1") +
			", " + this.largeRow.ToString("F1") +
				", " + this.largeColumn.ToString("F1") +
				", " + this.wonBy.ToString("F1") +
				", " + this.valid.ToString("F1") + ")";
	}
	public string ToString(string Format)
	{
		return "(" + this.unused.ToString(Format) +
			", " + this.largeRow.ToString(Format) +
				", " + this.largeColumn.ToString(Format) +
				", " + this.wonBy.ToString(Format) +
				", " + this.valid.ToString(Format) + ")";
	}
	public override int GetHashCode()
	{

		BitArray A = new BitArray(BitConverter.GetBytes(this.unused));
		A = A.Xor(new BitArray(BitConverter.GetBytes(this.largeRow)));
		A = A.Xor(new BitArray(BitConverter.GetBytes(this.largeColumn)));
		A = A.Xor(new BitArray(BitConverter.GetBytes(this.wonBy)));
		A = A.Xor(new BitArray(BitConverter.GetBytes(this.valid)));
		byte[] B = new byte[4];
		A.CopyTo(B, 0);
		return BitConverter.ToInt32(B, 0);
	}
}
