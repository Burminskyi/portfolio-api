using PortfolioApi.Models;

namespace PortfolioApi.Helpers;

public static class SkillLevelParser
{
    public static SkillLevel Parse(string level)
    {
        return level.Trim().ToLowerInvariant() switch
        {
            "beginner" => SkillLevel.Beginner,
            "intermediate" => SkillLevel.Intermediate,
            "advanced" => SkillLevel.Advanced,
            "expert" => SkillLevel.Expert,
            _ => throw new ArgumentException($"Invalid skill level: '{level}'. Allowed: beginner, intermediate, advanced, expert.")
        };
    }

    public static string ToApiString(SkillLevel level) => level switch
    {
        SkillLevel.Beginner => "beginner",
        SkillLevel.Intermediate => "intermediate",
        SkillLevel.Advanced => "advanced",
        SkillLevel.Expert => "expert",
        _ => "beginner"
    };
}
