// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;

namespace Contoso.FraudProtection.ApplicationCore.Entities.OrderAggregate
{
    /// <summary>
    /// Represents a snapshot of the item that was ordered. If catalog item details change, details of
    /// the item that was part of a completed order should not change.
    /// </summary>
    public class CatalogItemOrdered // ValueObject
    {
        public CatalogItemOrdered(int catalogItemId, string productName, string pictureUri)
        {
            if (catalogItemId < 1)
                throw new ArgumentOutOfRangeException(nameof(catalogItemId));
            if (string.IsNullOrEmpty(pictureUri))
                throw new ArgumentNullException(nameof(pictureUri));
            if (string.IsNullOrEmpty(productName))
                throw new ArgumentNullException(nameof(productName));

            CatalogItemId = catalogItemId;
            ProductName = productName;
            PictureUri = pictureUri;
        }

        private CatalogItemOrdered()
        {
            // required by EF
        }

        public int CatalogItemId { get; private set; }
        public string ProductName { get; private set; }
        public string PictureUri { get; private set; }
    }
}
