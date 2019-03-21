// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations.Schema;

namespace Contoso.FraudProtection.ApplicationCore.Entities
{
    public class CatalogItem : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "Money")]
        public decimal Price { get; set; }
        public string PictureUri { get; set; }
        public int CatalogTypeId { get; set; }
        public CatalogType CatalogType { get; set; }
        public int CatalogBrandId { get; set; }
        public CatalogBrand CatalogBrand { get; set; }
    }
}
