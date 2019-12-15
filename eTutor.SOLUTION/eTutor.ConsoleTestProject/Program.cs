using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace eTutor.ConsoleTestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string currentDirectory = Directory.GetCurrentDirectory();
            string path = Path.Combine(currentDirectory, "etutorapp-25808-firebase-adminsdk-exwmv-656a0fd3f9.json");
            Console.WriteLine(path);
            Console.WriteLine("\n");

            string json = File.ReadAllText(path, Encoding.UTF8);

            var firebase = FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(json)
            });
            
            Console.WriteLine(firebase.Name);

            FirebaseMessaging messaging = FirebaseMessaging.DefaultInstance;

            Message message = new Message
            {
                Notification = new Notification
                {
                    Title = "Simple Notification",
                    Body = "Part of the body"
                },
                Token =
                    "crhm711-RDw:APA91bFtsuksmqhOzzGe7efmkRJIG7kknhZb31XsUJZszB1mkd3suKggOiTU6UShSUMKD4PT_SXz4E7VMeZDsk_kZbj1uUx005chJlcIe1dXINroX_T7tJmjNPiKuEfyVGZwCnLw2jAE"
            };

            var result = messaging.SendAsync(message);

            Task.WaitAll();
            
            Console.WriteLine($"RESULT MESSAGE: {result.Result}");

        }
    }
}