$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition 
$netSimPath = Join-Path $scriptPath "..\NetSim\bin\Release\net5.0\NetSim.exe"
$settingsPath = Join-Path $scriptPath "..\NetworkSettingsCreator\bin\Debug\net5.0\networkSettings.json"
#плотность 3,08 
#порог перколяции 0,37
#1- 0,31 = 0.69  

# -l 50
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.37
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.69
}

# -l 100
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -l 100
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.37 -l 100
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.69 -l 100
}

# -f 1
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -f 1
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.37 -f 1
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.69 -f 1
}
 #-l 100 -f 1
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -l 100 -f 1
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.37 -l 100 -f 1
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.69 -l 100 -f 1
}

# без блокировки NetSimNotBlock3
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -a false
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.37 -a false
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.69 -a false
}
# -l 100 без блокировки 
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -l 100 -a false
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.37 -l 100 -a false
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.69 -l 100 -a false
}

# -f 1 без блокировки 
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -f 1 -a false
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.37 -f 1 -a false
    & $netSimPath -i $settingsPath -r composite -q $q -n 150 -p 0.69 -f 1 -a false
}
