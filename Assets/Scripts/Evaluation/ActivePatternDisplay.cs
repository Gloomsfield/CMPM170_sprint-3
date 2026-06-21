using UnityEngine;
using System.Collections.Generic;

public class ActivePatternDisplay {
    private List<Pattern> _prevActivePatterns;
    public void UpdateDisplay(List<Pattern> activePatterns) {
        foreach (var pattern in activePatterns) {
            Debug.Log(pattern.ToString());
        }
    }
}
