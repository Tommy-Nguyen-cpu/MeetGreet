# MeetGreet
A website developed to connect students (with .EDU emails) to events within their general vicinity.


# Prerequisites
1. In order to run MeetGreet, you must install ".NET SDK" from the following link: https://dotnet.microsoft.com/en-us/download/visual-studio-sdks. After words, follow the instructions once the sdk has completed downloading.
2. Alternatively, you can run the "dotnet-install" scripts.
  2.1. For Linux/MacOS users, the script can be found in "Linux & MacOS" folder.
  ![PathToMacScript](https://user-images.githubusercontent.com/75864631/228932990-61c6570a-dc63-41c2-8de2-d24392199139.PNG)
    
    2.1.2. "cd" into the folder and run the following command "./dotnet-install.sh --channel 7.0".
  2.2. For Windows, the script can be found in "Windows" folder.
  ![InstallingForWindows](https://user-images.githubusercontent.com/75864631/228933426-ca124230-5ce2-4180-b232-86e9a2685886.PNG)
    
    2.2.2. "cd" into the folder and run the following command "./dotnet-install.ps1 --channel 7.0".

# Run MeetGreet Through Console
Once you have installed the sdk, run the following commands in this order:
   1. "cd ../../"
   2. "dotnet build"
   4. "dotnet publish"
   5. "cd bin/Debug/netX.X" (where "netX.X" is a folder within Debug. You can navigate to this folder and figure out what "X.X" is).
   6. "dotnet MeetGreet.dll"

After the final command, a command line window should pop up displaying the following messages:
![ResultFromRunning](https://user-images.githubusercontent.com/75864631/228938385-768fc981-1500-4437-a990-1c1e98323aff.PNG)

Part of the messages contain the URL we will be using to get to the site (the URL circled in red above).
Open a browser and enter the URL, click enter.

# Alternative For Windows Users
In order to run MeetGreet in Windows, go to the following directory within the MeetGreet folder: "bin/Debug/.net7.0" and double click on "MeetGreet.dll".

# Run MeetGreet In Visual Studio 2022
A much simpler and guaranteed way of running MeetGreet is to open the project in "Visual Studio 2022" (Community version which is free). Within the MeetGreet folder, click on "MeetGreet.sIn", which should open the entire project in Visual Studio 2022.
