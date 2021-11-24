namespace ContactsViewer.Pages;

public partial class Index
{
    [Inject] public ContactsService ContactsService { get; set; }
    public List<ContactInfo> Contacts { get; set; }

    public async Task Reload()
    {
        Contacts = await ContactsService.GetContacts();
    }

    protected async override Task OnInitializedAsync()
    {
        Contacts = await ContactsService.GetContacts();

        await base.OnInitializedAsync();
    }
}
