using Microsoft.AspNetCore.Mvc;
using password_analyzer_cs.Models;
using System.Text.RegularExpressions;

namespace password_analyzer_cs.Controllers
{
    public class PasswordController : Controller
    {
        /// <summary>
        /// Common passwords that should be avoided
        /// </summary>
        private readonly HashSet<string> _commonPasswords = new HashSet<string>
        {
            "password", "123456", "12345678", "123456789", "12345",
            "qwerty", "abc123", "password1", "admin", "welcome"
        };

        /// <summary>
        /// Default action - shows password analysis form
        /// </summary>
        public IActionResult Analyze()
        {
            return View();
        }

        /// <summary>
        /// Analyzes password strength based on security criteria
        /// </summary>
        [HttpPost]
        public IActionResult Analyze(string password)
        {
            var result = new PasswordAnalysisResult
            {
                Password = password,
                Length = password?.Length ?? 0
            };

            AnalyzePassword(result);
            CalculateScore(result);
            DetermineStrength(result);

            return View(result);
        }

        /// <summary>
        /// Analyzes password against security criteria
        /// </summary>
        private void AnalyzePassword(PasswordAnalysisResult result)
        {
            if (string.IsNullOrEmpty(result.Password))
            {
                return;
            }

            // Check for uppercase letters
            result.HasUpperCase = Regex.IsMatch(result.Password, "[A-Z]");
            if (result.HasUpperCase)
            {
                result.RequirementsMet.Add("Uppercase letter");
            }
            else
            {
                result.RequirementsMissing.Add("Uppercase letter");
            }

            // Check for lowercase letters
            result.HasLowerCase = Regex.IsMatch(result.Password, "[a-z]");
            if (result.HasLowerCase)
            {
                result.RequirementsMet.Add("Lowercase letter");
            }
            else
            {
                result.RequirementsMissing.Add("Lowercase letter");
            }

            // Check for numbers
            result.HasNumbers = Regex.IsMatch(result.Password, "[0-9]");
            if (result.HasNumbers)
            {
                result.RequirementsMet.Add("Number");
            }
            else
            {
                result.RequirementsMissing.Add("Number");
            }

            // Check for special characters
            result.HasSpecialChars = Regex.IsMatch(result.Password, "[^a-zA-Z0-9]");
            if (result.HasSpecialChars)
            {
                result.RequirementsMet.Add("Special character");
            }
            else
            {
                result.RequirementsMissing.Add("Special character");
            }

            // Check length
            if (result.Length >= 8)
            {
                result.RequirementsMet.Add("Minimum 8 characters");
            }
            else
            {
                result.RequirementsMissing.Add("Minimum 8 characters");
            }

            // Check if it's a common password
            result.IsCommonPassword = _commonPasswords.Contains(result.Password.ToLower());
            if (result.IsCommonPassword)
            {
                result.RequirementsMissing.Add("Avoid common passwords");
            }
            else if (!string.IsNullOrEmpty(result.Password))
            {
                result.RequirementsMet.Add("Not a common password");
            }
        }

        /// <summary>
        /// Calculates password strength score
        /// </summary>
        private void CalculateScore(PasswordAnalysisResult result)
        {
            if (string.IsNullOrEmpty(result.Password))
            {
                result.Score = 0;
                return;
            }

            int score = 0;

            // Length score (max 30 points)
            score += Math.Min(result.Length * 2, 30);

            // Character variety scores
            if (result.HasUpperCase) score += 15;
            if (result.HasLowerCase) score += 15;
            if (result.HasNumbers) score += 15;
            if (result.HasSpecialChars) score += 15;

            // Penalty for common passwords
            if (result.IsCommonPassword) score = Math.Max(0, score - 30);

            // Bonus for length over 12
            if (result.Length > 12) score += 10;

            result.Score = Math.Min(score, result.MaxScore);
        }

        /// <summary>
        /// Determines password strength category
        /// </summary>
        private void DetermineStrength(PasswordAnalysisResult result)
        {
            if (result.Score >= 80)
            {
                result.Strength = "Strong";
            }
            else if (result.Score >= 50)
            {
                result.Strength = "Medium";
            }
            else
            {
                result.Strength = "Weak";
            }
        }
    }
}