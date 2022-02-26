$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition 
$netSimPath = Join-Path $scriptPath "..\NetSim\bin\Release\net5.0\NetSim.exe"
$settingsPath = Join-Path $scriptPath "12.01 300 Nodes\networkSettings.json"
#плотность 3,54 
#порог перколяции 0,34 (30)
#1- 0,29 = 0.71  

# -l 50
$dataFlow = 100, 500, 1000, 1500
foreach ($q in $dataFlow)
{
    & $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p 0.71
    & $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p 0.30
    & $netSimPath -i $settingsPath -r allnet3 -q $q -n 150 -p 0.71

}

# -l 100
#$dataFlow = 100, 500, 1000, 1500
#foreach ($q in $dataFlow)
#{
#    & $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p 0.71 -l 100
#    & $netSimPath -i $settingsPath -r allnet8 -q $q -n 150 -p 0.30 -l 100
#    & $netSimPath -i $settingsPath -r allnet3 -q $q -n 150 -p 0.71 -l 100
#    
#}

