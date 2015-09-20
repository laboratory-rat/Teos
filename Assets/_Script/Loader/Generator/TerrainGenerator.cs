using UnityEngine;
using System.Collections;
using Common.Data;
using World.Navigation.Common;

namespace World.Generator.Terrain {

	public class TerrainGenerator{

		int size;
		int height;
		int moleCount;
		int molePower;
		System.Random rand;
		int fullCells;
		int maxFC;
		Dot target;
		float[,] hmap;

		public TerrainGenerator (ProTerrainData ptd) {
			size = ptd.Size;
			height = ptd.Height;
			moleCount = ptd.MoleCount;
			molePower = ptd.MolePower;
			rand = new System.Random (ptd.Seed);
			maxFC = (int) Mathf.Pow (ptd.Size, 2) * ptd.Fullness / 100;
		}

		public float[,] GenerateHeightmap () {
			fullCells = 0;
			hmap = new float[size, size];

			// Build heightMap
			for (int x = 0; x < size; x++) {
				for (int y = 0; y < size; y++) {
					hmap[x, y] = 0f;
				}
			}


			while (maxFC > fullCells) {
				if (fullCells == 0)
					target = new Dot(size / 2, size / 2);
				else {
					target = new Dot (rand.Next(0, size - 1), rand.Next (0, size - 1));
				}

				for (int i = 0; i < moleCount; i++) {
					if (fullCells >= maxFC)
						break;
					int power = (fullCells + molePower > maxFC) ? maxFC - fullCells : molePower;
					Mole (target, power);
				}
			}
			return hmap;
		}

		void Mole(Dot Pl, int Po) {
			Dot place = Pl;
			int power = Po;

			while (power > 0) {
				int r = rand.Next (1, 1000);
				int newX;
				int newY;

				if (r < 126) {
					newX = place.x - 1;
					newY = place.y - 1;
				}
				else if (r < 251) {
					newX = place.x - 1;
					newY = place.y;
				}
				else if (r < 376) {
					newX = place.x - 1;
					newY = place.y + 1;
				}
				else if (r < 501) {
					newX = place.x;
					newY = place.y + 1;
				}
				else if (r < 626) {
					newX = place.x + 1;
					newY = place.y + 1;
				}
				else if (r < 751) {
					newX = place.x;
					newY = place.y + 1;
				}
				else if (r < 876) {
					newX = place.x + 1;
					newY = place.y - 1;
				}
				else {
					newX = place.x;
					newY = place.y - 1;
				}

				if (newX < 0 || newY < 0 || newX > (size - 1) || newY > (size - 1) )
					continue;
				place = new Dot(newX, newY);
				if (hmap[newX, newY] == 0f) {
					hmap[newX, newY] = (float)height;
					power--;
					fullCells++;
				}
			}
		}


	}
}