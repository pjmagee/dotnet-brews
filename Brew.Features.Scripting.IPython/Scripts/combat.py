def combat_round(player, enemy):

    player.DealDamage(enemy, 10)
    
    if enemy.Health <= 0:
        logger.LogInformation(enemy.Name + ' is defeated')
        return
        
    enemy.DealDamage(player, 5)
    
    if player.Health <= 0:
        logger.LogInformation(player.Name + ' is defeated')
        return
        
    logger.LogInformation(player.Name + ' has ' + str(player.Health) + ' health left')
    logger.LogInformation(enemy.Name + ' has ' + str(enemy.Health) + ' health left')

# Running multiple rounds until one is defeated
while character.Health > 0 and monster.Health > 0:
    combat_round(character, monster)
