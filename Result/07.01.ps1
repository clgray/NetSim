$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition 
$netSimPath = Join-Path $scriptPath "..\NetSim\bin\Release\net5.0\NetSim.exe"
$settingsPath = Join-Path $scriptPath "..\NetworkSettingsCreator\bin\Debug\net5.0\networkSettings.json"

& $netSimPath -i $settingsPath -r dijkstraf6 -q 100 -l 25
& $netSimPath -i $settingsPath -r dijkstrapath -q 100 -l 25
& $netSimPath -i $settingsPath -r dijkstraqueue -q 100 -l 25

& $netSimPath -i $settingsPath -r dijkstraf6 -q 300 -l 25
& $netSimPath -i $settingsPath -r dijkstrapath -q 300 -l 25
& $netSimPath -i $settingsPath -r dijkstraqueue -q 300 -l 25

& $netSimPath -i $settingsPath -r dijkstraf6 -q 500 -l 25
& $netSimPath -i $settingsPath -r dijkstrapath -q 500 -l 25
& $netSimPath -i $settingsPath -r dijkstraqueue -q 500 -l 25

& $netSimPath -i $settingsPath -r dijkstraf6 -q 1000 -l 25
& $netSimPath -i $settingsPath -r dijkstrapath -q 1000 -l 25
& $netSimPath -i $settingsPath -r dijkstraqueue -q 1000 -l 25
