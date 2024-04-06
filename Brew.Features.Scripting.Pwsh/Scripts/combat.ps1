
function Invoke-Combat {
    param($player, $enemy, $logger)
    
    $player.DealDamage($enemy, 10)
    
    if ($enemy.Health -le 0) {
        $logger.LogInformation("$($enemy.Name) is defeated")
        return
    }

    $enemy.DealDamage($player, 5)
    
    if ($player.Health -le 0) {
        $logger.LogInformation("$($player.Name) is defeated")
        return
    }

    $logger.LogInformation("$($player.Name) has $($player.Health) health left")
    $logger.LogInformation("$($enemy.Name) has $($enemy.Health) health left")
}

Invoke-Combat -player $player -enemy $enemy -logger $logger
