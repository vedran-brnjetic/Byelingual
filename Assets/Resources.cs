using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Assets.StoryTemplate.Infrastructure;
using SimpleJSON;
using UnityEngine;


namespace Assets
{
    public static class Resources
    {
        private const string GameUri = "http://api.byelingual.me:8000/";

        public static string GetStringResponse(string apiSource)
        {
            // Create a request for the URL.   
            var request = WebRequest.Create(new Uri(GameUri + apiSource +"/"));
            // If required by the server, set the credentials.  
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.  
            var response = request.GetResponse();
            // Display the status.  
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.  
            var dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.  
            var reader = new StreamReader(dataStream);
            // Read the content.  
            var responseFromServer = reader.ReadToEnd();
            // Clean up the streams and the response.  
            reader.Close();
            response.Close();

            
            //Return the value
            //Debug.Log(responseFromServer);
            return responseFromServer;
        }

        public static async Task<byte[]> GetBitmapFromURL(string apiSource)
        {
            // Create a request for the URL.   
            var request = WebRequest.Create(new Uri(GameUri + "media/" + apiSource));
            
            // If required by the server, set the credentials.  
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.  
            var response = await request.GetResponseAsync();
            // Display the status.  
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.  
            var dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.  
            var bitmap = new byte[(int)dataStream.Length+1000];
            var numBytesToRead = (int)dataStream.Length;
            var numBytesRead = 0;
            do
            {
                // Read may return anything from 0 to 10.
                var n = dataStream.Read(bitmap, numBytesRead,1000);
                numBytesRead += n;
                numBytesToRead -= n;
                
            } while (numBytesToRead > 0);
            // Clean up the streams and the response.  

            response.Close();


            //Return the value
            //Debug.Log(responseFromServer);
            return bitmap;
        }

        public static Dictionary<string, Story> GetStoriesFromInternet()
        {
            string jsonString;
            var stories = new Dictionary<string, Story>();
            try
            {
                jsonString = GetStringResponse("stories");
            }
            catch (WebException e)
            {
                Debug.Log(e.Message);
                jsonString = "{}";
            }

            var jsonStories = JSON.Parse(jsonString);

            var storyCount = jsonStories["stories"].Count;
            if (storyCount > 0)
                for (var i = 0; i < storyCount; i++)
                {
                    var story = new Story(
                        jsonStories["stories"][i]["name"],
                        jsonStories["stories"][i]["description"],
                        jsonStories["stories"][i]["image"]
                    ).WithImage(
                        An.Image().Named(
                            jsonStories["stories"][i]["name"]
                            )
                        );
                    stories[story.SnakeCase()]=story;
                }

            return stories;
        }


    }
}