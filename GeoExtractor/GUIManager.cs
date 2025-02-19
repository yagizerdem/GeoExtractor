using GeoExtractor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoExtractor
{


    internal class GUIManager
    {

        private static GUIManager Instance;

        private ScreenState _screenState;

        public ScreenState ScreenState
        {
            get => _screenState;
            set
            {
               _screenState = value;
               OnNavigate();
            }

        }

        private string _fileNameInput;

        public string FileNameInput
        {
            get => _fileNameInput;
            set
            {
                _fileNameInput = value;
                Render();
            }
        }

        private string _errorMessage = string.Empty;

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                Render();
            }
        } 

        private GUIManager()
        {
            if (Instance == null) Instance = this;

            ScreenState = ScreenState.StartUp;
        }
        public static GUIManager GetInstance =>  Instance == null ? new GUIManager() : Instance;   
    

        private string StartUpScreen()
        {
            string template = """
                #########################
                #                       #
                # 1-)  Extract Data     #
                #                       #
                # 2-)  How To Use Guide #
                #                       #
                #########################
                
                press 1 to extract data
                press 2 to list installation steps and usage guide
                """.Trim();

            return template;
                 
        }

        private string ExtractDataScreen()
        {
            string template = $"""
                Enter Rd3 file name :  {FileNameInput}
                
                1-) Go Back
                Enter-)  Begin process file
                """.Trim();

            return template;

        }


        private string LoadingScreen()
        {
            string template = """
                Loading ...
                """.Trim();

            return template;

        }

        private string GuideScreen()
        {
            string template = """
                1. Run app via visual studio
                    this ll create Folder named 'GeoRadarService'
                    in documents special folder in users computer (windows)

                2. After running app first time , navigate to GeoRadarBinaryStorage
                   Folder which is nested in GeoRadarService folder. example path ->
                    c://Users/username/GeoRadarService/GeoRadarBinaryStorage

                3. Paste all your rd3 , rad , cor GeoRadarBinaryStorage.
                    rd3 , rad and cor files shold remain in same folder! (GeoRadarBinaryStorage)
                
                4. Press 1 from numpad to navigate to ExtractData screen

                5. Enter file name of rd3 file which is reside on GeoRadarBinaryStorage

                6. After Entering file name press Enter key to start process , this may take some time.

                7. After Process finished , new .json and .png file with unique id created respectively
                   in GeoRadarJsonStorage and GeoRadarImageStorage Folders which are nested in 
                   GeoRadarService folder


                """.Trim();

            return template;

        }

        private string ErrorScreen()
        {
            string template = $"""
                Error Occured : {ErrorMessage}

                1-) Go Back
                """.Trim();

            return template;

        }

        public void Render()
        {
            Console.Clear();
            if(_screenState is ScreenState.StartUp)
            {
                Console.WriteLine(StartUpScreen());
            }
            else if (_screenState is ScreenState.ExtractData)
            {
                Console.WriteLine(ExtractDataScreen());
            }
            else if(_screenState is ScreenState.Loading)
            {
                Console.WriteLine(LoadingScreen());
            }
            else if(_screenState is ScreenState.Guide)
            {
                Console.WriteLine(GuideScreen());
            }
            else if (_screenState is ScreenState.Error)
            {
                Console.WriteLine(ErrorScreen());
            }
        }


        public void OnNavigate()
        {
            Render();
        }


    }
}
