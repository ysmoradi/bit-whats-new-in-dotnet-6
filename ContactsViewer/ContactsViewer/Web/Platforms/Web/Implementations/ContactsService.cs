namespace ContactsViewer.App.Platforms.Web.Implementations;

public class ContactsService : IContactsService
{
    public async Task<List<ContactInfo>> GetContacts()
    {
        // use http client to get synced data from server

        List<ContactInfo> result = new();

        for (int i = 0; i < 100; i++)
        {
            result.Add(new() { DisplayName = $"Test Contact {i}" });
        }

        return result;
    }
}
