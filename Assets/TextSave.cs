using System.IO;
using UnityEngine;

namespace Assets
{
    public class TextSave : MonoBehaviour
    {
        
        public static void WriteString(string str)
        {
            string path = "SaveGame.txt";


            
            var sr = File.CreateText(path);
            sr.WriteLine(str);
            sr.Close();

            
            Debug.Log(str);
        }

        
        public static string ReadString()
        {
            const string path = "SaveGame.txt";

            //Read the text from directly from the test.txt file
            var reader = new StreamReader(path);
            var str = reader.ReadToEnd();
            reader.Close();
            
            Debug.Log(str);
            return str;
        }

    }
}