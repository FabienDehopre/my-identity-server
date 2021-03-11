namespace Dehopre.Domain.Core.Util
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        private static readonly char Sensitive = '*';
        private static readonly char At = '@';

        public static string UrlEncode(this string url) => Uri.EscapeDataString(url);
        public static bool NotEqual(this string original, string compareTo) => !original.Equals(compareTo);
        public static bool IsEmail(this string field) =>
            field.IsPresent() && Regex.IsMatch(field, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

        public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        public static bool IsMissing(this string value) => string.IsNullOrEmpty(value);

        public static bool IsPresent(this string value) => !string.IsNullOrWhiteSpace(value);

        private static string UrlCombine(string path1, string path2)
        {
            path1 = path1.TrimEnd('/') + "/";
            path2 = path2.TrimStart('/');

            return Path.Combine(path1, path2)
                .Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        public static string UrlPathCombine(this string path1, params string[] path2)
        {
            path1 = path1.TrimEnd('/') + "/";
            foreach (var s in path2)
            {
                path1 = UrlCombine(path1, s).Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }

            return path1;

        }

        public static string AddSpacesToSentence(this string state)
        {
            var text = state.ToCharArray();
            var chars = new char[text.Length + HowManyCapitalizedChars(text) - 1];

            chars[0] = text[0];
            var j = 1;
            for (var i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                {
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (char.IsUpper(text[i - 1]) && i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                    {
                        chars[j++] = ' ';
                        chars[j++] = text[i];
                        continue;
                    }
                }

                chars[j++] = text[i];
            }

            return new string(chars.AsSpan());
        }
        private static int HowManyCapitalizedChars(char[] state)
        {
            var count = 0;
            for (var i = 0; i < state.Length; i++)
            {
                if (char.IsUpper(state[i]))
                {
                    count++;
                }
            }

            return count;
        }

        public static bool HasTrailingSlash(this string url) => url is not null && url.EndsWith("/");


        public static string TruncateSensitiveInformation(this string part) => part.AsSpan().TruncateSensitiveInformation();

        public static string TruncateSensitiveInformation(this ReadOnlySpan<char> part)
        {
            var firstAndLastLetterBuffer = new char[2];
            var firstAndLastLetter = new Span<char>(firstAndLastLetterBuffer);

            if (part != "")
            {
                part.Slice(0, 1).CopyTo(firstAndLastLetter.Slice(0, 1));
                part[^1..].CopyTo(firstAndLastLetter[1..]);

                return string.Create(part.Length, firstAndLastLetterBuffer, (span, s) =>
                {
                    s.AsSpan(0, 1).CopyTo(span);
                    for (var i = 1; i < span.Length - 1; i++)
                    {
                        span[i] = Sensitive;
                    }
                    s.AsSpan(s.Length - 1).CopyTo(span[^1..]);
                });
            }
            else
            {
                return "";
            }

        }

        public static string TruncateEmail(this string email)
        {
            var beforeAt = email.AsSpan(0, email.IndexOf(At)).TruncateSensitiveInformation().AsSpan();
            var afterAt = email.AsSpan(email.IndexOf(At) + 1).TruncateSensitiveInformation().AsSpan();

            var finalSpan = new Span<char>(new char[email.Length]);

            beforeAt.CopyTo(finalSpan);
            finalSpan[beforeAt.Length] = At;
            afterAt.CopyTo(finalSpan[(beforeAt.Length + 1)..]);

            return finalSpan.ToString();
        }
    }
}
