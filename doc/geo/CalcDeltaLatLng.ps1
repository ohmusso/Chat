function CalcDistanceLatFixed{
    param(
        $r,
        $lat,
        $dlat
    )
    $s = [math]::sin($lat)*[math]::sin($lat+$dlat)
    $c = [math]::cos($lat)*[math]::cos($lat+$dlat)
    $r * [math]::acos($s+$c)
}

function CalcDistanceLngFixed{
    param(
        $r,
        $lat,
        $dlng
    )
    # sin(lat)^2
    $s = [math]::sin($lat)
    $s *= $s

    # cos(lat)^2 * cos(Δlng)
    $c = [math]::cos($lat)
    $c *= $c
    $c *= [math]::cos($dlng)

    $r * [math]::acos($s+$c)
}

function deg2lad{
    param(
        $deg
    )
    $deg * [math]::PI / 180
}

function lad2deg{
    param(
        $rad
    )
    $rad * (180 / [math]::PI)
}

function CheckErrorRate10per{
    param(
        $ref,
        $obj
    )
    $error = [math]::abs($obj - $ref) / $ref

    if( $error -le 0.1 ){
        $true
    }
    else{
        $false
    }
}

function CalcDeltaLat{
    param(
        $r,
        $lat,
        $dlat,
        $targetDist
    )
    $d = CalcDistanceLatFixed -r $r -lat $lat -dlat $dlat # 距離を算出

    if( CheckErrorRate10per -ref $targetDist -obj $d ){
        return $dlat # 目標距離になるΔlat
    }
    else{
        if( ($d - $targetDist) -lt 0 ){
            $dlat = $dlat * 1.05
        }
        else{
            $dlat = $dlat * 0.8
        }
        CalcDeltaLat -r $r -lat $lat -dlat $dlat -targetDist $targetDist # 再起呼び出し
    }
}

function CalcDeltaLng{
    param(
        $r,
        $lat,
        $dlng,
        $targetDist
    )
    $d = CalcDistanceLngFixed -r $r -lat $lat -dlng $dlng # 距離を算出

    if( CheckErrorRate10per -ref $targetDist -obj $d ){
        return $dlng # 目標距離になるΔlng
    }
    else{
        if( ($d - $targetDist) -lt 0 ){
            $dlng = $dlng * 1.05
        }
        else{
            $dlng = $dlng * 0.8
        }
        CalcDeltaLng -r $r -lat $lat -dlng $dlng -targetDist $targetDist # 再起呼び出し
    }
}

$R = 6378137 # [m]
$Dist = 500 # [m]

$ResultDlat = @()
$ResultDlng = @()
for( $i = 0; $i -lt 90; $i++){
    $lat = deg2lad -deg $i

    $dlat = deg2lad -deg 1.0
    $rad = CalcDeltaLat -r $R -lat $lat -dlat $dlat -targetDist $Dist # 再起呼び出し
    $ResultDlat += lad2deg -rad $rad

    $dlng = deg2lad -deg 1.0
    $rad = CalcDeltaLng -r $R -lat $lat -dlng $dlng -targetDist $Dist # 再起呼び出し
    $ResultDlng += lad2deg -rad $rad
}

Write-Host "Delta latitude that distance to be $($Dist)m"
$ResultDlat

Write-Host "Delta longitude that distance to be $($Dist)m"
$ResultDlng
