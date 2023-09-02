$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition 
$netSimPath = Join-Path $scriptPath "..\..\NetSim\bin\Release\net5.0\NetSim.exe"
$settingsPath = Join-Path $scriptPath "3.08Settings\networkSettings.json"
#плотность 3,08 
#порог перколяции 0,37
#1- 0,37 = 0.63  

# -l 50
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150
    & $netSimPath -i $settingsPath -r allnet10 -q $q -n 150 -p 0.63
}

# -l 100
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -l 100
    & $netSimPath -i $settingsPath -r allnet10 -q $q -n 150 -p 0.63 -l 100
}

# -f 1
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -f 1
    & $netSimPath -i $settingsPath -r allnet10 -q $q -n 150 -p 0.63 -f 1
}
 #-l 100 -f 1
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -l 100 -f 1
    & $netSimPath -i $settingsPath -r allnet10 -q $q -n 150 -p 0.63 -l 100 -f 1
}

