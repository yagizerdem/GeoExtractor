using Microsoft.Data.Sqlite;
using System;
using System.Linq;
using System.Reflection.Emit;
using System.Text.Json;
using UUIDNext;


namespace GeoExtractor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Intilize();

            GUIManager.GetInstance.Render();

            char[] specialCharacters = { '.', ' ', '!', ',', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '{', '}', '[', ']', '|', '\\', ':', ';', '"', '\'', '<', '>', '?', '/', '`', '~' };

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                    if (GUIManager.GetInstance.ScreenState is Enums.ScreenState.StartUp)
                    {
                        if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
                        {
                            GUIManager.GetInstance.ScreenState = Enums.ScreenState.ExtractData;
                        }
                        else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
                        {
                            GUIManager.GetInstance.ScreenState = Enums.ScreenState.Guide;
                        }
                    }
                    else if(GUIManager.GetInstance.ScreenState is Enums.ScreenState.ExtractData)
                    {
                        if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
                        {
                            GUIManager.GetInstance.ScreenState = Enums.ScreenState.StartUp;
                        }
                        else if (char.IsLetterOrDigit(keyInfo.KeyChar) || specialCharacters.Contains(keyInfo.KeyChar))
                        {
                            GUIManager.GetInstance.FileNameInput += keyInfo.KeyChar;
                        }
                        else if (keyInfo.Key == ConsoleKey.Backspace)
                        {
                            GUIManager.GetInstance.FileNameInput = string.Join("", GUIManager.GetInstance.FileNameInput.ToList().
                                Take(Math.Max(0, GUIManager.GetInstance.FileNameInput.Length - 1)));
                        }
                        else if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            try
                            {
                                string fileName = GUIManager.GetInstance.FileNameInput.Trim();
                                string ext = Path.GetExtension(fileName);

                                if (ext != string.Empty && !ext.Equals(".rd3", StringComparison.OrdinalIgnoreCase))
                                {
                                    GUIManager.GetInstance.ScreenState = Enums.ScreenState.Error;
                                    GUIManager.GetInstance.ErrorMessage = "file extension is not correct , \n enter no file extension or rd3";
                                    continue;
                                }
                                if (ext == string.Empty)
                                {
                                    fileName += ".rd3";
                                }

                                // get full path of binary 
                                string GeoRadarJsonStorageFolderPath = System.Environment.GetEnvironmentVariable("GeoRadarJsonStorageFolderPath")!;
                                string GeoRadarImageStorageFolderPath = System.Environment.GetEnvironmentVariable("GeoRadarImageStorageFolderPath")!;
                                string GeoRadarBinaryStorageFolderPath = System.Environment.GetEnvironmentVariable("GeoRadarBinaryStorageFolderPath")!;


                                Guid sequentialUuid = Uuid.NewDatabaseFriendly(Database.SQLite);

                                // creating path variables
                                string Rd3FileFullpath = System.IO.Path.Join(GeoRadarBinaryStorageFolderPath, fileName);
                                string JsonOutputPath = System.IO.Path.Join(GeoRadarJsonStorageFolderPath, $"{sequentialUuid.ToString()}.json");
                                string ImageOutputPath = System.IO.Path.Join(GeoRadarImageStorageFolderPath, $"{sequentialUuid.ToString()}.png");

                                GUIManager.GetInstance.ScreenState = Enums.ScreenState.Loading;


                                RHandler rHandler = new RHandler();
                                string json_response = rHandler.RunRScript(Rd3FileFullpath, JsonOutputPath);


                                GPR_DataModel model = JsonSerializer.Deserialize<GPR_DataModel>(json_response);

                                // log to json file

                                File.WriteAllText(JsonOutputPath, json_response);

                                string base64String = model.img_base64.FirstOrDefault();

                                if (base64String != null)
                                {

                                    if (model.img_base64.FirstOrDefault().Contains(","))
                                        base64String = model.img_base64.FirstOrDefault().Split(',')[1];



                                    // Convert Base64 string to byte array
                                    byte[] imageBytes = Convert.FromBase64String(base64String);

                                    File.WriteAllBytes(ImageOutputPath, imageBytes);
                                }


                                if(json_response != null && json_response != string.Empty)
                                {
                                    using (var connection = new SqliteConnection($"Data Source={System.Environment.GetEnvironmentVariable("AppServiceDbPath")}"))
                                    {
                                        connection.Open();
                                        InsertJsonData(connection, json_response);
                                    }
                                }
                      


                                GUIManager.GetInstance.ScreenState = Enums.ScreenState.StartUp;
                            }
                            catch (Exception ex)
                            {
                                GUIManager.GetInstance.ScreenState = Enums.ScreenState.Error;
                            }
                        }

                    }
                    else if(GUIManager.GetInstance.ScreenState is Enums.ScreenState.Error)
                    {
                        if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
                        {
                            GUIManager.GetInstance.ErrorMessage = string.Empty;
                            GUIManager.GetInstance.ScreenState = Enums.ScreenState.StartUp;
                        }
                    }
                    else if(GUIManager.GetInstance.ScreenState is Enums.ScreenState.Guide)
                    {
                        if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
                        {
                            GUIManager.GetInstance.ScreenState = Enums.ScreenState.StartUp;
                        }
                    }
                }

            }

        }

             
        private static  void Intilize()
        {
            string documentsFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string GeoRadarServiceFolderPath = Path.Join(documentsFolderPath, "GeoRadarService");

            if (!Directory.Exists(GeoRadarServiceFolderPath))
            {
                Directory.CreateDirectory(GeoRadarServiceFolderPath);
            }

            string AppServiceDbPath = Path.Join(GeoRadarServiceFolderPath, "AppServices.db");

            if (!File.Exists(AppServiceDbPath))
            {
                File.Create(AppServiceDbPath);
            }

            // store output json data from r script 

            string GeoRadarJsonStorageFolderPath = Path.Join(GeoRadarServiceFolderPath, "GeoRadarJsonStorage");

            if (!Directory.Exists(GeoRadarJsonStorageFolderPath))
            {
                Directory.CreateDirectory(GeoRadarJsonStorageFolderPath);
            }

            string GeoRadarBinaryStorageFolderPath = Path.Join(GeoRadarServiceFolderPath, "GeoRadarBinaryStorage");

            if (!Directory.Exists(GeoRadarBinaryStorageFolderPath))
            {
                Directory.CreateDirectory(GeoRadarBinaryStorageFolderPath);
            }


            string GeoRadarImageStorageFolderPath = Path.Join(GeoRadarServiceFolderPath, "GeoRadarImageStorage");


            if (!Directory.Exists(GeoRadarImageStorageFolderPath))
            {
                Directory.CreateDirectory(GeoRadarImageStorageFolderPath);
            }

            System.Environment.SetEnvironmentVariable("AppServiceDbPath", AppServiceDbPath);
            System.Environment.SetEnvironmentVariable("GeoRadarJsonStorageFolderPath", GeoRadarJsonStorageFolderPath);
            System.Environment.SetEnvironmentVariable("GeoRadarBinaryStorageFolderPath", GeoRadarBinaryStorageFolderPath);
            System.Environment.SetEnvironmentVariable("GeoRadarImageStorageFolderPath", GeoRadarImageStorageFolderPath);

            // crate table if not exist 

            using (var connection = new SqliteConnection($"Data Source={System.Environment.GetEnvironmentVariable("AppServiceDbPath")}"))
            {
                connection.Open();
                CreateJsonTable(connection);
            }

        }

        private static void CreateJsonTable(SqliteConnection connection)
        {
            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText = @"
            CREATE TABLE IF NOT EXISTS JsonDataTable (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                JsonString TEXT
            );";
            createTableCommand.ExecuteNonQuery();
        }
        static void InsertJsonData(SqliteConnection connection, string jsonString)
        {
            var insertCommand = connection.CreateCommand();
            insertCommand.CommandText = @"
            INSERT INTO JsonDataTable (JsonString)
            VALUES (@jsonString);";
            insertCommand.Parameters.AddWithValue("@jsonString", jsonString);
            insertCommand.ExecuteNonQuery();
            Console.WriteLine("Inserted JSON data: " + jsonString);
        }

    }
}
