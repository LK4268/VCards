﻿using FolkerKinzel.VCards.Models.Enums;

namespace FolkerKinzel.VCards.Intls.Converters
{
    internal static class VCdVersionConverter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Keine allgemeinen Ausnahmetypen abfangen", Justification = "<Ausstehend>")]
        internal static VCdVersion Parse(string? value)
        {
            try
            {
                return (value?[0]) switch
                {
                    '4' => VCdVersion.V4_0,
                    '3' => VCdVersion.V3_0,
                    _ => VCdVersion.V2_1
                };
            }
            catch
            {
                return VCdVersion.V2_1;
            }
        }


    }
}
