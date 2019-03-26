// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace System
{
    public static class FormattingExtensions
    {
        public static string AsMoney(this decimal value)
        {
            return "$ " + Math.Round(value, 2).ToString("N2");
        }
    }
}
