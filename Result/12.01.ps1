$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition 
$netSimPath = Join-Path $scriptPath "..\NetSim\bin\Release\net5.0\NetSim.exe"
$settingsPath = Join-Path $scriptPath "..\NetworkSettingsCreator\bin\Debug\net5.0\networkSettings.json"

# -l 50
#& $netSimPath -i $settingsPath -r dijkstraqueue -q 100 -n 150
#& $netSimPath -i $settingsPath -r composite -q 100 -n 150
#& $netSimPath -i $settingsPath -r composite -q 100 -n 150 -p 0.34
& $netSimPath -i $settingsPath -r composite -q 100 -n 150 -p 0.71

#& $netSimPath -i $settingsPath -r dijkstraqueue -q 500 -n 150 
#& $netSimPath -i $settingsPath -r composite -q 500 -n 150
#& $netSimPath -i $settingsPath -r composite -q 500 -n 150 -p 0.34
& $netSimPath -i $settingsPath -r composite -q 500 -n 150 -p 0.71    

#& $netSimPath -i $settingsPath -r dijkstraqueue -q 1000 -n 150
#& $netSimPath -i $settingsPath -r composite -q 1000 -n 150
#& $netSimPath -i $settingsPath -r composite -q 1000 -n 150 -p 0.34
& $netSimPath -i $settingsPath -r composite -q 1000 -n 150 -p 0.71

#& $netSimPath -i $settingsPath -r dijkstraqueue -q 1500 -n 150
#& $netSimPath -i $settingsPath -r composite -q 1500 -n 150
#& $netSimPath -i $settingsPath -r composite -q 1500 -n 150 -p 0.34
& $netSimPath -i $settingsPath -r composite -q 1500 -n 150 -p 0.71

# -l 100
#& $netSimPath -i $settingsPath -r dijkstraqueue -q 100 -n 150 -l 100
#& $netSimPath -i $settingsPath -r composite -q 100 -n 150 -l 100
#& $netSimPath -i $settingsPath -r composite -q 100 -n 150 -p 0.34 -l 100
& $netSimPath -i $settingsPath -r composite -q 100 -n 150 -p 0.71 -l 100

#& $netSimPath -i $settingsPath -r dijkstraqueue -q 500 -n 150 -l 100
#& $netSimPath -i $settingsPath -r composite -q 500 -n 150 -l 100
#& $netSimPath -i $settingsPath -r composite -q 500 -n 150 -p 0.34 -l 100
& $netSimPath -i $settingsPath -r composite -q 500 -n 150 -p 0.71 -l 100

#& $netSimPath -i $settingsPath -r dijkstraqueue -q 1000 -n 150 -l 100
#& $netSimPath -i $settingsPath -r composite -q 1000 -n 150 -l 100
#& $netSimPath -i $settingsPath -r composite -q 1000 -n 150 -p 0.34 -l 100
& $netSimPath -i $settingsPath -r composite -q 1000 -n 150 -p 0.71 -l 100

#& $netSimPath -i $settingsPath -r dijkstraqueue -q 1500 -n 150 -l 100
#& $netSimPath -i $settingsPath -r composite -q 1500 -n 150 -l 100
#& $netSimPath -i $settingsPath -r composite -q 1500 -n 150 -p 0.34 -l 100
& $netSimPath -i $settingsPath -r composite -q 1500 -n 150 -p 0.71 -l 100

# -f 1
#& $netSimPath -i $settingsPath -r dijkstraqueue -q 100 -f 1 -n 150
#& $netSimPath -i $settingsPath -r composite -q 100 -f 1 -n 150
#& $netSimPath -i $settingsPath -r composite -q 100 -f 1 -n 150 -p 0.34  
& $netSimPath -i $settingsPath -r composite -q 100 -f 1 -n 150 -p 0.71  

#& $netSimPath -i $settingsPath -r dijkstraqueue -q 500 -f 1 -n 150 
#& $netSimPath -i $settingsPath -r composite -q 500 -f 1 -n 150
#& $netSimPath -i $settingsPath -r composite -q 500 -f 1 -n 150 -p 0.34
& $netSimPath -i $settingsPath -r composite -q 500 -f 1 -n 150 -p 0.71    

#& $netSimPath -i $settingsPath -r dijkstraqueue -q 1000 -f 1 -n 150
#& $netSimPath -i $settingsPath -r composite -q 1000 -f 1 -n 150
#& $netSimPath -i $settingsPath -r composite -q 1000 -f 1 -n 150 -p 0.34
& $netSimPath -i $settingsPath -r composite -q 1000 -f 1 -n 150 -p 0.71

#& $netSimPath -i $settingsPath -r dijkstraqueue -q 1500 -f 1 -n 150
#& $netSimPath -i $settingsPath -r composite -q 1500 -f 1 -n 150
#& $netSimPath -i $settingsPath -r composite -q 1500 -f 1 -n 150 -p 0.34
& $netSimPath -i $settingsPath -r composite -q 1500 -f 1 -n 150 -p 0.71

# без блокировки NetSimNotBlock3
#& $netSimPath -i $settingsPath -r dijkstraqueue -q 100 -a false -n 150
#& $netSimPath -i $settingsPath -r composite -q 100 -a false -n 150
#& $netSimPath -i $settingsPath -r composite -q 100 -a false -n 150 -p 0.34
& $netSimPath -i $settingsPath -r composite -q 100 -a false -n 150 -p 0.71

#& $netSimPath -i $settingsPath -r dijkstraqueue -q 500 -a false -n 150 
#& $netSimPath -i $settingsPath -r composite -q 500 -a false -n 150
#& $netSimPath -i $settingsPath -r composite -q 500 -a false -n 150 -p 0.34
& $netSimPath -i $settingsPath -r composite -q 500 -a false -n 150 -p 0.71  

#& $netSimPath -i $settingsPath -r dijkstraqueue -q 1000 -a false -n 150
#& $netSimPath -i $settingsPath -r composite -q 1000 -a false -n 150
#& $netSimPath -i $settingsPath -r composite -q 1000 -a false -n 150 -p 0.34
& $netSimPath -i $settingsPath -r composite -q 1000 -a false -n 150 -p 0.71

#& $netSimPath -i $settingsPath -r dijkstraqueue -q 1500 -a false -n 150
#& $netSimPath -i $settingsPath -r composite -q 1500 -a false -n 150
#& $netSimPath -i $settingsPath -r composite -q 1500 -a false -n 150 -p 0.34
& $netSimPath -i $settingsPath -r composite -q 1500 -a false -n 150 -p 0.71
