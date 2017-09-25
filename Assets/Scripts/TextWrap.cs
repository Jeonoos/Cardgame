using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWrap : MonoBehaviour {

	public TextMesh textObject;
	public int maxLineChars = 10;

	string[] words;
	public string result = "";
	int charCount;

	void Start () {
		updateText();   //call this function to format your string.
	}

	public void updateText () {
		FormatString(textObject.text);
	}

	void FormatString (string text ) {

		int currentLine = 1;
		int charCount = 0;
		string[] words = text.Split(" "[0]); //Split the string into seperate words
		string result = "";

		for (var index = 0; index < words.Length; index++) {

			var word = words[index].Trim();

			if (index == 0) {
				result = words[0];
				textObject.text = result;
			}

			if (index > 0 ) {
				charCount += word.Length + 1; //+1, because we assume, that there will be a space after every word
				if (word == "#") {
					charCount = 0;
					result += "\n ";
				} else {
					if (charCount <= maxLineChars) {
						result += " " + word;
					} else {
						charCount = 0;
						result += "\n " + word;
					}
				}

				textObject.text = result;
			}
		}
	}

}
