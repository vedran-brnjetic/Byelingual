using System.IO;
using UnityEditor;
using UnityEngine;

namespace Assets
{
    public class TextSave : MonoBehaviour
    {
        
        public static void WriteString(string str)
        {
            string path = "Assets/Resources/CabinInTheWoods/SaveGames/SaveGame.txt";

            //Write some text to the test.txt file
            StreamWriter writer = new StreamWriter(path, false);
            writer.WriteLine(str);
            writer.Close();

            //Re-import the file to update the reference in the editor
            AssetDatabase.ImportAsset(path);
            TextAsset asset = UnityEngine.Resources.Load("CabinInTheWoods/SaveGames/SaveGame") as TextAsset;

            //Print the text from the file
            Debug.Log(asset.text);
        }

        
        public static string ReadString()
        {
            string path = "Assets/Resources/CabinInTheWoods/SaveGames/SaveGame.txt";

            //Read the text from directly from the test.txt file
            StreamReader reader = new StreamReader(path);
            var str = reader.ReadToEnd();
            reader.Close();
            return str;
        }

    }
}