namespace _241611JalopFinalAssignment1.Models;

// Page headings and navigation read the same titles so an example has one name everywhere.
public static class JALOPARLANMARTIN_241611_ExampleCatalog
{
    // These private arrays preserve the five approved slots in each category.
    private static readonly IReadOnlyDictionary<string, string[]> Titles = new Dictionary<string, string[]>
    {
        ["input-output"] = new[]
        {
            "ID Card Generator", "Area and Perimeter Calculator", "Temperature Converter", "Time Unit Converter", "Character and Word Counter"
        },
        ["nested-if-else"] = new[]
        {
            "Login and Access Checker", "Latin Honors Evaluator", "BMI Calculator", "Triangle Classifier", "Largest of Three Numbers"
        },
        ["loops"] = new[]
        {
            "Pyramid Generator", "Sum of Digits", "Prime Factorization", "Number Guessing Game", "Digit Counter"
        },
        ["arrays"] = new[]
        {
            "Duplicate Finder", "Array Sort", "Largest and Smallest", "Multiplication Table", "House Member Finder"
        }
    };

    public static string GetTitle(string category, int number)
    {
        // URLs count from one; arrays count from zero. Unknown selections keep a neutral fallback.
        return Titles.TryGetValue(category, out var titles) && number >= 1 && number <= titles.Length
            ? titles[number - 1]
            : $"Example {number}";
    }
}
