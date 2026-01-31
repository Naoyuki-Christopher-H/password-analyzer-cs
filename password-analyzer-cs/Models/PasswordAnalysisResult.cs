namespace password_analyzer_cs.Models
{
    /// <summary>
    /// Represents the result of password analysis
    /// </summary>
    public class PasswordAnalysisResult
    {
        public string Password { get; set; }

        public string Strength { get; set; }

        public int Score { get; set; }

        public int MaxScore { get; set; } = 100;

        public List<string> RequirementsMet { get; set; } = new List<string>();

        public List<string> RequirementsMissing { get; set; } = new List<string>();

        public int Length { get; set; }

        public bool HasUpperCase { get; set; }

        public bool HasLowerCase { get; set; }

        public bool HasNumbers { get; set; }

        public bool HasSpecialChars { get; set; }

        public bool IsCommonPassword { get; set; }
    }
}