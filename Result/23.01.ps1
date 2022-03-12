$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition 
$netSimPath = Join-Path $scriptPath "..\NetSim\bin\Release\net5.0\NetSim.exe"
$settingsPath = Join-Path $scriptPath "12.01 300 Nodes\networkSettings.json"
#плотность 3,54 
#порог перколяции 0,31
#1- 0,31 = 0.69  

# -l 50
$dataFlow = 100, 500, 1000, 1500
$dataFlow = 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150
	& $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p 0.69
    & $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p 0.31
    & $netSimPath -i $settingsPath -r allnet3 -q $q -n 150 -p 0.69
	& $netSimPath -i $settingsPath -r allnet3 -q $q -n 150 -p 0.31
}

# -l 100
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -l 100
	& $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p 0.69 -l 100
    & $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p 0.31 -l 100
    & $netSimPath -i $settingsPath -r allnet3 -q $q -n 150 -p 0.69 -l 100
	& $netSimPath -i $settingsPath -r allnet3 -q $q -n 150 -p 0.31 -l 100
}

# -f 1
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -f 1
	& $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p 0.69 -f 1
    & $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p 0.31 -f 1
    & $netSimPath -i $settingsPath -r allnet3 -q $q -n 150 -p 0.69 -f 1
	& $netSimPath -i $settingsPath -r allnet3 -q $q -n 150 -p 0.31 -f 1
}
 #-l 100 -f 1
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -f 1 -l 100
	& $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p 0.69 -f 1 -l 100
    & $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p 0.31 -f 1 -l 100
    & $netSimPath -i $settingsPath -r allnet3 -q $q -n 150 -p 0.69 -f 1 -l 100
	& $netSimPath -i $settingsPath -r allnet3 -q $q -n 150 -p 0.31 -f 1 -l 100
}
