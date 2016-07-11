OpenCover.Console.exe ^
-register:user ^
-target:nunit3-console.exe ^
-targetargs:"--noheader --noresult %~dp0..\EndGameTests\EndGameTests.csproj" ^
-filter:"+[EndGame]*" ^
-mergebyhash ^
-skipautoprops ^
-output:"%~dp0..\coverage\opencover.results.xml"

ReportGenerator.exe -reports:"%~dp0..\coverage\opencover.results.xml" -targetdir:"%~dp0..\coverage" -reporttypes:TextSummary;Html
cat "%~dp0..\coverage\Summary.txt"
