﻿using UnityEngine;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour 
{
	public Chessman[,] Chessman { set; get; }
	private Chessman selectedChessman;

	private const float TILE_SIZE = 1.0f;
	private const float TILE_OFFSET = 0.5f;

	private int selectionX = -1; // nothing is selected
	private int selectionY = -1; // nothing is selected

	[SerializeField]
	private List<GameObject> chessmanPrefabs;
	private List<GameObject> activeChessman;

	private void Start()
	{
		SpawnAllChessmans();
	}

	private void Update()
	{
		UpdateSelection();
		DrawChessboard();
	}

	private void UpdateSelection()
	{
		if (!Camera.main) return;

		RaycastHit hit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessBoard")))
		{
			selectionX = (int)hit.point.x;
			selectionY = (int)hit.point.z;
		}
		else
		{
			selectionX = -1;
			selectionY = -1;
		}
	}

	private void SpawnChessman(int index, int x, int y)
	{
		GameObject go = Instantiate (chessmanPrefabs[index], GetTileCenter(x, y), Quaternion.identity) as GameObject;
		go.transform.SetParent (transform);
		Chessman[x, y] = go.GetComponent<Chessman>();
		activeChessman.Add(go);
	}

	private void SpawnAllChessmans()
	{
		activeChessman = new List<GameObject>();
		Chessman = new Chessman[8, 8];

		/// White team
		// King
		SpawnChessman(0, 4, 0);
		
		// Queen
		SpawnChessman(1, 3, 0);

		// Rooks
		SpawnChessman(2, 0, 0);
		SpawnChessman(2, 7, 0);
		
		// Bishops
		SpawnChessman(3, 2, 0);
		SpawnChessman(3, 5, 0);
		
		// Knights
		SpawnChessman(4, 1, 0);
		SpawnChessman(4, 6, 0);

		// Pawns
		for (int i = 0; i < 8; i++)
		{
			SpawnChessman(5, i, 1);
		}

		/// Black team
		// King
		SpawnChessman(6, 4, 7);
		
		// Queen
		SpawnChessman(7, 3, 7);

		// Rooks
		SpawnChessman(8, 0, 7);
		SpawnChessman(8, 7, 7);
		
		// Bishops
		SpawnChessman(9, 2, 7);
		SpawnChessman(9, 5, 7);
		
		// Knights
		SpawnChessman(10, 1, 7);
		SpawnChessman(10, 6, 7);

		// Pawns
		for (int i = 0; i < 8; i++)
		{
			SpawnChessman(11, i, 6);
		}
	}

	private Vector3 GetTileCenter (int x, int y)
	{
		Vector3 origin = Vector3.zero;
		
		origin.x += ((TILE_SIZE * x) + TILE_OFFSET);
		origin.z += ((TILE_SIZE * y) + TILE_OFFSET);

		return origin;
	}

	private void DrawChessboard()
	{
		Vector3 widthLine = Vector3.right * 8;
		Vector3 heigthLine = Vector3.forward * 8;

		for (int i = 0; i <= 8; i++)
		{
			Vector3 startPos = Vector3.forward * i;
			Debug.DrawLine(startPos, startPos + widthLine);
			for (int j = 0; j <= 8; j++)
			{
				startPos = Vector3.right * j;
				Debug.DrawLine(startPos, startPos + heigthLine);	
			}	
		}

		// Draw the hit cross
		if (selectionX >= 0 && selectionY >= 0)
		{
			Debug.DrawLine(
				Vector3.forward * selectionY + Vector3.right * selectionX, 
				Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1)
			);

			Debug.DrawLine(
				Vector3.forward * (selectionY + 1) + Vector3.right * selectionX, 
				Vector3.forward * selectionY + Vector3.right * (selectionX + 1)
			);
		}
	}
}
