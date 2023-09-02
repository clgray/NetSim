$q = 0
$a = ''
$g = ''
$l = 0
$i =0
$colors = @( 'magenta')
Get-Content .\logs20220813.txt | ForEach-Object {
    if($_ -match 'RoutingAlgorithm (?<algorithm>.+?),'){
        $a= $Matches.algorithm
    }
    if($_ -match ' (?<lam>0,\d\d$)'){
        $l=  $Matches.lam
    }
    if($_ -match 'Quantity (?<Quantity>\d*)'){
        if ($q -ne $Matches.Quantity)
        {
            Write-Output ']'
			Write-Output 'showStatBlocked();'  
			Write-Output 'showStatMessage();'
			Write-Output ' '
            $q = $Matches.Quantity
            [string]::Format("quantity = {0};", $q)
            Write-Output 'log_tags = ['
            
        }
    }
    if($_ -match '(?<guid>[A-Z0-9]{8}-([A-Z0-9]{4}-){3}[A-Z0-9]{12})'){
        $g= $Matches.guid
        [string]::Format('{{"tag":  "{0}", "label": "{1} {2}", "color": "{3}"}},', $g, $a, $l, $colors[$i])
        $i=$i+1;
        if ($i -eq $colors.Length)
        {$i = 0;}
    }
     
}


