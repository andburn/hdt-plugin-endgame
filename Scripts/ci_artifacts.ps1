$out_name = "hdt-plugin-endgame_$env:APPVEYOR_REPO_TAG_NAME.zip"
mkdir C:\Release\EndGame
Copy-Item -Path "$env:APPVEYOR_BUILD_FOLDER\EndGame\bin\x86\Release\*dll*" -Destination C:\Release\EndGame
& 7z a C:\Release\endgame.zip C:\Release\EndGame
Rename-Item -NewName "C:\Release\$out_name" -Path 'C:\Release\endgame.zip'
Push-AppveyorArtifact "C:\Release\$out_name" -FileName $out_name -DeploymentName release
