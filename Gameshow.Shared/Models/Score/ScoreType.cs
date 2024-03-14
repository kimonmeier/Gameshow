namespace Gameshow.Shared.Models.Score;

/// <summary>
///  This enums 
/// </summary>
public enum ScoreType : int
{
    /// <summary>
    /// nothing is displayed
    /// </summary>
    None = 0,
    /// <summary>
    /// Just a regular number is displayed
    /// </summary>
    Points,
    /// <summary>
    /// 
    /// </summary>
    LimitedPoints,
    Buzzer,
}