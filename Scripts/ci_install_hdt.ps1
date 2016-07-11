Add-Type -AssemblyName System.IO.Compression.FileSystem
$file = 'C:\hdt.zip'
$extract_to = 'C:\Projects'
$html = Invoke-WebRequest -Uri 'https://github.com/HearthSim/Hearthstone-Deck-Tracker/releases/latest'
$url = ($html.ParsedHtml.getElementsByTagName('a') | Where{ $_.href -like '*releases/download*' }).href.Replace('about:', 'https://github.com')
(New-Object Net.WebClient).DownloadFile($url, $file)
[System.IO.Compression.ZipFile]::ExtractToDirectory($file, $extract_to)
Rename-Item -NewName 'C:\Projects\release' -Path 'C:\Projects\Hearthstone Deck Tracker'
