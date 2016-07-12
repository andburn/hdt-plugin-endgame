function Package-Path($name)
{
    (Get-ChildItem -path "$env:APPVEYOR_BUILD_FOLDER\packages" | Where { $_ -like "*$name*" }).FullName
}

$OpenCoverPath = Package-Path('OpenCover')
$NunitPath = Package-Path('NUnit.ConsoleRunner')
$CoverallsPath = Package-Path('coveralls')

& "$OpenCoverPath\tools\OpenCover.Console.exe" `
    -register:user `
    -target:"$NunitPath\tools\nunit3-console.exe" `
    -targetargs:"--noheader /domain:single EndGameTests/bin/release/EndGameTests.dll" `
    -filter:"+[EndGame]*" -mergebyhash -skipautoprops `
    -output:"coverage.xml"

& "$CoverallsPath\tools\coveralls.net.exe" --opencover coverage.xml
