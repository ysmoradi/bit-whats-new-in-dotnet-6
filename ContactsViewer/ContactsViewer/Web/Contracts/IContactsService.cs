namespace ContactsViewer.App.Contracts;

public interface IContactsService
{
    Task<List<ContactInfo>> GetContacts();
}
