﻿using FolkerKinzel.VCards.Models.Enums;
using System.Diagnostics;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class ExpertiseLevelConverter
    {
        private static class Values
        {
            internal const string Beginner = "beginner";
            internal const string Average = "average";
            internal const string Expert = "expert";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Zeichenfolgen in Großbuchstaben normalisieren", Justification = "<Ausstehend>")]
        internal static ExpertiseLevel? Parse(string val)
        {
            Debug.Assert(val != null);

            return val.ToLowerInvariant() switch
            {
                Values.Beginner => ExpertiseLevel.Beginner,
                Values.Average => ExpertiseLevel.Average,
                Values.Expert => ExpertiseLevel.Expert,
                _ => (ExpertiseLevel?)null,
            };
        }



        internal static string? ToVCardString(this ExpertiseLevel? expertise)
        {
            return expertise switch
            {
                ExpertiseLevel.Beginner => Values.Beginner,
                ExpertiseLevel.Average => Values.Average,
                ExpertiseLevel.Expert => Values.Expert,
                _ => null,
            };
        }
    }
}
