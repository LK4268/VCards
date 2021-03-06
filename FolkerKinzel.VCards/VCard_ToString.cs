﻿using FolkerKinzel.VCards.Models;
using FolkerKinzel.VCards.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace FolkerKinzel.VCards
{
    public partial class VCard : IEnumerable<KeyValuePair<VCdProp, object>>
    {
        /// <summary>
        /// Erstellt eine <see cref="string"/>-Repräsentation des <see cref="VCard"/>-Objekts. 
        /// (Nur zum Debugging.)
        /// </summary>
        /// <returns>Eine <see cref="string"/>-Repräsentation des <see cref="VCard"/>-Objekts.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Version: ").Append(GetVersionString(this.Version))
                .Append(Environment.NewLine);


            foreach (var prop in this._propDic.OrderBy(x => x.Key))
            {
                switch (prop.Value)
                {
                    case IEnumerable numerable:
                        Debug.Assert(numerable != null);

                        foreach (var o in numerable)
                        {
                            if (o is null) continue;
                            var vcdProp = (IVCardData)o;
                            AppendProperty(prop, vcdProp);
                        }
                        break;
                    case IVCardData vcdProp:
                        AppendProperty(prop, vcdProp);
                        break;
                    default:
                        break;
                }
            }

            sb.Length -= Environment.NewLine.Length;

            return sb.ToString();

            ////////////////////////////////////////////

            static string GetVersionString(VCdVersion version)
            {
                return (version) switch
                {
                    VCdVersion.V2_1 => "2.1",
                    VCdVersion.V3_0 => "3.0",
                    VCdVersion.V4_0 => "4.0",
                    _ => "2.1"
                };
            }


            // ////////////////////////////////

            void AppendProperty(KeyValuePair<VCdProp, object> prop, IVCardData vcdProp)
            {
                const string INDENT = "    ";
                string s = vcdProp.Parameters.ToString();


                sb.AppendLine(); //Leerzeile

                if (s.Length != 0)
                {
                    sb.AppendLine(s);
                }


                if (vcdProp.Group != null)
                {
                    sb.Append('[').Append("Group: ").Append(vcdProp.Group).AppendLine("]");
                }

                var propStr = vcdProp.IsEmpty ? "<EMPTY>" : vcdProp.ToString();

                if (propStr != null &&
#if NET40
                    propStr.Contains(Environment.NewLine))
#else
                    propStr.Contains(Environment.NewLine, StringComparison.Ordinal))
#endif
                {
#if NET40
                    var arr = propStr.Split(
                        new string[] { Environment.NewLine }, StringSplitOptions.None);
#else
                    var arr = propStr.Split(Environment.NewLine, StringSplitOptions.None);
#endif

                    sb.Append(prop.Key).AppendLine(":");

                    foreach (var str in arr)
                    {
                        sb.Append(INDENT).AppendLine(str);
                    }
                }
                else
                {
                    sb.Append(prop.Key).Append(": ").AppendLine(propStr);
                }
            }
        }


    }
}
