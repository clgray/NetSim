#Invoke-Expression "& `"..\NetSim\NetSim\bin\Release\net5\NetSim.exe`" -i "\NetSim\NetworkSettingsCreator\bin\Debug\net5.0\networkSettings.json" -r dijkstraf6
$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition 
$netSimPath = Join-Path $scriptPath "..\NetSim\bin\Release\net5.0\NetSim.exe"
$settingsPath = Join-Path $scriptPath "..\NetworkSettingsCreator\bin\Debug\net5.0\networkSettings.json"


& $netSimPath -i $settingsPath -r dijkstraqueue -q 1000 -n 150
& $netSimPath -i $settingsPath -r composite -q 1000 -n 150