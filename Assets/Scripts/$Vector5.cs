using System;
using UnityEngine;
using System.Collections;

[Serializable]
public struct Vector5
{
	public float largeRow;
	public float largeColumn;
	public float smallRow;
	public float smallColumn;
	public float wonBy;
	public float magnitude
	{
		get { return Vector5.Magnitude(this); }
	}
	public float sqrMagnitude
	{
		get { return Vector5.SqrMagnitude(this); }
	}
	public Vector5(float V, float W, float X, float Y, float Z)
	{
		this.largeRow = V;
		this.largeColumn = W;
		this.smallRow = X;
		this.smallColumn = Y;
		this.wonBy = Z;
	}
	public Vector5(float W, float X, float Y, float Z)
	{
		this.largeRow = 0;
		this.largeColumn = W;
		this.smallRow = X;
		this.smallColumn = Y;
		this.wonBy = Z;
	}
	public Vector5(float X, float Y, float Z)
	{
		this.largeRow = 0;
		this.largeColumn = 0;
		this.smallRow = X;
		this.smallColumn = Y;
		this.wonBy = Z;
	}
	public Vector5(float X, float Y)
	{
		this.largeRow = 0;
		this.largeColumn = 0;
		this.smallRow = X;
		this.smallColumn = Y;
		this.wonBy = 0;
	}
	public static Vector5 One = new Vector5(1, 1, 1, 1, 1);
	public static Vector5 Zero = new Vector5(0, 0, 0, 0, 0);
	public static float Distance(Vector5 a, Vector5 b)
	{
		return (float)Math.Sqrt(((b.largeRow - a.largeRow) * (b.largeRow - a.largeRow)) +
			((b.largeColumn - a.largeColumn) * (b.largeColumn - a.largeColumn)) +
			((b.smallRow - a.smallRow) * (b.smallRow - a.smallRow)) +
			((b.smallColumn - a.smallColumn) * (b.smallColumn - a.smallColumn)) +
			((b.wonBy - a.wonBy) * (b.wonBy - a.wonBy)));
	}
	public static float Magnitude(Vector5 a)
	{
		return Mathf.Sqrt(
			Mathf.Pow(a.largeRow, 2) +
			Mathf.Pow(a.largeColumn, 2) +
			Mathf.Pow(a.smallRow, 2) +
			Mathf.Pow(a.smallColumn, 2) +
			Mathf.Pow(a.wonBy, 2));
	}
	public static float Dot(Vector5 a, Vector5 b)
	{
		return ((a.largeRow * b.largeRow) + (a.largeColumn * b.largeColumn) + (a.smallRow * b.smallRow) + (a.smallColumn * b.smallColumn) + (a.wonBy * b.wonBy));
	}
	public static float Angle(Vector5 a, Vector5 b)
	{
		Vector5 A = a;
		Vector5 B = b;
		A.largeRow = A.largeRow % 360;
		A.largeColumn = A.largeColumn % 360;
		A.smallRow = A.smallRow % 360;
		A.smallColumn = A.smallColumn % 360;
		A.wonBy = A.wonBy % 360;
		B.largeRow = B.largeRow % 360;
		B.largeColumn = B.largeColumn % 360;
		B.smallRow = B.smallRow % 360;
		B.smallColumn = B.smallColumn % 360;
		B.wonBy = B.wonBy % 360;
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
		return Mathf.Sqrt(((B.largeRow - A.largeRow) * (B.largeRow - A.largeRow)) +
			((B.largeColumn - A.largeColumn) * (B.largeColumn - A.largeColumn)) +
			((B.smallRow - A.smallRow) * (B.smallRow - A.smallRow)) +
			((B.smallColumn - A.smallColumn) * (B.smallColumn - A.smallColumn)) +
			((B.wonBy - A.wonBy) * (B.wonBy - A.wonBy)));
	}
	public static float SqrMagnitude(Vector5 a)
	{
		return Mathf.Pow(Vector5.Magnitude(a), 2);
	}
	public static Vector5 Normalize(Vector5 a)
	{
		float Mag = Vector5.Magnitude(a);
		return new Vector5(
			a.largeRow / Mag,
			a.largeColumn / Mag,
			a.smallRow / Mag,
			a.smallColumn / Mag,
			a.wonBy / Mag);
	}
	public static Vector5 Lerp(Vector5 from, Vector5 to, float t)
	{
		return new Vector5(
			Mathf.Lerp(from.largeRow, to.largeRow, t),
			Mathf.Lerp(from.largeColumn, to.largeColumn, t),
			Mathf.Lerp(from.smallRow, to.smallRow, t),
			Mathf.Lerp(from.smallColumn, to.smallColumn, t),
			Mathf.Lerp(from.wonBy, to.wonBy, t));
	}
	public static Vector5 Slerp(Vector5 from, Vector5 to, float t)
	{
		Vector5 A = from;
		Vector5 B = to;
		A.largeRow = A.largeRow % 360;
		A.largeColumn = A.largeColumn % 360;
		A.smallRow = A.smallRow % 360;
		A.smallColumn = A.smallColumn % 360;
		A.wonBy = A.wonBy % 360;
		B.largeRow = B.largeRow % 360;
		B.largeColumn = B.largeColumn % 360;
		B.smallRow = B.smallRow % 360;
		B.smallColumn = B.smallColumn % 360;
		B.wonBy = B.wonBy % 360;
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
		return Vector5.Lerp(A, B, t);
	}
	public override bool Equals(object obj)
	{
		if (obj is Vector5)
		{
			Vector5 A = (Vector5)obj;
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
			else return true;
		}
		else throw new InvalidCastException("The object 'obj' is not castable to the type 'Vector5'");
	}
	public override string ToString()
	{
		return "(" + this.largeRow.ToString("F1") +
			", " + this.largeColumn.ToString("F1") +
				", " + this.smallRow.ToString("F1") +
				", " + this.smallColumn.ToString("F1") +
				", " + this.wonBy.ToString("F1") + ")";
	}
	public string ToString(string Format)
	{
		return "(" + this.largeRow.ToString(Format) +
			", " + this.largeColumn.ToString(Format) +
				", " + this.smallRow.ToString(Format) +
				", " + this.smallColumn.ToString(Format) +
				", " + this.wonBy.ToString(Format) + ")";
	}
	public override int GetHashCode()
	{

		BitArray A = new BitArray(BitConverter.GetBytes(this.largeRow));
		A = A.Xor(new BitArray(BitConverter.GetBytes(this.largeColumn)));
		A = A.Xor(new BitArray(BitConverter.GetBytes(this.smallRow)));
		A = A.Xor(new BitArray(BitConverter.GetBytes(this.smallColumn)));
		A = A.Xor(new BitArray(BitConverter.GetBytes(this.wonBy)));
		byte[] B = new byte[4];
		A.CopyTo(B, 0);
		return BitConverter.ToInt32(B, 0);
	}
}
