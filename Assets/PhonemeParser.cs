using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Syl
{
    public static class PhonemeParser
    {
        public static readonly Dictionary<string, Sound> Map = new Dictionary<string, Sound>()
        {
            ["B"] = Sound.B,
            ["D"] = Sound.D,
            ["G"] = Sound.G,
            ["K"] = Sound.K,
            ["P"] = Sound.P,
            ["T"] = Sound.T,

            ["F"] = Sound.F,
            ["V"] = Sound.V,
            ["S"] = Sound.S,
            ["Z"] = Sound.Z,
            ["SH"] = Sound.SH,
            ["ZH"] = Sound.ZH,
            ["TH"] = Sound.TH,
            ["DH"] = Sound.DH,
            ["H"] = Sound.H,

            ["CH"] = Sound.CH,
            ["J"] = Sound.J,

            ["M"] = Sound.M,
            ["N"] = Sound.N,
            ["NG"] = Sound.NG,

            ["L"] = Sound.L,
            ["R"] = Sound.R,

            ["W"] = Sound.W,
            ["Y"] = Sound.Y,

            ["I"] = Sound.I,
            ["II"] = Sound.II,
            ["E"] = Sound.E,
            ["A"] = Sound.A,
            ["AA"] = Sound.AA,
            ["U"] = Sound.U,
            ["OO"] = Sound.OO,
            ["UU"] = Sound.UU,
            ["Ə"] = Sound.Ə,
            ["AI"] = Sound.AI,
            ["AU"] = Sound.AU,
            ["OI"] = Sound.OI,
            ["OU"] = Sound.OU,
            ["EI"] = Sound.EI
        };

        public static bool IsVowel(Sound s)
        {
            switch (s)
            {
                case Sound.I:
                case Sound.II:
                case Sound.E:
                case Sound.A:
                case Sound.AA:
                case Sound.U:
                case Sound.UU:
                case Sound.OO:
                case Sound.Ə:
                case Sound.AI:
                case Sound.AU:
                case Sound.OI:
                case Sound.OU:
                case Sound.EI:
                    return true;
                default:
                    return false;
            }
        }

        private static readonly string[] TokensByLength = Map.Keys.OrderByDescending(x => x.Length).ToArray();

        public static Syllable[] Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new Syllable[0];

            var syllableTexts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<Syllable>();

            foreach (var s in syllableTexts)
            {
                var phonemes = Tokenize(s);
                result.Add(BuildSyllable(phonemes, s));
            }
            return result.ToArray();
        }

        private static List<Sound> Tokenize(string text)
        {
            var result = new List<Sound>();
            int index = 0;

            while (index < text.Length)
            {
                bool matched = false;

                foreach (var token in TokensByLength)
                {
                    if (index + token.Length <= text.Length &&
                        string.Compare(text, index, token, 0, token.Length, StringComparison.Ordinal) == 0)
                    {
                        result.Add(Map[token]);
                        index += token.Length;
                        matched = true;
                        break;
                    }
                }
                if (!matched)
                    throw new ArgumentException("Unknown phoneme sequence near: " + text.Substring(index));
            }
            return result;
        }

        private static Syllable BuildSyllable(List<Sound> phonemes, string originalText)
        {
            Sound? onsetN = null;
            var coda = new List<Sound>();
            Sound nucleus = Sound.None;

            foreach (var phoneme in phonemes)
            {
                if (IsVowel(phoneme))
                {
                    if (nucleus != Sound.None)
                        throw new ArgumentException("Syllable has more than one nucleus: " + originalText);
                    nucleus = phoneme;
                }
                else if (nucleus == Sound.None)
                {
                    if (onsetN != null)
                        throw new ArgumentException("Syllable has more than one onset consonant: " + originalText);
                    onsetN = phoneme;
                }
                else
                    coda.Add(phoneme);
            }

            if (nucleus == Sound.None)
                throw new ArgumentException("Syllable has no nucleus: " + originalText);

            if (onsetN == null)
                onsetN = Sound.None;
            var onset = onsetN.Value;

            if (coda.Count > 2)
                throw new Exception("Invalid coda length: " + coda.Join(" "));

            return new Syllable(onset, nucleus, coda.ToArray());
        }
    }
}