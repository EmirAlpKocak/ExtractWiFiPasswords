# ExtractWiFiPasswords
C# program for extracting all saved WiFi passwords on a Windows system.

ExtractWiFiPasswords is a simple C# program that extracts all saved WiFi passwords on a Windows system.
It uses built in netsh.exe on Windows to do that.

With this program, you can quickly get your WiFi password if you do not remember it.

⚠️ This tool is made for educational purposes only. Use this only in your systems or systems that you have the rights for running this kind of software. For more information, please read the Security file.

The logic is simple, it runs netsh wlan show profiles at the background and extracts all WiFi profiles using RegEx. Then it also searchs for saved passwords using netsh wlan show profile name="profilename" key=clear and displays them out to the CMD window.

This program has no GUI, you need to run it from Command Prompt. Directly opening it won't work.

Every line of code are explained in the project file.

!! IMPORTANT INFO !!

As explained in the code, this will only work on English systems. To make it work on another language, you need to change the "All User Profile" and "Key Content" in the code to your language. For example in German you will use: Profil für alle Benutzer and Schlüsselinhalt. You may also need to change chcp 437 in the code for special characters.

In some versions of Windows, this program may require an elevated CMD window to work. In my case, it didn't reqire on Windows 11 but it did require on Windows 7.

If you don't get any output from the program, it can mean that it can't find any profiles or netsh failed at background

If you get "Not found" as password, it means that you may need to run the program as administrator or the WiFi has no password.

Requires .NET Framework 4.0.
