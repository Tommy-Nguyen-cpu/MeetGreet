# MeetGreet
A website developed to connect students (with .EDU emails) to events within their general vicinity.


# Prerequisites
Download and extract the MeetGreet repository.
In order to run MeetGreet, you must install ".NET SDK". You can install ".NET SDK" via the following methods:

## .NET SDK Installation Instructions For Windows Users
1. Install the SDK from the following link: https://dotnet.microsoft.com/en-us/download/visual-studio-sdks. After words, follow the instructions once the sdk has completed downloading.

## .NET SDK Installation Instructions For MACOS/Linux Users
2. Follow the steps laid out in section "2.1" to install via command prompt.

  2.1. For Linux/MacOS users, the script to install .NET SDK can be in the "MeetGreet" folder, which is the extracted folder.
 
 ![PathToDotNetScript](https://user-images.githubusercontent.com/75864631/230197968-9189912b-b7b3-47e8-9b81-83fcaf87efe3.jpg)

    
    2.1.1.  Run the following commands:
    2.1.2. cd **{The name of the exported folder}**
    2.1.2. chmod +x ./dotnet-install.sh
    2.1.3. ./dotnet-install.sh
    2.1.4. cd ../../

# Run MeetGreet Through Console
Once you have installed the sdk, through the command line, navigate to the downloaded and extracted "MeetGreet" folder and run the following commands in this order: 
   1. dotnet build
   2. dotnet publish -o Release
   3. cd Release
   4. dotnet MeetGreet.dll
    
    **NOTE** : If after installing .NET sdk and attempting to run the "dotnet" commands, you receive an error saying that the command "dotnet" cannot be found, run the following command:
      1. export PATH="$PATH:$HOME/.dotnet
      Then try going through the steps outlined in "Run MeetGreet Through Console" again.
      
After running "dotnet MeetGreet.dll", a command line window should pop up displaying the following messages:
![ResultFromRunning](https://user-images.githubusercontent.com/75864631/228938385-768fc981-1500-4437-a990-1c1e98323aff.PNG)

Part of the messages contain the URL we will be using to get to the site (the URL circled in red above).
Open a browser and enter the URL, click enter.

# Run MeetGreet In Visual Studio 2022
A much simpler and guaranteed way of running MeetGreet is to open the project in "Visual Studio 2022" (Community version which is free). Within the MeetGreet folder, click on "MeetGreet.sIn", which should open the entire project in Visual Studio 2022.
