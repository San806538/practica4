
using Microsoft.ML.Data;

namespace EcommerceIA.Models{
    public class RatingData
    {
        [LoadColumn(0)]
        public string UserId { get; set; }

        [LoadColumn(1)]
        public string ProductId { get; set; }

        [LoadColumn(2)]
        public float Label { get; set; }
    }

    public class ProductPrediction
    {
        public string ProductId { get; set; }
        public float Score { get; set; }
    }
}