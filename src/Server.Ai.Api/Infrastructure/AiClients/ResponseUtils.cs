namespace Server.Ai.Api.Infrastructure.AiClients;

public static class ResponseUtils
{
    public static bool CodeBlockFound(string responseMessage) => responseMessage.StartsWith("```");
    public static string RemoveCodeBlock(string responseMessage)
    {
        if (string.IsNullOrWhiteSpace(responseMessage))
            return responseMessage;

        string trimmed = responseMessage.Trim();

        // Check if the string starts with the markdown code fence
        if (trimmed.StartsWith("```"))
        {
            // Find the end of the opening fence (e.g., ```json\n)
            int endOfOpeningFence = trimmed.IndexOf('\n');

            // If no newline, just skip the 3 backticks; 
            // otherwise, skip the entire first line (the fence + language identifier)
            int startPath = (endOfOpeningFence == -1) ? 3 : endOfOpeningFence + 1;

            // Find the last occurrence of the closing fence
            int lastFence = trimmed.LastIndexOf("```");

            if (lastFence > startPath)
            {
                // Extract everything between the fences
                trimmed = trimmed.Substring(startPath, lastFence - startPath).Trim();
            }
            else
            {
                // If there's no closing fence, just take everything after the opening
                trimmed = trimmed.Substring(startPath).Trim();
            }
        }

        return trimmed;
    }
}