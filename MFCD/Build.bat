@echo off


::--------------------------------Linux--------------------------------::


::Publish to Linux (.NET Core Bundled)::
dotnet publish -c Release -f netcoreapp3.1 -p:PublishSingleFile=true -p:PublishTrimmed=true -r linux-x64 MFCD.csproj 1>nul

::Rename so it can be moved.::
cd "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\bin\Release\netcoreapp3.1\linux-x64\publish"
del MFCD.pdb
ren MFCD MFCD.bin


::Move Linux build to Desktop::
move "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\bin\Release\netcoreapp3.1\linux-x64\publish\MFCD.bin" "C:\Users\Cinnamon\Desktop\MFCD\Linux\MFCD.bin" 1>nul

::Rename appropriately.::
cd "C:\Users\Cinnamon\Desktop\MFCD\Linux\"
del MFCD-full
ren MFCD.bin MFCD-full
cd "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\"




::Publish to Linux (Requires .NET Core)::
dotnet publish -c Release -f netcoreapp3.1 --no-self-contained -p:PublishSingleFile=true -r linux-x64 MFCD.csproj 1>nul

::Rename so it can be moved::
cd "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\bin\Release\netcoreapp3.1\linux-x64\Publish\"
del MFCD.pdb
ren MFCD MFCD.bin


::Move Linux build to Desktop::
move "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\bin\Release\netcoreapp3.1\linux-x64\publish\MFCD.bin" "C:\Users\Cinnamon\Desktop\MFCD\Linux\MFCD.bin" 1>nul

::Rename Appropriately.::
cd "C:\Users\Cinnamon\Desktop\MFCD\Linux\"
del MFCD-slim
ren MFCD.bin MFCD-slim
cd "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\"


::--------------------------------MacOS--------------------------------::


::Publish to MacOS::
dotnet publish -c Release -f netcoreapp3.1 -p:PublishSingleFile=true -p:PublishTrimmed=true -r osx.10.14-x64 MFCD.csproj 1>nul

::Rename so it can be moved::
cd "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\bin\Release\netcoreapp3.1\osx.10.14-x64\Publish\"
del MFCD.pdb
ren MFCD MFCD.bin


::Move MacOS to Desktop::
move "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\bin\Release\netcoreapp3.1\osx.10.14-x64\Publish\MFCD.bin" "C:\Users\Cinnamon\Desktop\MFCD\MacOS\MFCD.bin" 1>nul

::Rename Appropriately.::
cd "C:\Users\Cinnamon\Desktop\MFCD\MacOS\" 
del MFCD-MacOS-full
ren MFCD.bin MFCD-MacOS-full
cd "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\"


::Publish to MacOS (Requires .NET Core)::
dotnet publish -c Release -f netcoreapp3.1 -p:PublishSingleFile=true --no-self-contained -r osx.10.14-x64 MFCD.csproj 1>nul

::Rename so it can be moved::
cd "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\bin\Release\netcoreapp3.1\osx.10.14-x64\Publish\"
del MFCD.pdb
ren MFCD MFCD.bin

::Move MacOS build to Desktop::
move "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\bin\Release\netcoreapp3.1\osx.10.14-x64\Publish\MFCD.bin" "C:\Users\Cinnamon\Desktop\MFCD\MacOS\MFCD.bin" 1>nul

::Rename Appropriately.::
cd "C:\Users\Cinnamon\Desktop\MFCD\MacOS"
del MFCD-MacOS-slim
ren MFCD.bin MFCD-MacOS-slim
cd "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\"

::--------------------------------Win10--------------------------------::


::Publish to Win10::
dotnet publish -c Release -f netcoreapp3.1 -p:PublishSingleFile=true -p:PublishTrimmed=true -r win-x64 MFCD.csproj 1>nul

::Move Win10 to Desktop::
move "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\bin\Release\netcoreapp3.1\win-x64\publish\MFCD.exe" "C:\Users\Cinnamon\Desktop\MFCD\Win10\MFCD.exe" 1>nul

::Rename Appropriately.::
cd "C:\Users\Cinnamon\Desktop\MFCD\Win10\"
del MFCD-full.exe
ren MFCD.exe MFCD-full.exe
cd "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\"



::Publish to Win10 (Requires .NET Core)::
dotnet publish -c Release -f netcoreapp3.1 -p:PublishSingleFile=true --no-self-contained -r win-x64 MFCD.csproj 1>nul


::Move Win10 build to Desktop::
move "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\bin\Release\netcoreapp3.1\win-x64\publish\MFCD.exe" "C:\Users\Cinnamon\Desktop\MFCD\Win10\MFCD.exe" 1>nul

::Rename Appropriately.::
cd "C:\Users\Cinnamon\Desktop\MFCD\Win10\"
del MFCD.exe
ren MFCD.exe MFCD-slim.exe
cd "C:\Users\Cinnamon\Documents\GitHub\Furry-Image-Downloader\MFCD\"


echo Press any key to exit this tool.
pause >nul