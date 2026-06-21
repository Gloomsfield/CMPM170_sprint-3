using UnityEngine;
using System.Collections.Generic;

public class ActivePatternDisplay {
    private List<Pattern> _prevActivePatterns;
    public string GetActivePatternText(List<Pattern> activePatterns) {
        string s = "";
        foreach (var pattern in activePatterns) {
            s += $"{pattern.ToString()}\n";
        }
        return s;
    }
}
