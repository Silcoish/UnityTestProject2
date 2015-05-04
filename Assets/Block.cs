using UnityEngine;
using System.Collections;

public class Block {

	public enum Direction
	{
		NORTH,
		EAST,
		SOUTH,
		WEST,
		UP,
		DOWN
	}

	public Block()
	{

	}

	public virtual MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.DOWN))
			meshData = FaceDataUp(chunk, x, y, z, meshData);

		if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.UP))
			meshData = FaceDataDown(chunk, x, y, z, meshData);

		if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.SOUTH))
			meshData = FaceDataNorth(chunk, x, y, z, meshData);

		if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.NORTH))
			meshData = FaceDataSouth(chunk, x, y, z, meshData);

		if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.WEST))
			meshData = FaceDataEast(chunk, x, y, z, meshData);

		if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.EAST))
			meshData = FaceDataWest(chunk, x, y, z, meshData);

		return meshData;
	}

	protected virtual MeshData FaceDataUp (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		meshData.vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));

		meshData.AddQuadTriangles();

		return meshData;
	}

	protected virtual MeshData FaceDataDown (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		meshData.vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

		meshData.AddQuadTriangles();

		return meshData;
	}

	protected virtual MeshData FaceDataNorth (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		meshData.vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));

		meshData.AddQuadTriangles();

		return meshData;
	}

	protected virtual MeshData FaceDataSouth (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		meshData.vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));

		meshData.AddQuadTriangles();

		return meshData;
	}

	protected virtual MeshData FaceDataEast (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		meshData.vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f));

		meshData.AddQuadTriangles();

		return meshData;
	}

	protected virtual MeshData FaceDataWest (Chunk chunk, int x, int y, int z, MeshData meshData)
	{
		meshData.vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f));
		meshData.vertices.Add(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f));

		meshData.AddQuadTriangles();

		return meshData;
	}

	public virtual bool IsSolid(Direction direction)
	{
		switch(direction)
		{
			case Direction.NORTH:
				return true;
			case Direction.EAST:
				return true;
			case Direction.SOUTH:
				return true;
			case Direction.WEST:
				return true;
			case Direction.UP:
				return true;
			case Direction.DOWN:
				return true;
		}

		return false;
	}

}
