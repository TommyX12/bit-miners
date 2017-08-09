using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;

public class PasswordLoader : MyMono {
	public static PasswordLoader Current;

	public TextAsset source;
	public string[] passwords;

	void Awake(){
		Current = this;
	}

	public void Start(){
		passwords = source.text.Split ('\n');
	}

	public string GetPassword(){
		return Util.GetRandomElement (new List<string> (passwords));
	}
}
