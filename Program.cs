// Licensed under CC BY-NC-SA 4.0 (https://creativecommons.org/licenses/by-nc-sa/4.0/)
// AUTHOR: EmirAlpKocak
// GitHub: https://github.com/EmirAlpKocak
// Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International (CC BY-NC-SA 4.0)
// ExtractWiFiPasswords project C#

// DISCLAIMER [!!!]
// This code is made for educational purposes only.
// You must use this code or program on your OWN systems or a system that you HAVE THE RIGHTS to run this kind of software.
// Use the following code or program at on your OWN RISK.
// The author is not responsible for any damage or problems that following code can cause.
// You have been warned.
// If you do not agree with these terms, do not use this code or program.
// Purpose of codes are explained with comments.

using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
// import required libs.

namespace ExtractWiFiPasswords
{
    class Program
    {
        static void Main(string[] args)
        {
            string result = RunCmd("chcp 437 & netsh wlan show profiles"); // we use netsh to extract WLAN profiles.
            // chcp 437 here is used for handling some characters that special to non English language.
            // you may change this value according to your system language as 437 may not be enough for some characters
            string pattern = @"(?:All User Profile\s*:\s*)(.*)";
            // we create the pattern for Regex, it looks a bit complicated but we are looking for All User Profiles,
            // and discarding all white spaces.
            // CRITICAL NOTE HERE: This will only work on English systems as we look for
            // "All User Profile" , for example in a German system this code will not work. you need to change it
            // to Profil für alle Benutzer
            MatchCollection matches = Regex.Matches(result, pattern); // lets find the profile name
            foreach (Match match in matches) // loop through the profiles
            {
                string profileName = match.Groups[1].Value.Trim(); // extract profile name we found
                string keyPattern = @"(?:Key Content\s*:\s*)(.*)"; // lets look for Key Content:    passwordhere on command output
                // CRITICAL NOTE HERE: This will only work on English systems as we look for
                // "Key Content" , for example in a German system this code will not work. you need to change it
                // to Schlüsselinhalt
                string result2 = RunCmd($"chcp 437 & netsh wlan show profile \"{profileName}\" key=clear");
                // this is the command we use on netsh to get the saved password
                // note that this command may require administrator on some Windows versions
                // in my tests it required admin on Windows 7 but not on Windows 11.
                Match keyMatch = Regex.Match(result2, keyPattern); // we use Match this time because there is only one password
                // for each profile, we don't need to loop.
                string wifiPass = "Not found"; // the output will be Not found if we can't find a password.
                if (keyMatch.Success) // if we found one
                {
                    wifiPass = keyMatch.Groups[1].Value.Trim(); // extract it!
                }
                Console.WriteLine($"WiFi Name: {profileName} WiFi Password: {wifiPass}");
                // output wifi name and password to the screen.
            }
        }
        static string RunCmd(string arg)
        {
            // I have created a function so we don't have to create process all the time.
            // we can simply use RunCmd(program name and arguments)
            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = $"/c {arg}"; // enter command to the cmd /c
            info.FileName = "cmd"; // run cmd
            info.RedirectStandardOutput = true;
            // we use this to redirect output from the cmd command to our string.
            info.UseShellExecute = false; // this is required for redirect
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            // those are used to hide the cmd window from the yser
            using (Process process = Process.Start(info)) // start the process
            {
                using (StreamReader reader = process.StandardOutput) // Now we read the output.
                {
                    string result = reader.ReadToEnd(); // Extract the result from command to string.
                    return result; // send the string back to where it was called.
                }
            }
        }
    }
}
