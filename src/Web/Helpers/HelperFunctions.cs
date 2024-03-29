// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Contoso.FraudProtection.Web.Helpers
{
    public static class HelperFunctions
    {
        public static List<SelectListItem> CountryRegionList = new List<SelectListItem>
            {
                new SelectListItem { Value = "AF", Text = "Afghanistan" },
                new SelectListItem { Value = "AX", Text = "�land Islands" },
                new SelectListItem { Value = "AL", Text = "Albania" },
                new SelectListItem { Value = "DZ", Text = "Algeria" },
                new SelectListItem { Value = "AS", Text = "American Samoa" },
                new SelectListItem { Value = "AD", Text = "Andorra" },
                new SelectListItem { Value = "AO", Text = "Angola" },
                new SelectListItem { Value = "AI", Text = "Anguilla" },
                new SelectListItem { Value = "AQ", Text = "Antarctica" },
                new SelectListItem { Value = "AG", Text = "Antigua and Barbuda" },
                new SelectListItem { Value = "AR", Text = "Argentina" },
                new SelectListItem { Value = "AM", Text = "Armenia" },
                new SelectListItem { Value = "AW", Text = "Aruba" },
                new SelectListItem { Value = "AU", Text = "Australia" },
                new SelectListItem { Value = "AT", Text = "Austria" },
                new SelectListItem { Value = "AZ", Text = "Azerbaijan" },
                new SelectListItem { Value = "BS", Text = "Bahamas" },
                new SelectListItem { Value = "BH", Text = "Bahrain" },
                new SelectListItem { Value = "BD", Text = "Bangladesh" },
                new SelectListItem { Value = "BB", Text = "Barbados" },
                new SelectListItem { Value = "BY", Text = "Belarus" },
                new SelectListItem { Value = "BE", Text = "Belgium" },
                new SelectListItem { Value = "BZ", Text = "Belize" },
                new SelectListItem { Value = "BJ", Text = "Benin" },
                new SelectListItem { Value = "BM", Text = "Bermuda" },
                new SelectListItem { Value = "BT", Text = "Bhutan" },
                new SelectListItem { Value = "BO", Text = "Bolivia" },
                new SelectListItem { Value = "BQ", Text = "Bonaire" },
                new SelectListItem { Value = "BA", Text = "Bosnia and Herzegovina" },
                new SelectListItem { Value = "BW", Text = "Botswana" },
                new SelectListItem { Value = "BV", Text = "Bouvet Island" },
                new SelectListItem { Value = "BR", Text = "Brazil" },
                new SelectListItem { Value = "IO", Text = "British Indian Ocean Territory" },
                new SelectListItem { Value = "VG", Text = "British Virgin Islands" },
                new SelectListItem { Value = "BN", Text = "Brunei" },
                new SelectListItem { Value = "BG", Text = "Bulgaria" },
                new SelectListItem { Value = "BF", Text = "Burkina Faso" },
                new SelectListItem { Value = "BI", Text = "Burundi" },
                new SelectListItem { Value = "CV", Text = "Cabo Verde" },
                new SelectListItem { Value = "KH", Text = "Cambodia" },
                new SelectListItem { Value = "CM", Text = "Cameroon" },
                new SelectListItem { Value = "CA", Text = "Canada" },
                new SelectListItem { Value = "KY", Text = "Cayman Islands" },
                new SelectListItem { Value = "CF", Text = "Central African Republic" },
                new SelectListItem { Value = "TD", Text = "Chad" },
                new SelectListItem { Value = "CL", Text = "Chile" },
                new SelectListItem { Value = "CN", Text = "China" },
                new SelectListItem { Value = "CX", Text = "Christmas Island" },
                new SelectListItem { Value = "CC", Text = "Cocos (Keeling) Islands" },
                new SelectListItem { Value = "CO", Text = "Colombia" },
                new SelectListItem { Value = "KM", Text = "Comoros" },
                new SelectListItem { Value = "CG", Text = "Congo" },
                new SelectListItem { Value = "CD", Text = "Congo (DRC)" },
                new SelectListItem { Value = "CK", Text = "Cook Islands" },
                new SelectListItem { Value = "CR", Text = "Costa Rica" },
                new SelectListItem { Value = "CI", Text = "C�te d�Ivoire" },
                new SelectListItem { Value = "HR", Text = "Croatia" },
                new SelectListItem { Value = "CU", Text = "Cuba" },
                new SelectListItem { Value = "CW", Text = "Cura�ao" },
                new SelectListItem { Value = "CY", Text = "Cyprus" },
                new SelectListItem { Value = "CZ", Text = "Czechia" },
                new SelectListItem { Value = "DK", Text = "Denmark" },
                new SelectListItem { Value = "DJ", Text = "Djibouti" },
                new SelectListItem { Value = "DM", Text = "Dominica" },
                new SelectListItem { Value = "DO", Text = "Dominican Republic" },
                new SelectListItem { Value = "EC", Text = "Ecuador" },
                new SelectListItem { Value = "EG", Text = "Egypt" },
                new SelectListItem { Value = "SV", Text = "El Salvador" },
                new SelectListItem { Value = "GQ", Text = "Equatorial Guinea" },
                new SelectListItem { Value = "ER", Text = "Eritrea" },
                new SelectListItem { Value = "EE", Text = "Estonia" },
                new SelectListItem { Value = "SZ", Text = "Eswatini" },
                new SelectListItem { Value = "ET", Text = "Ethiopia" },
                new SelectListItem { Value = "FK", Text = "Falkland Islands" },
                new SelectListItem { Value = "FO", Text = "Faroe Islands" },
                new SelectListItem { Value = "FJ", Text = "Fiji" },
                new SelectListItem { Value = "FI", Text = "Finland" },
                new SelectListItem { Value = "FR", Text = "France" },
                new SelectListItem { Value = "GF", Text = "French Guiana" },
                new SelectListItem { Value = "PF", Text = "French Polynesia" },
                new SelectListItem { Value = "TF", Text = "French Southern Territories" },
                new SelectListItem { Value = "GA", Text = "Gabon" },
                new SelectListItem { Value = "GM", Text = "Gambia" },
                new SelectListItem { Value = "GE", Text = "Georgia" },
                new SelectListItem { Value = "DE", Text = "Germany" },
                new SelectListItem { Value = "GH", Text = "Ghana" },
                new SelectListItem { Value = "GI", Text = "Gibraltar" },
                new SelectListItem { Value = "GR", Text = "Greece" },
                new SelectListItem { Value = "GL", Text = "Greenland" },
                new SelectListItem { Value = "GD", Text = "Grenada" },
                new SelectListItem { Value = "GP", Text = "Guadeloupe" },
                new SelectListItem { Value = "GU", Text = "Guam" },
                new SelectListItem { Value = "GT", Text = "Guatemala" },
                new SelectListItem { Value = "GG", Text = "Guernsey" },
                new SelectListItem { Value = "GN", Text = "Guinea" },
                new SelectListItem { Value = "GW", Text = "Guinea-Bissau" },
                new SelectListItem { Value = "GY", Text = "Guyana" },
                new SelectListItem { Value = "HT", Text = "Haiti" },
                new SelectListItem { Value = "HM", Text = "Heard Island and McDonald Islands" },
                new SelectListItem { Value = "HN", Text = "Honduras" },
                new SelectListItem { Value = "HK", Text = "Hong Kong SAR" },
                new SelectListItem { Value = "HU", Text = "Hungary" },
                new SelectListItem { Value = "IS", Text = "Iceland" },
                new SelectListItem { Value = "IN", Text = "India" },
                new SelectListItem { Value = "ID", Text = "Indonesia" },
                new SelectListItem { Value = "IR", Text = "Iran" },
                new SelectListItem { Value = "IQ", Text = "Iraq" },
                new SelectListItem { Value = "IE", Text = "Ireland" },
                new SelectListItem { Value = "IM", Text = "Isle of Man" },
                new SelectListItem { Value = "IL", Text = "Israel" },
                new SelectListItem { Value = "IT", Text = "Italy" },
                new SelectListItem { Value = "JM", Text = "Jamaica" },
                new SelectListItem { Value = "XJ", Text = "Jan Mayen" },
                new SelectListItem { Value = "JP", Text = "Japan" },
                new SelectListItem { Value = "JE", Text = "Jersey" },
                new SelectListItem { Value = "JO", Text = "Jordan" },
                new SelectListItem { Value = "KZ", Text = "Kazakhstan" },
                new SelectListItem { Value = "KE", Text = "Kenya" },
                new SelectListItem { Value = "KI", Text = "Kiribati" },
                new SelectListItem { Value = "KR", Text = "Korea" },
                new SelectListItem { Value = "XK", Text = "Kosovo" },
                new SelectListItem { Value = "KW", Text = "Kuwait" },
                new SelectListItem { Value = "KG", Text = "Kyrgyzstan" },
                new SelectListItem { Value = "LA", Text = "Laos" },
                new SelectListItem { Value = "LV", Text = "Latvia" },
                new SelectListItem { Value = "LB", Text = "Lebanon" },
                new SelectListItem { Value = "LS", Text = "Lesotho" },
                new SelectListItem { Value = "LR", Text = "Liberia" },
                new SelectListItem { Value = "LY", Text = "Libya" },
                new SelectListItem { Value = "LI", Text = "Liechtenstein" },
                new SelectListItem { Value = "LT", Text = "Lithuania" },
                new SelectListItem { Value = "LU", Text = "Luxembourg" },
                new SelectListItem { Value = "MO", Text = "Macao SAR" },
                new SelectListItem { Value = "MG", Text = "Madagascar" },
                new SelectListItem { Value = "MW", Text = "Malawi" },
                new SelectListItem { Value = "MY", Text = "Malaysia" },
                new SelectListItem { Value = "MV", Text = "Maldives" },
                new SelectListItem { Value = "ML", Text = "Mali" },
                new SelectListItem { Value = "MT", Text = "Malta" },
                new SelectListItem { Value = "MH", Text = "Marshall Islands" },
                new SelectListItem { Value = "MQ", Text = "Martinique" },
                new SelectListItem { Value = "MR", Text = "Mauritania" },
                new SelectListItem { Value = "MU", Text = "Mauritius" },
                new SelectListItem { Value = "YT", Text = "Mayotte" },
                new SelectListItem { Value = "MX", Text = "Mexico" },
                new SelectListItem { Value = "FM", Text = "Micronesia" },
                new SelectListItem { Value = "MD", Text = "Moldova" },
                new SelectListItem { Value = "MC", Text = "Monaco" },
                new SelectListItem { Value = "MN", Text = "Mongolia" },
                new SelectListItem { Value = "ME", Text = "Montenegro" },
                new SelectListItem { Value = "MS", Text = "Montserrat" },
                new SelectListItem { Value = "MA", Text = "Morocco" },
                new SelectListItem { Value = "MZ", Text = "Mozambique" },
                new SelectListItem { Value = "MM", Text = "Myanmar" },
                new SelectListItem { Value = "NA", Text = "Namibia" },
                new SelectListItem { Value = "NR", Text = "Nauru" },
                new SelectListItem { Value = "NP", Text = "Nepal" },
                new SelectListItem { Value = "NL", Text = "Netherlands" },
                new SelectListItem { Value = "NC", Text = "New Caledonia" },
                new SelectListItem { Value = "NZ", Text = "New Zealand" },
                new SelectListItem { Value = "NI", Text = "Nicaragua" },
                new SelectListItem { Value = "NE", Text = "Niger" },
                new SelectListItem { Value = "NG", Text = "Nigeria" },
                new SelectListItem { Value = "NU", Text = "Niue" },
                new SelectListItem { Value = "NF", Text = "Norfolk Island" },
                new SelectListItem { Value = "KP", Text = "North Korea" },
                new SelectListItem { Value = "MK", Text = "North Macedonia" },
                new SelectListItem { Value = "MP", Text = "Northern Mariana Islands" },
                new SelectListItem { Value = "NO", Text = "Norway" },
                new SelectListItem { Value = "OM", Text = "Oman" },
                new SelectListItem { Value = "PK", Text = "Pakistan" },
                new SelectListItem { Value = "PW", Text = "Palau" },
                new SelectListItem { Value = "PS", Text = "Palestinian Authority" },
                new SelectListItem { Value = "PA", Text = "Panama" },
                new SelectListItem { Value = "PG", Text = "Papua New Guinea" },
                new SelectListItem { Value = "PY", Text = "Paraguay" },
                new SelectListItem { Value = "PE", Text = "Peru" },
                new SelectListItem { Value = "PH", Text = "Philippines" },
                new SelectListItem { Value = "PN", Text = "Pitcairn Islands" },
                new SelectListItem { Value = "PL", Text = "Poland" },
                new SelectListItem { Value = "PT", Text = "Portugal" },
                new SelectListItem { Value = "PR", Text = "Puerto Rico" },
                new SelectListItem { Value = "QA", Text = "Qatar" },
                new SelectListItem { Value = "RE", Text = "R�union" },
                new SelectListItem { Value = "RO", Text = "Romania" },
                new SelectListItem { Value = "RU", Text = "Russia" },
                new SelectListItem { Value = "RW", Text = "Rwanda" },
                new SelectListItem { Value = "XS", Text = "Saba" },
                new SelectListItem { Value = "BL", Text = "Saint Barth�lemy" },
                new SelectListItem { Value = "KN", Text = "Saint Kitts and Nevis" },
                new SelectListItem { Value = "LC", Text = "Saint Lucia" },
                new SelectListItem { Value = "MF", Text = "Saint Martin" },
                new SelectListItem { Value = "PM", Text = "Saint Pierre and Miquelon" },
                new SelectListItem { Value = "VC", Text = "Saint Vincent and the Grenadines" },
                new SelectListItem { Value = "WS", Text = "Samoa" },
                new SelectListItem { Value = "SM", Text = "San Marino" },
                new SelectListItem { Value = "ST", Text = "S�o Tom� and Pr�ncipe" },
                new SelectListItem { Value = "SA", Text = "Saudi Arabia" },
                new SelectListItem { Value = "SN", Text = "Senegal" },
                new SelectListItem { Value = "RS", Text = "Serbia" },
                new SelectListItem { Value = "SC", Text = "Seychelles" },
                new SelectListItem { Value = "SL", Text = "Sierra Leone" },
                new SelectListItem { Value = "SG", Text = "Singapore" },
                new SelectListItem { Value = "XE", Text = "Sint Eustatius" },
                new SelectListItem { Value = "SX", Text = "Sint Maarten" },
                new SelectListItem { Value = "SK", Text = "Slovakia" },
                new SelectListItem { Value = "SI", Text = "Slovenia" },
                new SelectListItem { Value = "SB", Text = "Solomon Islands" },
                new SelectListItem { Value = "SO", Text = "Somalia" },
                new SelectListItem { Value = "ZA", Text = "South Africa" },
                new SelectListItem { Value = "GS", Text = "South Georgia and South Sandwich Islands" },
                new SelectListItem { Value = "SS", Text = "South Sudan" },
                new SelectListItem { Value = "ES", Text = "Spain" },
                new SelectListItem { Value = "LK", Text = "Sri Lanka" },
                new SelectListItem { Value = "SH", Text = "St Helena, Ascension, Tristan da Cunha" },
                new SelectListItem { Value = "SD", Text = "Sudan" },
                new SelectListItem { Value = "SR", Text = "Suriname" },
                new SelectListItem { Value = "SJ", Text = "Svalbard" },
                new SelectListItem { Value = "SE", Text = "Sweden" },
                new SelectListItem { Value = "CH", Text = "Switzerland" },
                new SelectListItem { Value = "SY", Text = "Syria" },
                new SelectListItem { Value = "TW", Text = "Taiwan" },
                new SelectListItem { Value = "TJ", Text = "Tajikistan" },
                new SelectListItem { Value = "TZ", Text = "Tanzania" },
                new SelectListItem { Value = "TH", Text = "Thailand" },
                new SelectListItem { Value = "TL", Text = "Timor-Leste" },
                new SelectListItem { Value = "TG", Text = "Togo" },
                new SelectListItem { Value = "TK", Text = "Tokelau" },
                new SelectListItem { Value = "TO", Text = "Tonga" },
                new SelectListItem { Value = "TT", Text = "Trinidad and Tobago" },
                new SelectListItem { Value = "TN", Text = "Tunisia" },
                new SelectListItem { Value = "TR", Text = "Turkey" },
                new SelectListItem { Value = "TM", Text = "Turkmenistan" },
                new SelectListItem { Value = "TC", Text = "Turks and Caicos Islands" },
                new SelectListItem { Value = "TV", Text = "Tuvalu" },
                new SelectListItem { Value = "UM", Text = "U.S. Outlying Islands" },
                new SelectListItem { Value = "VI", Text = "U.S. Virgin Islands" },
                new SelectListItem { Value = "UG", Text = "Uganda" },
                new SelectListItem { Value = "UA", Text = "Ukraine" },
                new SelectListItem { Value = "AE", Text = "United Arab Emirates" },
                new SelectListItem { Value = "GB", Text = "United Kingdom" },
                new SelectListItem { Value = "US", Text = "United States" },
                new SelectListItem { Value = "UY", Text = "Uruguay" },
                new SelectListItem { Value = "UZ", Text = "Uzbekistan" },
                new SelectListItem { Value = "VU", Text = "Vanuatu" },
                new SelectListItem { Value = "VA", Text = "Vatican City" },
                new SelectListItem { Value = "VE", Text = "Venezuela" },
                new SelectListItem { Value = "VN", Text = "Vietnam" },
                new SelectListItem { Value = "WF", Text = "Wallis and Futuna" },
                new SelectListItem { Value = "YE", Text = "Yemen" },
                new SelectListItem { Value = "ZM", Text = "Zambia" },
                new SelectListItem { Value = "ZW", Text = "Zimbabwe" },
            };

        public static List<SelectListItem> CardTypeList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Card Type" },
                new SelectListItem { Value = "Visa", Text = "Visa" },
                new SelectListItem { Value = "Mastercard", Text = "Mastercard" },
                new SelectListItem { Value = "Amex", Text = "Amex" },
                new SelectListItem { Value = "Discover", Text = "Discover" }
            };

        public static List<SelectListItem> PIMonthList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Month" },
                new SelectListItem { Value = "01", Text = "January" },
                new SelectListItem { Value = "02", Text = "February" },
                new SelectListItem { Value = "03", Text = "March" },
                new SelectListItem { Value = "04", Text = "April" },
                new SelectListItem { Value = "05", Text = "May" },
                new SelectListItem { Value = "06", Text = "June" },
                new SelectListItem { Value = "07", Text = "July" },
                new SelectListItem { Value = "08", Text = "August" },
                new SelectListItem { Value = "09", Text = "September" },
                new SelectListItem { Value = "10", Text = "October" },
                new SelectListItem { Value = "11", Text = "November" },
                new SelectListItem { Value = "12", Text = "December" },
            };

        public static List<SelectListItem> PIYearList = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "Year" },
                new SelectListItem { Value = DateTimeOffset.Now.Year.ToString().Substring(2, 2), Text = DateTimeOffset.Now.Year.ToString() },
                new SelectListItem { Value = DateTimeOffset.Now.AddYears(1).ToString() , Text = DateTimeOffset.Now.AddYears(1).Year.ToString() },
                new SelectListItem { Value = DateTimeOffset.Now.AddYears(2).ToString() , Text = DateTimeOffset.Now.AddYears(2).Year.ToString() },
                new SelectListItem { Value = DateTimeOffset.Now.AddYears(3).ToString() , Text = DateTimeOffset.Now.AddYears(3).Year.ToString() },
                new SelectListItem { Value = DateTimeOffset.Now.AddYears(4).ToString() , Text = DateTimeOffset.Now.AddYears(4).Year.ToString() },
            };
    }
}
