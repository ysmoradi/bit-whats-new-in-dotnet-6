namespace ContactViewer.Services;

public class ContactsService
{
    public async Task<List<ContactPerson>> GetContacts()
    {
        if (await Permissions.CheckStatusAsync<Permissions.ContactsRead>() != PermissionStatus.Granted)
            await Permissions.RequestAsync<Permissions.ContactsRead>();

        throw new NotImplementedException();
    }
}