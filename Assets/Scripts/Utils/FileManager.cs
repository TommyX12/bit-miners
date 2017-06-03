using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class FileManager
{
	public static void Save<T>(string path, T data)
	{
		path = Application.persistentDataPath + "/" + path;
		
		string directory = Path.GetDirectoryName(path);
		Directory.CreateDirectory(directory);
		
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(path);
		bf.Serialize(file, data);
		file.Close();
		
		Debug.Log("Saved file to: " + path);
	}
	
	public static T Load<T>(string path)
	{
		path = Application.persistentDataPath + "/" + path;
		
		if (File.Exists(path)){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(path, FileMode.Open);
			T data = (T)bf.Deserialize(file);
			file.Close();
			
			Debug.Log("Loaded file from: " + path);
			
			return data;
		}
		
		Debug.LogWarning("Load failed: Could not find file: " + path);
		
		return default(T);
	}
	
	public static string[] GetFolders(string path)
	{
		if (path.Length == 0) path = Application.persistentDataPath;
		else path = Application.persistentDataPath + "/" + path;
		
		string[] directories = Directory.GetDirectories(path);
		
		if (directories.Length == 0) return directories;
		
		int rootLength = Path.GetDirectoryName(directories[0]).Length + 1;
		
		for (int i = 0; i < directories.Length; ++i) {
			directories[i] = directories[i].Substring(rootLength);
		}
		
		return directories;
	}
	
	public static void DeleteFolder(string path)
	{
		path = Application.persistentDataPath + "/" + path;
		
		Directory.Delete(path, true);
	}
}
