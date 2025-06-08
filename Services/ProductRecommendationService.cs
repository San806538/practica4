using Microsoft.ML;
using Microsoft.ML.Trainers;
using System.Collections.Generic;
using System.Linq;
using EcommerceIA.Models;

public class ProductRecommendationService
{
    private readonly MLContext _mlContext;
    private readonly ITransformer _model;

    public ProductRecommendationService()
    {
        _mlContext = new MLContext();

        var data = _mlContext.Data.LoadFromTextFile<RatingData>(
            "Data/ratings-data.csv", hasHeader: true, separatorChar: ',');

        var options = new MatrixFactorizationTrainer.Options
        {
            MatrixColumnIndexColumnName = nameof(RatingData.UserId),
            MatrixRowIndexColumnName = nameof(RatingData.ProductId),
            LabelColumnName = nameof(RatingData.Label),
            NumberOfIterations = 20,
            ApproximationRank = 100
        };

        var pipeline = _mlContext.Transforms.Conversion
            .MapValueToKey(nameof(RatingData.UserId))
            .Append(_mlContext.Transforms.Conversion.MapValueToKey(nameof(RatingData.ProductId)))
            .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(options));
        _model = pipeline.Fit(data);
    }

    public List<ProductPrediction> RecommendProducts(string userId, List<string> allProducts)
    {
        var predictionEngine = _mlContext.Model.CreatePredictionEngine<RatingData, ScoreResult>(_model);

        return allProducts
            .Select(p => new RatingData { UserId = userId, ProductId = p })
            .Select(p => new ProductPrediction
            {
                ProductId = p.ProductId,
                Score = predictionEngine.Predict(p).Score
            })
            .OrderByDescending(p => p.Score)
            .Take(5)
            .ToList();
    }

    private class ScoreResult
    {
        public float Score { get; set; }
    }
}
