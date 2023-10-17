using iText.Kernel.Geom;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using ProspectLabApp.Pages.Entities;

namespace ProspectLabApp.Pages
{
    public class ProductExtractor
    {
        public static List<Product> ExtractProducts(string filePath, string excludedPath, string leafletName)
        {
            var text = File.ReadAllText(filePath);
            var excluded = File.ReadAllText(excludedPath);

            string[] delimeters = new string[] { "\n", "\r\n" };
            // First list that gets converted from pdf
            List<string> unfilteredList = text.Split(delimeters, StringSplitOptions.RemoveEmptyEntries).ToList();
            // Concatenating the integers that have a , right after them and another integer after
            List<string> concatInt = new List<string>();
            // List of strings that are excluded
            List<string> excludedStrings = excluded.Split(delimeters, StringSplitOptions.RemoveEmptyEntries).ToList();
            // Filtering of words of 4 words or more
            List<string> threeWordsList = new List<string>();
            // List of strings without the strings that are excluded
            List<string> withoutExcludedList = new List<string>();
            // List with the titles concatenated
            List<string> concatTitlesList = new List<string>();


            List<Product> products = new List<Product>();

            string brandPattern = @"[A-Z]+[- ]*[A-Z]+";
            string titlePattern = @"((?!.*%)(?<!\d|\p{P})[A-ZĂÂÎȘȚ][a-zăâîșț\s-]+)";
            string continuationTitlePattern = @"\b•?[a-zăâîșț](?!\d+\s?[a-zăâîșțA-ZĂÂÎȘȚ]+\b)[a-zăâîșțA-ZĂÂÎȘȚ0-9 -]*\b";
            string quantityPattern = @"(•?\s?(per|caserolă|[0-9,.]+)\s*(kg|L|g|buc\.|ml|capsule|set|bucăți|grăsime))";
            string otherQuantityPattern = @"•?\s*\b(\d+|\(\d+\+\d+\))\s*x\s*\d+([,.]\d+)?\s*L\b";
            string discountPattern = @"-[0-9]+%$";
            string pricePattern = @"(?<![a-zA-Z])[0-9]+,[0-9]+(?![a-zA-Z])";

            for (int i = 0; i < unfilteredList.Count; i++)
            {
                string currentString = unfilteredList[i];
                if (currentString == ",")
                {
                    currentString = unfilteredList[i - 1] + currentString + unfilteredList[i + 1];
                    concatInt.Add(currentString);
                    i++;
                }
                else if (Regex.IsMatch(currentString, @"^\d+,$") && i + 1 < unfilteredList.Count)
                {
                    currentString += unfilteredList[i + 1];
                    concatInt.Add(currentString);
                    i++;
                }
                else if (i > 0 && unfilteredList[i - 1] != ",")
                {
                    if (i + 1 < unfilteredList.Count && unfilteredList[i + 1] == ",")
                    {
                        continue;
                    }
                    concatInt.Add(currentString);
                }
            }

            for (int i = 0; i < concatInt.Count; i++)
            {
                string line = concatInt[i].Trim();
                int wordCount = line.Split(' ').Length;
                if (wordCount > 3)
                {
                    if (Regex.IsMatch(line, otherQuantityPattern))
                    {
                        threeWordsList.Add(line);
                    }
                    continue;
                }
                threeWordsList.Add(line);
            }

            foreach (var item in threeWordsList)
            {
                if (excludedStrings.Contains(item))
                {
                    continue;
                }

                withoutExcludedList.Add(item);
            }

            for (int i = 0; i < withoutExcludedList.Count; i++)
            {
                string currentString = withoutExcludedList[i];

                if (currentString.All(c => char.IsLetter(c) || char.IsWhiteSpace(c) || char.IsPunctuation(c)) && !Regex.IsMatch(currentString, @"[A-Z]+[- ]*[A-Z]+"))
                {
                    while (i + 1 < withoutExcludedList.Count && withoutExcludedList[i + 1].All(c => char.IsLetter(c) || char.IsWhiteSpace(c) || char.IsPunctuation(c))
                                                             && !Regex.IsMatch(withoutExcludedList[i + 1], @"[A-Z]+[- ]*[A-Z]+"))
                    {
                        currentString += " " + withoutExcludedList[i + 1];
                        i++;
                    }
                }
                concatTitlesList.Add(currentString);
            }

            Product product = new Product();
            for (int i = 0; i < concatTitlesList.Count; i++)
            {
                product = new Product();

                switch (concatTitlesList[i].Trim())
                {
                    // Search for brand
                    case string brand when Regex.IsMatch(brand, brandPattern) || brand == "esmara®" || brand == "W5":
                        product.Brand = brand;
                        i++;
                        break;
                    default:
                        product.Brand = null;
                        break;
                }

                switch (concatTitlesList[i].Trim())
                {
                    // Search for title
                    case string s when Regex.IsMatch(s, titlePattern):
                        product.Title = s;
                        i++;
                        while (i < concatTitlesList.Count && Regex.IsMatch(concatTitlesList[i].Trim(), continuationTitlePattern) && !Regex.IsMatch(concatTitlesList[i].Trim(), quantityPattern))
                        {
                            product.Title += " " + concatTitlesList[i].Trim();
                            i++;
                        }
                        switch (concatTitlesList[i].Trim())
                        {
                            // Search for Discount within Title
                            case string st when Regex.IsMatch(st, discountPattern):
                                product.Discount = st;
                                i++;

                                switch (concatTitlesList[i].Trim())
                                {
                                    // Search for Quantity within Discount - Title
                                    case string str when Regex.IsMatch(str, quantityPattern):
                                        product.Quantity = str;
                                        i++;

                                        switch (concatTitlesList[i].Trim())
                                        {
                                            // Search for Price within Quantity - Discount - Title
                                            case string stri when Regex.IsMatch(stri, pricePattern):
                                                if (i + 1 < concatTitlesList.Count && Regex.IsMatch(concatTitlesList[i + 1].Trim(), pricePattern) && !Regex.IsMatch(concatTitlesList[i + 1].Trim(), quantityPattern))
                                                {
                                                    decimal price1 = decimal.Parse(stri.Replace(",", "."));
                                                    decimal price2 = decimal.Parse(concatTitlesList[i + 1].Trim().Replace(",", "."));
                                                    product.Price = Math.Min(price1, price2);
                                                    i++;
                                                }
                                                else
                                                {
                                                    product.Price = decimal.Parse(stri.Replace(",", "."));
                                                }
                                                break;
                                            default:
                                                i--;
                                                break;
                                        }
                                        break;
                                    // Search for Price within Discount - Title
                                    case string str when Regex.IsMatch(str, pricePattern):
                                        if (i + 1 < concatTitlesList.Count && Regex.IsMatch(concatTitlesList[i + 1].Trim(), pricePattern) && !Regex.IsMatch(concatTitlesList[i + 1].Trim(), quantityPattern))
                                        {
                                            decimal price1 = decimal.Parse(str.Replace(",", "."));
                                            decimal price2 = decimal.Parse(concatTitlesList[i + 1].Trim().Replace(",", "."));
                                            product.Price = Math.Min(price1, price2);
                                            i++;
                                        }
                                        else
                                        {
                                            product.Price = decimal.Parse(str.Replace(",", "."));
                                        }
                                        i++;
                                        switch (concatTitlesList[i].Trim())
                                        {
                                            case string stri when Regex.IsMatch(stri, quantityPattern):
                                                product.Quantity = stri;
                                                break;
                                        }
                                        break;
                                }
                                break;
                            // Search for Quantity within Title
                            case string st when Regex.IsMatch(st, quantityPattern):
                                product.Quantity = st;
                                i++;
                                switch (concatTitlesList[i].Trim())
                                {
                                    // Search for Discount within Quantity - Title
                                    case string str when Regex.IsMatch(str, discountPattern):
                                        product.Discount = str;
                                        i++;
                                        switch (concatTitlesList[i].Trim())
                                        {
                                            // Search for Price within Discount - Quantity - Title
                                            case string stri when Regex.IsMatch(stri, pricePattern):
                                                if (i + 1 < concatTitlesList.Count && Regex.IsMatch(concatTitlesList[i + 1].Trim(), pricePattern) && !Regex.IsMatch(concatTitlesList[i + 1].Trim(), quantityPattern))
                                                {
                                                    decimal price1 = decimal.Parse(stri.Replace(",", "."));
                                                    decimal price2 = decimal.Parse(concatTitlesList[i + 1].Trim().Replace(",", "."));
                                                    product.Price = Math.Min(price1, price2);
                                                    i++;
                                                }
                                                else
                                                {
                                                    product.Price = decimal.Parse(stri.Replace(",", "."));
                                                }
                                                break;
                                            default:
                                                i--;
                                                break;
                                        }
                                        break;
                                    // Search for Price within Quantity - Title
                                    case string str when Regex.IsMatch(str, pricePattern):
                                        if (i + 1 < concatTitlesList.Count && Regex.IsMatch(concatTitlesList[i + 1].Trim(), pricePattern) && !Regex.IsMatch(concatTitlesList[i + 1].Trim(), quantityPattern))
                                        {
                                            decimal price1 = decimal.Parse(str.Replace(",", "."));
                                            decimal price2 = decimal.Parse(concatTitlesList[i + 1].Trim().Replace(",", "."));
                                            product.Price = Math.Min(price1, price2);
                                            i++;
                                        }
                                        else
                                        {
                                            product.Price = decimal.Parse(str.Replace(",", "."));
                                        }
                                        i++;
                                        switch (concatTitlesList[i].Trim())
                                        {
                                            //Search for Discount within Price - Quantity - Title
                                            case string stri when Regex.IsMatch(str, discountPattern):
                                                product.Discount = stri;
                                                break;
                                            default: i--; break;
                                        }
                                        break;
                                    default:
                                        i--;
                                        break;
                                }
                                break;
                            // Search for Price within Title
                            case string st when Regex.IsMatch(st, pricePattern):
                                if (i + 1 < concatTitlesList.Count && Regex.IsMatch(concatTitlesList[i + 1].Trim(), pricePattern) && !Regex.IsMatch(concatTitlesList[i + 1].Trim(), quantityPattern))
                                {
                                    decimal price1 = decimal.Parse(st.Replace(",", "."));
                                    decimal price2 = decimal.Parse(concatTitlesList[i + 1].Trim().Replace(",", "."));
                                    product.Price = Math.Min(price1, price2);
                                    i++;
                                }
                                else
                                {
                                    product.Price = decimal.Parse(st.Replace(",", "."));
                                }
                                i++;
                                switch (concatTitlesList[i].Trim())
                                {
                                    // Search for Discount within Price - Title
                                    case string str when Regex.IsMatch(str, discountPattern):
                                        product.Discount = str;
                                        i++;
                                        switch (concatTitlesList[i].Trim())
                                        {
                                            // Search for Quantity within Discount - Price - Title
                                            case string stri when Regex.IsMatch(stri, quantityPattern):
                                                product.Quantity = stri;
                                                break;
                                            default:
                                                i--;
                                                break;
                                        }
                                        break;
                                    // Search for Quantity within Price - Title
                                    case string str when Regex.IsMatch(str, quantityPattern):
                                        product.Quantity = str;
                                        i++;
                                        switch (concatTitlesList[i].Trim())
                                        {
                                            // Search for Discount within Quantity - Price - Title
                                            case string stri when Regex.IsMatch(stri, discountPattern):
                                                product.Discount = stri;
                                                break;
                                            default: i--; break;
                                        }
                                        break;
                                    default:
                                        i--;
                                        break;
                                }
                                break;
                            default:
                                i--;
                                break;
                        }
                        break;
                    default:
                        product.Title = null;
                        break;
                }

                if (product.Title != null && product.Price != 0)
                {
                    product.LeafletName = leafletName;
                    products.Add(product);
                }
            }
            return products;
        }
    }
}

