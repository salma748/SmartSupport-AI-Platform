using SmartSupport_AI.BLL;

namespace SmartSupport_AI.PL;

internal static class GeminiConnectivityCheck
{
    public static async Task<int> RunAsync(HttpClient client, AIProviderOptions options, string? key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            Console.Error.WriteLine("GEMINI_API_KEY is missing or empty.");
            return 2;
        }

        var provider = new GeminiAIProvider(client,
            new AIProviderOptions { Model = options.Model, MaxOutputTokens = options.MaxOutputTokens, AllowCustomerContent = true }, key);
        var answer = await new AIOrchestrator(provider).AnswerAsync("What is your warranty policy?",
            [new KnowledgeSource(1, "Synthetic warranty fixture", "The example warranty lasts one year from purchase.")],
            CancellationToken.None);
        if (answer?.ProviderStatus != "VerifiedExcerpt")
        {
            Console.Error.WriteLine($"Gemini agent verification failed ({answer?.ProviderStatus ?? "No answer"}). Check model access, key restrictions, or quota.");
            return 1;
        }

        Console.WriteLine("Gemini connectivity and the synthetic knowledge-tool round trip passed.");
        return 0;
    }
}
