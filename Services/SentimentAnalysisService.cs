using Microsoft.ML;
using Microsoft.ML.Data;
using EcommerceIA.Models;

public class SentimentAnalysisService
{
    private readonly MLContext _mlContext;
    private readonly PredictionEngine<SentimentData, SentimentPrediction> _predictionEngine;

    public SentimentAnalysisService()
    {
        _mlContext = new MLContext();

        var data = _mlContext.Data.LoadFromTextFile<SentimentData>(
            "Data/sentiment-data.tsv", hasHeader: true);

        var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
            .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression());

        var model = pipeline.Fit(data);
        _predictionEngine = _mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
    }

    public SentimentPrediction Predict(string input)
    {
        var sample = new SentimentData { Text = input };
        return _predictionEngine.Predict(sample);
    }
}
