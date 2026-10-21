using TheWorld.Rules;
using static TheWorld.Rules.DiceRollStrategies;

namespace TheWorld.GameMechanics;

/// <summary>
/// Builds a <see cref="Player"/> one part at a time.
///
/// Start from a name with FromName.  Each part gets its own With... method,
/// and Build() puts the finished player together.  Any part you skip keeps its
/// default.
///
/// The builder is a record, so every With... hands back a new copy with one
/// part changed.  The builder you called it on is left exactly as it was.
/// </summary>
public record PlayerBuilder
{
    
    // Default values for every part of a Character.  This is ONE WAY (not
    // necessarily a GOOD way) to guarantee that Build() always makes a valid
    // Character...
    // TODO:  Find a better way!
    private string _name = "Frank";
    private PlayerLevel _level = PlayerLevel.Level1;
    private HealthPoints _healthPoints = HealthPoints.Create(20);
    private AbilityScore _abilities = AbilityScore.Create(10, 10, 10, 10, 10, 10)!;
    private Money _wallet = Money.Default;

    /// <summary>
    /// Does Nothing, except force every caller to start from one of the From... Factory Methods.
    /// </summary>
    private PlayerBuilder() { }

    /// <summary>
    /// Start with a FROM method.  The first natural place to start is the player's name.
    /// </summary>
    /// <param name="name">Checked by ValidateName, at the bottom of this file.</param>
    public static PlayerBuilder FromName(string name) => new() { _name = ValidateName(name) };

    //------------------------------------------------------------------------------------
    // With methods are how we "Build" the Player from its constituent parts.
    // Blindly handing the values to the class is the most basic way to do this,
    // and it checks no more than the Factory method we wrote in Lesson 5.
    // Which means we can do better!
    //------------------------------------------------------------------------------------

    public PlayerBuilder WithLevel(PlayerLevel level) => this with { _level = level };
    public PlayerBuilder WithHealthPoints(HealthPoints hp) => this with { _healthPoints = hp };
    public PlayerBuilder WithAbilities(AbilityScore abilities) => this with { _abilities = abilities };
    public PlayerBuilder WithWallet(Money wallet) => this with { _wallet = wallet };
    
    
    /// <summary>
    /// Here's the traditional hard-core way to do this.
    /// 3d6 for each score, rolled in order, and whatever comes up is what you get.
    /// You could end up with all 3s or all 18s. And you only get one shot.
    /// </summary>
    public PlayerBuilder WithAbilities(Random rng) => this with
    { _abilities = AbilityScore.Roll(rng, Dice.D6 with { Count = 3 }, Default)
    };
    
    /// <summary>
    /// Puts the player together from whatever parts have been set.
    /// </summary>
    public Player Build() => 
        Player.Create(_name, _level, _healthPoints, _abilities, _wallet);
    
    /// <summary>
    /// The Fully Strategy Based approach gives the programmer the power to
    ///manipulate the outcome, or to
    /// create undesirable ones. The scores come back unchecked. That
    ///responsibility is outsourced to the
    /// DiceRollStrategy you hand in.
    /// </summary>
    public PlayerBuilder WithAbilities(Random rng, Dice dice, DiceRollStrategy
        strategy) => this with
        { _abilities = AbilityScore.Roll(rng, dice, strategy) };
    
    #region Future Projects!
    /// <summary>
    /// TODO:  Research:  JSON Serialization of Objects.
    /// </summary>
    /// <exception cref="NotImplementedException">Always, for now.</exception>
    public static PlayerBuilder FromJson(string jsonString) => 
        throw new NotImplementedException("Not yet implemented");
    
    /// <summary>
    /// Use Pattern Matching to make sure the chosen name is Valid.
    /// `string.IsNullOrWhiteSpace` catches `null`, "", and "                 " all in one case.
    /// The `.Trim()` method gets rid of any leading or trailing White-Space characters.
    /// TODO:  Add more cases to this switch statement to catch other possible invalid names.
    /// TODO:  Research:  Use "Regular Expressions" to validate that a player name is made up ONLY of
    /// Alphabet characters, with valid punctuation inside the name.
    /// </summary>
    /// <returns>The trimmed name, or "Frank" when there's nothing there but White-Space.</returns>
    private static string ValidateName(string name) =>
        name switch
        {
            var n when string.IsNullOrWhiteSpace(n) => "Frank",
            _ => name.Trim()
        };
    #endregion
}
