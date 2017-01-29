using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class HighScores : MonoBehaviour {

	List<Highscore> highscores;
	string path;

	// Use this for initialization
	void Start () {
		highscores = new List<Highscore> ();
		path = Application.dataPath.Substring (0, Application.dataPath.Length - 7) + "/highscores.txt";
		LoadHighscores ();
	}

	void LoadHighscores () {
		if (File.Exists (path)) {
			try {
				StreamReader streamReader = new StreamReader (path);
				//check if streamReader is null
				using (streamReader) {
					string line;
					do {
						line = streamReader.ReadLine ();
						if (line != null) {
							string[] entry = line.Split (',');
							int score = int.Parse (entry [0]);
							string name = entry [1];
							Highscore addMe = new Highscore (score, name);
							highscores.Add (addMe);
						}
					} while (line != null);
					streamReader.Close();
				}
			} catch (Exception e) {
				Debug.LogError (e.Message);
			}
			highscores.Sort (SortHighscores);
		}
		WriteHighscores ();
	}

	void AddHighscore (int score, string name) {
		Highscore highscoreToAdd = new Highscore (score, name);
		highscores.Add (highscoreToAdd);
		highscores.Sort (SortHighscores);
		WriteHighscores ();
	}

	void WriteHighscores() {
		StreamWriter streamWriter = new StreamWriter (path, false);
		for (int i = 0; i < highscores.Count; i++) {
			string toWrite = highscores [i].score.ToString () + "," + highscores [i].name;
			streamWriter.WriteLine (toWrite);
		}
		streamWriter.Close ();
	}

	int SortHighscores(Highscore a, Highscore b) {
		if (b.score > a.score) {
			return 1;
		} else if (b.score == a.score) {
			return string.Compare (a.name, b.name);
		} else {
			return -1;
		}
	}

}

public struct Highscore {
	public int score;
	public string name;

	public Highscore(int score, string name) {
		this.score = score;
		this.name = name;
	}
}
