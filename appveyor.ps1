#Requires -Version 3.0

Param(
	[string]$ProjectName,
	[switch]$PreBuild,
	[switch]$PostBuild,
	[switch]$PostTest
)

$ProjectNameLower = $ProjectName.ToLower()

$Root = "C:\projects\build"
If (-not (test-path $Root)) {
	mkdir $Root | out-null
}

# check that the powershell scripts exist (should be cloned before this script runs)
$ScriptDir = "$Root\Powershell\Scripts"
If (-not (test-path $ScriptDir)) {
	Write-Host -Foreground Red "Powershell scripts not found. Exiting."
	Return
}

# source external scripts
. "$ScriptDir\Utils-GitHub.ps1"
. "$ScriptDir\Utils-Appveyor.ps1"

Function Package-Path($name)
{
    (Get-ChildItem -path "$env:APPVEYOR_BUILD_FOLDER\packages" | Where { $_ -like "*$name*" }).FullName
}

If ($PreBuild) {
	# Restore nuget packages and get dependent libraries
	Write-Host -Foreground Cyan "Cloning dependencies from GitHub"	
	GetLatestRelease $Root "HearthSim" "Hearthstone-Deck-Tracker" -Scrape
	$ExtractPath = Join-Path -Path $Root -ChildPath "Hearthstone Deck Tracker"
	Rename-Item -NewName "$ExtractPath\HearthstoneDeckTracker.exe" "$ExtractPath\Hearthstone Deck Tracker.exe"
	GetLatestRelease $Root "andburn" "hdt-plugin-common" -Scrape
	Write-Host -Foreground Cyan "Restoring nuget packages"	
	nuget restore
} ElseIf ($PostBuild) {
	# Create a release package
	Write-Host -Foreground Cyan "Creating deployment artifacts"
	BuildArtifacts $ProjectName "hdt-plugin-$ProjectNameLower" "$Root\$ProjectName" "bin\x86\Release"
} ElseIf ($PostTest) {
	$OpenCoverPath = Package-Path('OpenCover')
	$NunitPath = Package-Path('NUnit.ConsoleRunner')
	$CoverallsPath = Package-Path('coveralls')

	& "$OpenCoverPath\tools\OpenCover.Console.exe" `
		-register:user `
		-target:"$NunitPath\tools\nunit3-console.exe" `
		-targetargs:"--noheader /domain:single $ProjectName.Tests/bin/x86/release/$ProjectName.Tests.dll" `
		-filter:"+[$ProjectName]*" -mergebyhash -skipautoprops `
		-output:"coverage.xml"

	& "$CoverallsPath\tools\coveralls.net.exe" --opencover coverage.xml
}
