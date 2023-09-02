$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition 
$netSimPath = Join-Path $scriptPath "..\NetSim\bin\Release\net5.0\NetSim.exe"
$settingsPath = Join-Path $scriptPath "2.57\Settings\networkSettings.json"
#плотность 3,54 
#порог перколяции 0,31

#плотность 3,08 
#порог перколяции 0,37

#плотность 2,57 
#порог перколяции 0,49

# -l 50
$p =  0.49
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150
	& $netSimPath -i $settingsPath -r allnet3 -q $q -n 150 -p $p
    & $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p $p
}

# -l 100
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -l 100
 	& $netSimPath -i $settingsPath -r allnet3 -q $q -n 150 -p $p -l 100
    & $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p $p -l 100
}

# -f 1
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -f 1
    & $netSimPath -i $settingsPath -r allnet3 -q $q -n 150 -p $p -f 1
    & $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p $p -f 1
}
 #-l 100 -f 1
$dataFlow = 100, 500, 1000, 1500, 300, 700, 900, 1300
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r dijkstraqueue -q $q -n 150 -f 1 -l 100
    & $netSimPath -i $settingsPath -r allnet3 -q $q -n 150 -p $p -f 1 -l 100
    & $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p $p -f 1 -l 100

}
