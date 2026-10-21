
using TheWorld.GameMechanics;
using TheWorld.Display;
using TheWorld.GameMechanics.StockEntities;
using TheWorld.Rules;
using static TheWorld.Rules.DiceRollStrategies;

// This is the one Random Number Generator for the whole program. We inject it
//everywhere a roll happens.
    Random rng = new();

// The party.  Bree is the wizard;  Tamsin is the fighter.
var bree = StockPlayers.Bree;
var tamsin = StockPlayers.Tamsin;
    
    
    
PlayerBuilder.FromName("Hardcore").WithAbilities(rng).Build().ShowSheet();
PlayerBuilder.FromName("Lucky")
    .WithAbilities(rng, Dice.D6 with { Count = 3 }, HighestOfThreeRolls)
    .Build().ShowSheet();
    
// What they're up against.
var goblin = Creature.Create("Goblin", 7, 7, 8, 14, 10, 10, 8, 8, 50);

bree.ShowSheet();
tamsin.ShowSheet();
goblin.ShowSheet();



//              :::TESTS:::
 
//var rng = new Random(42);
//Console.WriteLine(Dice.D20.Roll(rng, DiceRollStrategies.Default));
//Console.WriteLine(Dice.D20.Roll(rng, DiceRollStrategies.Advantage));
//Console.WriteLine(Dice.D20.Roll(rng, DiceRollStrategies.Disadvantage));
//Console.WriteLine(Dice.D6 with { Count = 2, Modifier = 3 });
//Console.WriteLine(Dice.D6 with { Sides = 7 });
Console.WriteLine(Dice.D6 with { Count = 0 });
Console.WriteLine($"Tamsin swings at the goblin:  {tamsin.AttackBonus:+0;-0;+0} to hit, against AC {goblin.ArmorClass}.");
Console.WriteLine($"The goblin swings back:  {goblin.AttackBonus:+0;-0;+0} to hit, against AC {tamsin.ArmorClass}.");
