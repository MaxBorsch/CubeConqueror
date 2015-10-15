using UnityEngine;
using System.Collections;
using System.IO;


//===============================================================================================
// Language
// KLL 09/04/2009 - Max B. EDIT 12/01/14
//
// Reads text files in the Assets\Resources\Languages directory into a Hashtable. The text file
// consists of one line that has the key name while the next line has the actual
// text to display.  This is used to localize a game.
//
//  Example:
//      Assume we have a text file called English.txt in Assets\Resources\Languages
//      The file looks like this:
//
//      HELLO
//      Hello and welcome!
//      GOODBYE
//      Goodbye and thanks for playing!
//
//      in code we file load the language file:
//          Language.LoadLanguage("English");
//
//      then we can retrieve text by calling
//          Language.GetText("HELLO");
//
//      will return a string containing "Hello and welcome!"
//
//===============================================================================================

public class Language {

    public static Hashtable textTable;
	public static string[] languages;
	public static string current;

	private static FileInfo[] FindLanguages() {
		DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Languages/");
		FileInfo[] info = dir.GetFiles("*.txt");
		return info;
	}

	public static void RefreshLanguages() {
		FileInfo[] langs = FindLanguages();
		languages = new string[langs.Length];
		for (int i=0; i<langs.Length; i++) {
			languages[i] = langs[i].Name.Substring(0, langs[i].Name.Length - 4);
		}
	}

    public static bool LoadLanguage(string filename)
    {
        string fullpath = "Languages/" +  filename;

        TextAsset textAsset = (TextAsset) Resources.Load(fullpath, typeof(TextAsset));
        if (textAsset == null)
		{
			Debug.Log(fullpath + " file not found.");
            return false;
		}

        // create the text hash table if one doesn't exist
        if (textTable == null) {
            textTable = new Hashtable();
		}

        // clear the hashtable
		current = filename;
        textTable.Clear();

        StringReader reader = new StringReader(textAsset.text);
        string key;
        string val;
        while(true)
            {
            key = reader.ReadLine();
            val = reader.ReadLine();
            if (key != null && val != null)
                {
					textTable.Add(key, val);
                }
            else
                {
                break;
                }
            }


        reader.Close();

        return true;
    }


    public static string GetText(string key)
    {
		if (textTable[key] == null) return "?";
        return  (string)textTable[key];
    }

}
