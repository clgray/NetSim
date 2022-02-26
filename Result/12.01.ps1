$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition 
$netSimPath = Join-Path $scriptPath "..\NetSim\bin\Release\net5.0\NetSim.exe"
$settingsPath = Join-Path $scriptPath "..\NetworkSettingsCreator\bin\Debug\net5.0\networkSettings.json"

# -l 50
#$dataFlow = 300, 700, 900, 1200
#foreach ($q in $dataFlow)
#{
#    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150
#    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.34
#    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.71
#}
#
## -l 100
#$dataFlow = 300, 700, 900, 1200
#foreach ($q in $dataFlow)
#{
#    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -l 100
#    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.34 -l 100
#    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.71 -l 100
#}
#
## -f 1
#$dataFlow = 300, 700, 900, 1200
#foreach ($q in $dataFlow)
#{
#    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -f 1
#    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.34 -f 1
#    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.71 -f 1
#}
# -l 100 -f 1
$dataFlow = 100, 300, 500, 700, 900, 1000, 1200, 1500
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -l 100 -f 1
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.34 -l 100 -f 1
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.71 -l 100 -f 1
}

# без блокировки NetSimNotBlock3
#$dataFlow = 300, 700, 900, 1200
#foreach ($q in $dataFlow)
#{
#    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -a false
#    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.34 -a false
#    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.71 -a false
#}
## -l 100 без блокировки 
#$dataFlow = 100, 300, 500, 700, 900, 1000, 1200, 1500
#foreach ($q in $dataFlow)
#{
#    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -l 100 -a false
#    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.34 -l 100 -a false
#    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.71 -l 100 -a false
#}
#
## -f 1 без блокировки 
#$dataFlow = 100, 300, 500, 700, 900, 1000, 1200, 1500
#foreach ($q in $dataFlow)
#{
#    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -f 1 -a false
#    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.34 -f 1 -a false
#    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.71 -f 1 -a false
#}
