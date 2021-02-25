using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;
using Crestron.SimplSharp.CrestronIO;

namespace ConfigFileLibrary
{
    public class ConfigFile
    {
        private string hostname;
        private string configFileData;
        private string[] classroomVariable = new string[100];

        public ConfigFile()
        {
            CrestronConsole.SendControlSystemCommand("hostname", ref hostname);

            // Removes the unwanted data from the response and leaves us with only the hostname of the processor
            hostname = hostname.Remove(0, 11);
            hostname.Remove(0, 11);
            hostname = hostname.Remove(hostname.IndexOf(" \r\n"), 3);
        }

        public void ReadConfig()
        {
            // Will hold the start and end index of the room
            int startIndex;
            int endIndex;    

            try
            {
                StreamReader streamReader = new StreamReader("\\NVRAM\\aisle-config.csv", Encoding.UTF8);
                configFileData = streamReader.ReadToEnd();
            }
            catch (IOException e)
            {
                CrestronConsole.PrintLine("There was an error: {0}", e.ToString());
            }
            catch (FileNotFoundException e)
            {
                CrestronConsole.PrintLine("There was an error: {0}", e.ToString());
            }
            catch (DirectoryNotFoundException e)
            {
                CrestronConsole.PrintLine("There was an error: {0}", e.ToString());
            }

            // Finds the processors config information using its hostname.
            startIndex = configFileData.IndexOf(hostname);
            endIndex = configFileData.IndexOf("%", startIndex);

            // Will be used to separate the string into an array of classroom variables.
            classroomVariable = configFileData.Substring(startIndex, endIndex - (startIndex + 1)).Split(',');
        }

        public void GetVariables()
        {
            foreach (string result in classroomVariable)
            {
                CrestronConsole.PrintLine(result);
            }
            CrestronConsole.PrintLine(classroomVariable.Length.ToString());
        }
    }
}