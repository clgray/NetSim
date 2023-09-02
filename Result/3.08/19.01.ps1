$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition 
$netSimPath = Join-Path $scriptPath "..\..\NetSim\bin\Release\net5.0\NetSim.exe"
$settingsPath = Join-Path $scriptPath "Settings\networkSettings.json"
#плотность 2,57 
#порог перколяции 0,49
#1- 0,39 = 0.61  

# -l 50
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150
    & $netSimPath -i $settingsPath -r allnet10 -q $q -n 150 -p 0.49
}

# -l 100
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -l 100
    & $netSimPath -i $settingsPath -r allnet10 -q $q -n 150 -p 0.49 -l 100
}

# -f 1
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -f 1
    & $netSimPath -i $settingsPath -r allnet10 -q $q -n 150 -p 0.49 -f 1
}
 #-l 100 -f 1
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -l 100 -f 1
    & $netSimPath -i $settingsPath -r allnet10 -q $q -n 150 -p 0.49 -l 100 -f 1
}

# без блокировки NetSimNotBlock3
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -a false
    & $netSimPath -i $settingsPath -r allnet10 -q $q -n 150 -p 0.49 -a false
}
# -l 100 без блокировки 
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -l 100 -a false
    & $netSimPath -i $settingsPath -r allnet10 -q $q -n 150 -p 0.49 -l 100 -a false
}


