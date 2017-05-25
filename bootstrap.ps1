#Requires -Version 3.0

$RootDir = $PSScriptRoot
$Common = "Common"
$CommonRepo = "https://github.com/andburn/hdt-plugin-common.git"
$TempName = "CommonTemp"
$TempDir = "$RootDir\$TempName"

Function RemoveDirectoryIfExists {
Param( [string]$Directory )
    if (Test-Path $Directory) {
        Remove-Item $Directory -Force -Recurse
    }
}

Function DirectoryExistsAndIsNonEmpty {
Param( [string]$Directory )
    Test-Path "$Directory\*"
}

Function ErrorAndExit {
Param( [string]$Message )
    Write-Host -ForegroundColor Red "Error: $Message"
    Exit
}

Function GetSolutionName {
    (Get-ChildItem -Path $RootDir | Where { $_.Name -match ".+?\.sln" }).BaseName
}

Function GetProjectGUIDFromSolution {
Param( [string]$Name )
    $Solution = "$RootDir\$Name.sln"
    $Project = "$Name.$Common"
    if (-not (Test-Path $Solution)) {
        ErrorAndExit "$Solution not found"
    }
    $regex = " = `"$Project`", `"$Project\\$Project.csproj`", `"\{([A-Z0-9\-]+)\}`""
    Get-Content $Solution | ForEach-Object {
        if($_ -match $regex){
            return $Matches[1]
        }
    }
}

Function ReplaceCommonProjectItem {
Param(
    [string]$Name,
    [string]$Value
)
    $Project = "$TempDir\$Common\$Common.csproj"
    if (-not (Test-Path $Project)) {
        ErrorAndExit "$Project not found"
    }
    $file = Get-Content $Project
    $file -replace "<$Name>.+?</$Name>", "<$Name>$Value</$Name>" | Set-Content $Project
}

Function ReplacePackAddresses {
Param(
    [string]$Name
)
    # not the most robust way, but faster than searching all files
    $files = "Controls\MoonTextButton.xaml", "Controls\Styles.xaml", "Utils\PluginMenu.cs"
    foreach ($f in $files) {
        $file = "$TempDir\$Common\$f"
        $content = Get-Content $file
        $content -replace "pack://application:,,,/$Common;", "pack://application:,,,/$Name.$Common;" | Set-Content $file
    }
}

# git is required exit if not found
if (Get-Command "git.exe" -ErrorAction SilentlyContinue) {
    RemoveDirectoryIfExists $TempDir
    # clone the Common repo to a temp directory
    git clone --depth=1 $CommonRepo $TempName
    if (-not (DirectoryExistsAndIsNonEmpty $TempDir)) {
        ErrorAndExit "failed to clone git repository"
    }
    # get the name of the plugin/solution
    $name = GetSolutionName
    # get the GUID assigned to the common project of this plugin
    $guid = GetProjectGUIDFromSolution $name
    # replace the original GUID with the this plugins one
    ReplaceCommonProjectItem "ProjectGuid" "{$guid}"
    # prefix the assembly name witht ths plugin's name
    ReplaceCommonProjectItem "AssemblyName" "$name.$Common"
    # replace pack addresses with new name
    ReplacePackAddresses $name
    # rename the common project for this plugin
    Rename-Item "$TempDir\$Common\$Common.csproj" "$TempDir\$Common\$name.$Common.csproj"
    Rename-Item "$TempDir\$Common" "$TempDir\$name.$Common"
    # delete any existing files in this plugins common project
    Remove-Item "$RootDir\$name.$Common\*" -Recurse
    # copy the new Common project in its place
    Copy-Item "$TempDir\$name.$Common" $RootDir -Force -Recurse
    # remove the temporary files
    RemoveDirectoryIfExists $TempDir
} else {
    ErrorAndExit "git not found, make sure it is included in `$Path"
}
