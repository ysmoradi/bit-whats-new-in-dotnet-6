namespace System.IO;

public static class StreamExtensions
{
    public static async Task<string> ConvertToBase64Image(this Stream stream)
    {
        byte[] bytes;

        await using (MemoryStream memoryStream = new())
        {
            await stream.CopyToAsync(memoryStream);
            bytes = memoryStream.ToArray();
        }

        string base64 = Convert.ToBase64String(bytes);

        return $"data:image/jpg;base64, {base64}";
    }
}
